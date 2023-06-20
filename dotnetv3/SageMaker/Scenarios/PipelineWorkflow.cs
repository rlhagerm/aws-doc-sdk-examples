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
using Amazon.SageMaker;
using Amazon.SageMaker.Model;
using Amazon.SageMakerGeospatial;
using Amazon.SageMakerGeospatial.Model;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging.Debug;
using SageMakerActions;
using Filter = Amazon.EC2.Model.Filter;
using Host = Microsoft.Extensions.Hosting.Host;
using ResourceNotFoundException = Amazon.Lambda.Model.ResourceNotFoundException;


namespace SageMakerScenario;

public class PipelineWorkflow
{
    private static ILogger logger = null!;
    private static IAmazonIdentityManagementService _iamClient;
    private static SageMakerWrapper _sageMakerWrapper;
    private static IAmazonEC2 _ec2Client;
    private static IAmazonSQS _sqsClient;
    private static IAmazonLambda _lambdaClient;
    private static IAmazonSageMaker _sageMakerClient;

    // TODO replace this with uploading the function directly
    private static string functionArn = "arn:aws:lambda:us-west-2:565846806325:function:SageMakerVectorLambda";
    private static string functionName = "SageMakerVectorLambda";

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
                    .AddAWSService<IAmazonLambda>(new AWSOptions() { Region = RegionEndpoint.USWest2 })
                    .AddTransient<SageMakerWrapper>()
            )
            .Build();

        logger = LoggerFactory.Create(builder => { builder.AddConsole(); })
            .CreateLogger<PipelineWorkflow>();

        ServicesSetup(host);
       var queueUrl = await SetupQueue();
        await SetupPipeline();
        await ExecutePipeline(queueUrl);

    }

    /// <summary>
    /// Populate the services for use within the console application.
    /// </summary>
    /// <param name="host">The services host.</param>
    private static void ServicesSetup(IHost host)
    {
        _sageMakerWrapper = host.Services.GetRequiredService<SageMakerWrapper>();
        _iamClient = host.Services.GetRequiredService<IAmazonIdentityManagementService>();
        _ec2Client = host.Services.GetRequiredService<IAmazonEC2>();
        _sqsClient = host.Services.GetRequiredService<IAmazonSQS>();
        _lambdaClient = host.Services.GetRequiredService<IAmazonLambda>();
        _sageMakerClient = host.Services.GetRequiredService<IAmazonSageMaker>();
    }

    public async Task SetupDomain()
    {
        var roleArn = CreateRole();

        // TODO: create the domain if one does not already exist.
        // Get the default Amazon VPC of your account
        var defaultVpc = await _ec2Client.DescribeVpcsAsync(new DescribeVpcsRequest()
        {
            Filters = new List<Filter>()
            {
                new Filter()
                    { Name = "isDefault", Values = new List<string>() { "true" } }
            },
        });

        var defaultVpcId = defaultVpc.Vpcs.First().VpcId;

    }

    // snippet-start:[SageMaker.dotnetv3.CreateRole]
    /// <summary>
    /// Create a role to be used by SageMaker.
    /// </summary>
    /// <returns>The role Amazon Resource Name (ARN).</returns>
    public async Task<string> CreateRole()
    {
        // TODO: only create these resources if they do not already exist.
        Console.WriteLine(new string('-', 80));
        Console.WriteLine("Creating a role to use with SageMaker and attaching managed policy AmazonSageMakerFullAccess.");
        Console.WriteLine(new string('-', 80));

        //var roleName = "_configuration[\"roleName\"]";
        var roleName = "sagemakerroletest";

        var assumeRolePolicy = "{" +
                               "\"Version\": \"2012-10-17\"," +
                               "\"Statement\": [{" +
                               "\"Effect\": \"Allow\"," +
                               "\"Principal\": {" +
                               $"\"Service\": \"[" +
                                    "\"sagemaker.amazonaws.com\"" +
                                    "\"sagemaker-geospatial.amazonaws.com\"" +
                                    "\"lambda.amazonaws.com\"" +
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
                RoleName = roleName
            });

        await _iamClient.AttachRolePolicyAsync(
            new AttachRolePolicyRequest()
            {
                PolicyArn = "arn:aws:iam::aws:policy/AmazonSageMakerFullAccess",
                RoleName = roleName
            });
        await _iamClient.AttachRolePolicyAsync(
            new AttachRolePolicyRequest()
            {
                PolicyArn = "arn:aws:iam::aws:policy/AmazonSageMakerGeospatialFullAccess",
                RoleName = roleName
            });
        // todo: add inline policy with other permissions?

        // Allow time for the role to be ready.
        Thread.Sleep(10000);
        return roleResult.Role.Arn;
    }
    // snippet-end:[SageMaker.dotnetv3.CreateRole]


    public static async Task<string> SetupQueue()
    {
        string queueName = "SageMaker-Example-Queue";

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
            return response.QueueUrl;
        }
    }

    public static async Task ConnectLambda(string queueUrl)
    {
        var queueAttributes = await _sqsClient.GetQueueAttributesAsync(
            new GetQueueAttributesRequest() { QueueUrl = queueUrl, AttributeNames = new List<string>() { "All" }});
        var queueArn = queueAttributes.QueueARN;
        await _lambdaClient.CreateEventSourceMappingAsync(
            new CreateEventSourceMappingRequest()
            {
                EventSourceArn = queueArn,
                FunctionName = functionName,
                Enabled = true
            });
    }

    /// <summary>
    /// Create a pipeline from some json.
    /// </summary>
    /// <returns>The ARN of the pipeline.</returns>
    public static async Task<string> SetupPipeline()
    {
        try
        {
            var pipeline = await _sageMakerClient.DescribePipelineAsync(
                new DescribePipelineRequest() { PipelineName = "SdkPipeline" });
            return pipeline.PipelineArn;
        }
        catch (Amazon.SageMaker.Model.ResourceNotFoundException)
        {
            var pipelineJson = await File.ReadAllTextAsync("GeoSpacialPipeline.json");

            var createResponse = await _sageMakerClient.CreatePipelineAsync(
                new CreatePipelineRequest()
                {
                    PipelineDefinition = pipelineJson,
                    PipelineDescription = "test pipeline from sdk",
                    PipelineDisplayName = "SdkPipeline",
                    PipelineName = "SdkPipeline",
                    RoleArn =
                        "arn:aws:iam::565846806325:role/service-role/AmazonSageMakerServiceCatalogProductsUseRole"

                });

            return createResponse.PipelineArn;
        }
    }

    public async static Task<string> ExecutePipeline(string queueUrl)
    {
        var inputConfig = new VectorEnrichmentJobInputConfig()
        {
            DataSourceConfig = new VectorEnrichmentJobDataSourceConfigInput()
            {
                S3Data = new VectorEnrichmentJobS3Data()
                {
                    S3Uri =
                        "s3://sagemaker-us-west-2-565846806325/samplefiles/latlongtest.csv",
                }
            },
            DocumentType = VectorEnrichmentJobDocumentType.CSV
        };
        var exportConfig = new ExportVectorEnrichmentJobOutputConfig()
        {
            S3Data = new VectorEnrichmentJobS3Data()
            {
                S3Uri = "s3://sagemaker-us-west-2-565846806325/outputfiles/"
            }
        };
        var jobconfig = new VectorEnrichmentJobConfig()
        {
            ReverseGeocodingConfig = new ReverseGeocodingConfig()
            {
                XAttributeName = "Longitude",
                YAttributeName = "Latitude"
            }
        };

        var startExecutionResponse = await _sageMakerClient.StartPipelineExecutionAsync(
            new StartPipelineExecutionRequest()
            {
                PipelineName = "SdkPipeline",
                PipelineExecutionDisplayName = "test-execution2",
                PipelineExecutionDescription = "test execution description",
                PipelineParameters = new List<Parameter>()
                {
                    new Parameter() { Name = "parameter_queue_url", Value = queueUrl },
                    new Parameter()
                        { Name = "parameter_vej_input_config", Value = JsonSerializer.Serialize(inputConfig) },
                    new Parameter()
                        { Name = "parameter_vej_export_config", Value = JsonSerializer.Serialize(exportConfig)},
                    new Parameter()
                    { Name = "parameter_step_1_vej_config", Value = JsonSerializer.Serialize(jobconfig) }
                }
            });
        Console.WriteLine($"Started execution: {startExecutionResponse.PipelineExecutionArn}.");
        return startExecutionResponse.PipelineExecutionArn;
    }
}
