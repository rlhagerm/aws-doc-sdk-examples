// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
// SPDX-License-Identifier:  Apache-2.0

using Amazon.EC2;
using Amazon.EC2.Model;
using Amazon.IdentityManagement;
using Amazon.IdentityManagement.Model;
using Amazon.Lambda;
using Amazon.Lambda.Model;
using Amazon.SageMaker;
using Amazon.SageMaker.Model;
using Amazon.SageMakerGeospatial;
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


namespace SageMakerScenario;

public class PipelineWorkflow
{
    private static ILogger logger = null!;
    private IAmazonIdentityManagementService _iamClient;
    private SageMakerWrapper _sageMakerWrapper;
    private IAmazonEC2 _ec2Client;
    private IAmazonSQS _sqsClient;
    private IAmazonLambda _lambdaClient;
    private IAmazonSageMaker _sageMakerClient;

    // TODO replace this with uploading the function directly
    private string functionArn = "arn:aws:lambda:us-west-2:565846806325:function:SageMakerVectorLambda";
    private string functionName = "SageMakerVectorLambda";

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
                    .AddAWSService<IAmazonEC2>()
                    .AddAWSService<IAmazonSageMaker>()
                    .AddAWSService<IAmazonSageMakerGeospatial>()
                    .AddAWSService<IAmazonSQS>()
                    .AddAWSService<IAmazonLambda>()
                    .AddTransient<SageMakerWrapper>()
            )
            .Build();

        logger = LoggerFactory.Create(builder => { builder.AddConsole(); })
            .CreateLogger<PipelineWorkflow>();

    }

    /// <summary>
    /// Populate the services for use within the console application.
    /// </summary>
    /// <param name="host">The services host.</param>
    private void ServicesSetup(IHost host)
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


    public async Task<string> SetupQueue()
    {
        string queueName = "Sagemaker-Example-Queue";

        // TODO: check if the queue already exists, only create it if it does not exist

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
        var queueAttributes = await _sqsClient.GetQueueAttributesAsync(
            new GetQueueAttributesRequest() { QueueUrl = response.QueueUrl });
        var queueArn = queueAttributes.QueueARN;
        await _lambdaClient.CreateEventSourceMappingAsync(
            new CreateEventSourceMappingRequest()
            {
                EventSourceArn = queueArn,
                FunctionName = functionName,
                Enabled = true
            });
        return response.QueueUrl;
    }

    public async Task<string> SetupPipeline()
    {
        var pipelineJson = await _sageMakerClient.DescribePipelineAsync(
            new DescribePipelineRequest()
            {
                PipelineName = "GeospatialEarthObservationPipeline"
            });

        await _sageMakerClient.CreatePipelineAsync(new CreatePipelineRequest()
        {
            PipelineDefinition = pipelineJson.PipelineDefinition
        })
    }
}
