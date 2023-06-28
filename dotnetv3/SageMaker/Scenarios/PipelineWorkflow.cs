// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
// SPDX-License-Identifier:  Apache-2.0

using System.Text.Json;
using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;
using Amazon.Extensions.NETCore.Setup;
using Amazon.IdentityManagement;
using Amazon.IdentityManagement.Model;
using Amazon.Lambda;
using Amazon.Lambda.Model;
using Amazon.Runtime.Internal.Util;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.SageMaker;
using Amazon.SageMaker.Model;
using Amazon.SageMakerGeospatial;
using Amazon.SageMakerGeospatial.Model;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging.Debug;
using SageMakerActions;
using Host = Microsoft.Extensions.Hosting.Host;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using ResourceNotFoundException = Amazon.Lambda.Model.ResourceNotFoundException;


namespace SageMakerScenario;

public class PipelineWorkflow
{
    private static ILogger logger = null!;
    private static IAmazonIdentityManagementService _iamClient;
    private static SageMakerWrapper _sageMakerWrapper;
    private static IAmazonSQS _sqsClient;
    private static IAmazonS3 _s3Client;
    private static IAmazonLambda _lambdaClient;
    private static IAmazonSageMaker _sageMakerClient;
    private static IConfiguration _configuration = null!;

    // TODO replace this with uploading the function directly
    //private static string functionArn = "arn:aws:lambda:us-west-2:565846806325:function:SageMakerVectorLambda";
    private static string lambdaFunctionName = "SageMakerExampleFunction";
    private static string sageMakerRoleName = "SageMakerExampleRole";
    private static string lambdaRoleName = "SageMakerExampleLambdaRole";


    static async Task Main(string[] args)
    {
        // Set up dependency injection for the Amazon service.
        using var host = Host.CreateDefaultBuilder(args)
            .ConfigureLogging(logging =>
                logging.AddFilter("System", LogLevel.Debug)
                    .AddFilter<DebugLoggerProvider>("Microsoft", LogLevel.Information)
                    .AddFilter<ConsoleLoggerProvider>("Microsoft", LogLevel.Trace))
            .ConfigureServices((_, services) =>
                services.AddAWSService<IAmazonIdentityManagementService>()
                    .AddAWSService<IAmazonEC2>(new AWSOptions(){Region = RegionEndpoint.USWest2})
                    .AddAWSService<IAmazonSageMaker>(new AWSOptions() { Region = RegionEndpoint.USWest2 })
                    .AddAWSService<IAmazonSageMakerGeospatial>(new AWSOptions() { Region = RegionEndpoint.USWest2 })
                    .AddAWSService<IAmazonSQS>(new AWSOptions() { Region = RegionEndpoint.USWest2 })
                    .AddAWSService<IAmazonS3>(new AWSOptions() { Region = RegionEndpoint.USWest2 })
                    .AddAWSService<IAmazonLambda>(new AWSOptions() { Region = RegionEndpoint.USWest2 })
                    .AddTransient<SageMakerWrapper>()
        )
        .Build();

        _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("settings.json") // Load settings from .json file.
            .AddJsonFile("settings.local.json",
                true) // Optionally, load local settings.
            .Build();

        logger = LoggerFactory.Create(builder => { builder.AddConsole(); })
            .CreateLogger<PipelineWorkflow>();

        ServicesSetup(host);
        var lambdaRoleArn = await CreateLambdaRole();
        var sageMakerRoleArn = await CreateSageMakerRole();
        var functionArn = await SetupLambda(lambdaRoleArn);
        var queueUrl = await SetupQueue();
        await SetupPipeline(sageMakerRoleArn, functionArn);
        await ExecutePipeline(queueUrl);
        await CleanupResources(queueUrl);

    }

    /// <summary>
    /// Populate the services for use within the console application.
    /// </summary>
    /// <param name="host">The services host.</param>
    private static void ServicesSetup(IHost host)
    {
        _sageMakerWrapper = host.Services.GetRequiredService<SageMakerWrapper>();
        _iamClient = host.Services.GetRequiredService<IAmazonIdentityManagementService>();
        _sqsClient = host.Services.GetRequiredService<IAmazonSQS>();
        _s3Client = host.Services.GetRequiredService<IAmazonS3>();
        _lambdaClient = host.Services.GetRequiredService<IAmazonLambda>();
        _sageMakerClient = host.Services.GetRequiredService<IAmazonSageMaker>();
    }

    /// <summary>
    /// Set up the AWS Lambda, either by updating an existing function or creating a new function.
    /// </summary>
    /// <param name="roleArn">The role ARN to use for the Lambda function.</param>
    /// <returns>The ARN of the function.</returns>
    public static async Task<string> SetupLambda(string roleArn)
    {
        Console.WriteLine(new string('-', 80));
        Console.WriteLine("Setting up the Lambda function for the pipeline.");
        var handlerName = "SageMakerLambda::SageMakerLambda.SageMakerLambdaFunction::FunctionHandler";
        try
        {
            var functionInfo = await _lambdaClient.GetFunctionAsync(new GetFunctionRequest()
            {
                FunctionName = lambdaFunctionName
            });

            var updateResponse = GetYesNoResponse($"The Lambda function {lambdaFunctionName} already exists, do you want to update it?");

            if (updateResponse)
            {
                // Update the Lambda function.
                using var zipMemoryStream = new MemoryStream(await File.ReadAllBytesAsync("SageMakerLambda.zip"));
                await _lambdaClient.UpdateFunctionCodeAsync(
                    new UpdateFunctionCodeRequest()
                    {
                        FunctionName = lambdaFunctionName,
                        ZipFile = zipMemoryStream,
                    });
            }
            Console.WriteLine(new string('-', 80));
            return functionInfo.Configuration.FunctionArn;
        }
        catch (ResourceNotFoundException)
        {
            Console.WriteLine($"\tThe Lambda function {lambdaFunctionName} was not found, creating the new function.");

            // Create the function if it does not already exist.
            using var zipMemoryStream = new MemoryStream(await File.ReadAllBytesAsync("SageMakerLambda.zip"));
            var createResult = await _lambdaClient.CreateFunctionAsync(
                new CreateFunctionRequest()
                {
                    FunctionName = lambdaFunctionName,
                    Runtime = Runtime.Dotnet6,
                    Description = "SageMaker example function.",
                    Code = new FunctionCode()
                    {
                        ZipFile = zipMemoryStream
                    },
                    Handler = handlerName,
                    Role = roleArn,
                });
            Console.WriteLine(new string('-', 80));
            return createResult.FunctionArn;
        }
    }

    /// <summary>
    /// Create a role to be used by AWS Lambda. Does not create the role if it already exists.
    /// </summary>
    /// <returns>The role Amazon Resource Name (ARN).</returns>
    public static async Task<string> CreateLambdaRole()
    {
        Console.WriteLine(new string('-', 80));
        var roleArn = await GetRoleArnIfExists(lambdaRoleName);
        if (!string.IsNullOrEmpty(roleArn))
        {
            return roleArn;
        }

        Console.WriteLine("Creating a role to for AWS Lambda to use.");

        var assumeRolePolicy = "{" +
                                        "\"Version\": \"2012-10-17\"," +
                                        "\"Statement\": [{" +
                                            "\"Effect\": \"Allow\"," +
                                            "\"Principal\": {" +
                                                $"\"Service\": [" +
                                                    "\"sagemaker.amazonaws.com\"," +
                                                    "\"sagemaker-geospatial.amazonaws.com\"," +
                                                    "\"lambda.amazonaws.com\"," +
                                                    "\"s3.amazonaws.com\"" +
                                                "]" +
                                            "}," +
                                            "\"Action\": \"sts:AssumeRole\"" +
                                        "}]" +
                                    "}";

        var roleResult = await _iamClient!.CreateRoleAsync(
            new CreateRoleRequest()
            {
                AssumeRolePolicyDocument = assumeRolePolicy,
                Path = "/",
                RoleName = lambdaRoleName
            });

        await _iamClient.AttachRolePolicyAsync(
            new AttachRolePolicyRequest()
            {
                PolicyArn = "arn:aws:iam::aws:policy/AmazonSageMakerFullAccess",
                RoleName = lambdaRoleName
            });
        await _iamClient.AttachRolePolicyAsync(
            new AttachRolePolicyRequest()
            {
                PolicyArn = "arn:aws:iam::aws:policy/service-role/AmazonSageMakerGeospatialFullAccess",
                RoleName = lambdaRoleName
            });
        await _iamClient.AttachRolePolicyAsync(
            new AttachRolePolicyRequest()
            {
                PolicyArn = "arn:aws:iam::aws:policy/service-role/AmazonSageMakerServiceCatalogProductsLambdaServiceRolePolicy",
                RoleName = lambdaRoleName
            });
        await _iamClient.AttachRolePolicyAsync(
            new AttachRolePolicyRequest()
            {
                PolicyArn = "arn:aws:iam::aws:policy/service-role/AWSLambdaSQSQueueExecutionRole",
                RoleName = lambdaRoleName
            });


        // Allow time for the role to be ready.
        Thread.Sleep(10000);
        Console.WriteLine(new string('-', 80));

        return roleResult.Role.Arn;
    }


    /// <summary>
    /// Create a role to be used by SageMaker.
    /// </summary>
    /// <returns>The role Amazon Resource Name (ARN).</returns>
    public static async Task<string> CreateSageMakerRole()
    {
        Console.WriteLine(new string('-', 80));
        var roleArn = await GetRoleArnIfExists(sageMakerRoleName);
        if (!string.IsNullOrEmpty(roleArn))
        {
            return roleArn;
        }

        Console.WriteLine("Creating a role to use with SageMaker.");

        var assumeRolePolicy = "{" +
                                        "\"Version\": \"2012-10-17\"," +
                                        "\"Statement\": [{" +
                                            "\"Effect\": \"Allow\"," +
                                            "\"Principal\": {" +
                                                $"\"Service\": [" +
                                                    "\"sagemaker.amazonaws.com\"," +
                                                    "\"sagemaker-geospatial.amazonaws.com\"," +
                                                    "\"lambda.amazonaws.com\"," +
                                                    "\"s3.amazonaws.com\"" +
                                                "]" +
                                            "}," +
                                            "\"Action\": \"sts:AssumeRole\"" +
                                        "}]" +
                                    "}";

        var roleResult = await _iamClient!.CreateRoleAsync(
            new CreateRoleRequest()
            {
                AssumeRolePolicyDocument = assumeRolePolicy,
                Path = "/",
                RoleName = sageMakerRoleName
            });

        await _iamClient.AttachRolePolicyAsync(
            new AttachRolePolicyRequest()
            {
                PolicyArn = "arn:aws:iam::aws:policy/AmazonSageMakerFullAccess",
                RoleName = sageMakerRoleName
            });
        await _iamClient.AttachRolePolicyAsync(
            new AttachRolePolicyRequest()
            {
                PolicyArn = "arn:aws:iam::aws:policy/AmazonSageMakerGeospatialFullAccess",
                RoleName = sageMakerRoleName
            });

        // Allow time for the role to be ready.
        Thread.Sleep(10000);
        Console.WriteLine(new string('-', 80));
        return roleResult.Role.Arn;
    }

    /// <summary>
    /// Set up the SQS queue to use with the pipeline.
    /// </summary>
    /// <returns>The URL for the queue.</returns>
    public static async Task<string> SetupQueue()
    {
        Console.WriteLine(new string('-', 80));
        string queueName = _configuration["queueName"];

        Console.WriteLine($"Setting up queue {queueName}.");

        try
        {
            var queueInfo = await _sqsClient.GetQueueUrlAsync(new GetQueueUrlRequest()
                { QueueName = queueName });
            return queueInfo.QueueUrl;
        }
        catch (QueueDoesNotExistException)
        {
            var attrs = new Dictionary<string, string>
            {
                {
                    QueueAttributeName.DelaySeconds,
                    "5"
                },
                {
                    QueueAttributeName.ReceiveMessageWaitTimeSeconds,
                    "5"
                },
                {
                    QueueAttributeName.VisibilityTimeout,
                    "300"
                },
            };

            var request = new CreateQueueRequest
            {
                Attributes = attrs,
                QueueName = queueName,
            };

            var response = await _sqsClient.CreateQueueAsync(request);

            await ConnectLambda(response.QueueUrl);
            Console.WriteLine(new string('-', 80));
            return response.QueueUrl;
        }
    }

    /// <summary>
    /// Connect the queue to the lambda as an event source.
    /// </summary>
    /// <param name="queueUrl">The URL for the queue.</param>
    /// <returns>Async task.</returns>
    public static async Task ConnectLambda(string queueUrl)
    {
        Console.WriteLine(new string('-', 80));
        Console.WriteLine($"Connecting lambda and queue for the pipeline.");

        var queueAttributes = await _sqsClient.GetQueueAttributesAsync(
            new GetQueueAttributesRequest() { QueueUrl = queueUrl, AttributeNames = new List<string>() { "All" }});
        var queueArn = queueAttributes.QueueARN;
        await _lambdaClient.CreateEventSourceMappingAsync(
            new CreateEventSourceMappingRequest()
            {
                EventSourceArn = queueArn,
                FunctionName = lambdaFunctionName,
                Enabled = true
            });
        Console.WriteLine(new string('-', 80));
    }

    /// <summary>
    /// Create a pipeline from some json.
    /// </summary>
    /// <param name="roleArn">The ARN of the role for the pipeline.</param>
    /// <param name="functionArn">The ARN of the Lambda function for the pipeline.</param>
    /// <returns>The ARN of the pipeline.</returns>
    public static async Task<string> SetupPipeline(string roleArn, string functionArn)
    {
        Console.WriteLine(new string('-', 80));
        Console.WriteLine($"Setting up the pipeline.");

        var pipelineName = _configuration["pipelineName"];
        var pipelineJson = await File.ReadAllTextAsync("GeoSpacialPipeline.json");

        // Add the correct function ARN instead of the placeholder.
        pipelineJson = pipelineJson.Replace("*FUNCTION_ARN*", functionArn);

        var pipelineArn = await _sageMakerWrapper.SetupPipeline(pipelineJson, roleArn, pipelineName,
            "sdk example pipeline", pipelineName);

        Console.WriteLine($"\tPipeline set up with ARN {pipelineArn}.");
        Console.WriteLine(new string('-', 80));
        
        return pipelineArn;
    }

    /// <summary>
    /// Start a pipeline execution with job configurations.
    /// </summary>
    /// <param name="queueUrl">The URL for the queue used in the pipeline.</param>
    /// <returns>The pipeline execution ARN.</returns>
    public static async Task<string> ExecutePipeline(string queueUrl)
    {
        Console.WriteLine(new string('-', 80));
        Console.WriteLine($"Starting pipeline execution.");

        var pipelineName = _configuration["pipelineName"];
        var bucketName = _configuration["bucketName"];
        var input = $"s3://{bucketName}/samplefiles/latlongtest.csv";
        var output = $"s3://{bucketName}/outputfiles/";

        var executionARN =
            await _sageMakerWrapper.ExecutePipeline(queueUrl, input, output,
                pipelineName);

        Console.WriteLine($"\tExecution started with ARN {executionARN}.");
        Console.WriteLine(new string('-', 80));

        return executionARN;
    }

    /// <summary>
    /// Clean up the resources from the scenario.
    /// </summary>
    /// <param name="queueUrl">The URL of the queue to clean up.</param>
    /// <returns>Async task.</returns>
    private static async Task CleanupResources(string queueUrl)
    {
        Console.WriteLine(new string('-', 80));
        Console.WriteLine($"Clean up resources.");

        var pipelineName = _configuration["pipelineName"];
        if (GetYesNoResponse($"\tDelete pipeline {pipelineName}? (y/n)"))
        {
            Console.WriteLine($"\tDeleting pipeline.");
            // Delete the queue.
            await _sageMakerClient.DeletePipelineAsync(
                new DeletePipelineRequest() { PipelineName = pipelineName });
        }

        if (GetYesNoResponse($"\tDelete queue {queueUrl}? (y/n)"))
        {
            Console.WriteLine($"\tDeleting queue.");
            // Delete the queue.
            await _sqsClient.DeleteQueueAsync(new DeleteQueueRequest(queueUrl));
        }

        var bucketName = _configuration["bucketName"];
        if (GetYesNoResponse($"\tDelete Amazon S3 bucket {bucketName}? (y/n)"))
        {
            Console.WriteLine($"\tDeleting bucket.");
            // Delete all objects in the bucket.
            var deleteList = await _s3Client.ListObjectsV2Async(new ListObjectsV2Request()
            {
                BucketName = bucketName
            });
            await _s3Client.DeleteObjectsAsync(new DeleteObjectsRequest()
            {
                BucketName = bucketName,
                Objects = deleteList.S3Objects
                    .Select(o => new KeyVersion { Key = o.Key }).ToList()
            });
            // Now delete the bucket.
            await _s3Client.DeleteBucketAsync(new DeleteBucketRequest()
            {
                BucketName = bucketName
            });
        }

        if (GetYesNoResponse($"\tDelete role {lambdaRoleName}? (y/n)"))
        {
            Console.WriteLine($"\tDetaching policies and deleting role.");

            await _iamClient!.DetachRolePolicyAsync(new DetachRolePolicyRequest()
            {
                RoleName = lambdaRoleName,
                PolicyArn = "arn:aws:iam::aws:policy/AmazonSageMakerFullAccess",
            });

            await _iamClient!.DetachRolePolicyAsync(new DetachRolePolicyRequest()
            {
                RoleName = lambdaRoleName,
                PolicyArn = "arn:aws:iam::aws:policy/AmazonSageMakerGeospatialFullAccess",
            });

            await _iamClient!.DetachRolePolicyAsync(new DetachRolePolicyRequest()
            {
                RoleName = lambdaRoleName,
                PolicyArn = "arn:aws:iam::aws:policy/AWSLambdaSQSQueueExecutionRole",
            });

            await _iamClient!.DeleteRoleAsync(new DeleteRoleRequest()
            {
                RoleName = lambdaRoleName
            });
        }

        if (GetYesNoResponse($"\tDelete role {sageMakerRoleName}? (y/n)"))
        {
            Console.WriteLine($"\tDetaching policies and deleting role.");

            await _iamClient!.DetachRolePolicyAsync(new DetachRolePolicyRequest()
            {
                RoleName = sageMakerRoleName,
                PolicyArn = "arn:aws:iam::aws:policy/AmazonSageMakerFullAccess",
            });

            await _iamClient!.DetachRolePolicyAsync(new DetachRolePolicyRequest()
            {
                RoleName = sageMakerRoleName,
                PolicyArn = "arn:aws:iam::aws:policy/AmazonSageMakerGeospatialFullAccess",
            });

            await _iamClient!.DeleteRoleAsync(new DeleteRoleRequest()
            {
                RoleName = sageMakerRoleName
            });
        }

        Console.WriteLine(new string('-', 80));
    }

    /// <summary>
    /// Helper method to get a role's ARN if it already exists.
    /// </summary>
    /// <param name="roleName">The name of the AWS Identity and Access Management (IAM) Role to look for.</param>
    /// <returns>The role ARN if it exists, otherwise an empty string.</returns>
    private static async Task<string> GetRoleArnIfExists(string roleName)
    {
        Console.WriteLine($"Checking for role named {roleName}.");

        try
        {
            var existingRole = await _iamClient.GetRoleAsync(new GetRoleRequest()
            {
                RoleName = lambdaRoleName
            });
            return existingRole.Role.Arn;
        }
        catch (NoSuchEntityException)
        {
            return string.Empty;
        }
    }

    /// <summary>
    /// Helper method to get a yes or no response from the user.
    /// </summary>
    /// <param name="question">The question string to print on the console.</param>
    /// <returns>True if the user responds with a yes.</returns>
    private static bool GetYesNoResponse(string question)
    {
        Console.WriteLine(question);
        var ynResponse = Console.ReadLine();
        var response = ynResponse != null &&
                       ynResponse.Equals("y",
                           StringComparison.InvariantCultureIgnoreCase);
        return response;
    }
}
