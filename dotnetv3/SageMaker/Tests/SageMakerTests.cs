// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
// SPDX-License-Identifier:  Apache-2.0

using Amazon.SageMaker;
using Microsoft.Extensions.Configuration;
using SageMakerActions;
using SageMakerScenario;

namespace SageMakerTests;

/// <summary>
/// Tests for the SageMakerWrapper class.
/// </summary>
public class SageMakerTests
{
    private readonly IConfiguration _configuration;
    private readonly SageMakerWrapper _sageMakerWrapper;

    /// <summary>
    /// Constructor for the test class.
    /// </summary>
    public SageMakerTests()
    {
        _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("testsettings.json") // Load test settings from .json file.
            .AddJsonFile("testsettings.local.json",
                true) // Optionally load local settings.
            .Build();

        _sageMakerWrapper = new SageMakerWrapper(
            new AmazonSageMakerClient());
    }

    /// <summary>
    /// Set up a new pipeline. The returned ARN should not be empty.
    /// </summary>
    /// <returns>Async task.</returns>
    [Fact]
    [Order(1)]
    [Trait("Category", "Integration")]
    public async Task AddPipeline_ShouldReturnNonEmptyArn()
    {
        // Arrange.
        //var lambdaRoleArn = await PipelineWorkflow.CreateLambdaRole();
        var sageMakerRoleArn = await PipelineWorkflow.CreateSageMakerRole();
        //var functionArn = await PipelineWorkflow.SetupLambda(lambdaRoleArn);
        //var queueUrl = await PipelineWorkflow.SetupQueue();
        await PipelineWorkflow.SetupBucket();
        var pipelineJson =
            "{\"Version\":\"2020-12-01\",\"Metadata\":{},\"Parameters\":[]," +
            "\"PipelineExperimentConfig\":{\"ExperimentName\":" +
            "{\"Get\":\"Execution.PipelineName\"},\"TrialName\":" +
            "{\"Get\":\"Execution.PipelineExecutionId\"}},\"Steps\":[]}";

        // Act.
        var arn = await _sageMakerWrapper.SetupPipeline(
            pipelineJson, 
            sageMakerRoleArn,
            "testPipeline",
            "testPipeline",
            "test pipeline");

        // Assert.
        Assert.False(string.IsNullOrEmpty(arn));
    }

    /// <summary>
    /// Set up a new pipeline. The returned ARN should not be empty.
    /// </summary>
    /// <returns>Async task.</returns>
    [Fact]
    [Order(1)]
    [Trait("Category", "Integration")]
    public async Task ExecutePipeline_ShouldReturnNonEmptyArn()
    {
        // Arrange.
        //var lambdaRoleArn = await PipelineWorkflow.CreateLambdaRole();
        var sageMakerRoleArn = await PipelineWorkflow.CreateSageMakerRole();
        //var functionArn = await PipelineWorkflow.SetupLambda(lambdaRoleArn);
        //var queueUrl = await PipelineWorkflow.SetupQueue();
        await PipelineWorkflow.SetupBucket();
        var pipelineJson =
            "{\"Version\":\"2020-12-01\",\"Metadata\":{},\"Parameters\":[]," +
            "\"PipelineExperimentConfig\":{\"ExperimentName\":" +
            "{\"Get\":\"Execution.PipelineName\"},\"TrialName\":" +
            "{\"Get\":\"Execution.PipelineExecutionId\"}},\"Steps\":[]}";

        // Act.
        var arn = await _sageMakerWrapper.SetupPipeline(
            pipelineJson,
            sageMakerRoleArn,
            "testPipeline",
            "testPipeline",
            "test pipeline");

        // Assert.
        Assert.False(string.IsNullOrEmpty(arn));
    }

    /// <summary>
    /// Set up a new pipeline. The returned ARN should not be empty.
    /// </summary>
    /// <returns>Async task.</returns>
    [Fact]
    [Order(1)]
    [Trait("Category", "Integration")]
    public async Task CheckPipelineExecution_ShouldReturnStatus()
    {
        // Arrange.
        //var lambdaRoleArn = await PipelineWorkflow.CreateLambdaRole();
        var sageMakerRoleArn = await PipelineWorkflow.CreateSageMakerRole();
        //var functionArn = await PipelineWorkflow.SetupLambda(lambdaRoleArn);
        //var queueUrl = await PipelineWorkflow.SetupQueue();
        await PipelineWorkflow.SetupBucket();
        var pipelineJson =
            "{\"Version\":\"2020-12-01\",\"Metadata\":{},\"Parameters\":[]," +
            "\"PipelineExperimentConfig\":{\"ExperimentName\":" +
            "{\"Get\":\"Execution.PipelineName\"},\"TrialName\":" +
            "{\"Get\":\"Execution.PipelineExecutionId\"}},\"Steps\":[]}";

        // Act.
        var arn = await _sageMakerWrapper.SetupPipeline(
            pipelineJson,
            sageMakerRoleArn,
            "testPipeline",
            "testPipeline",
            "test pipeline");

        // Assert.
        Assert.False(string.IsNullOrEmpty(arn));
    }

    /// <summary>
    /// Set up a new pipeline. The returned ARN should not be empty.
    /// </summary>
    /// <returns>Async task.</returns>
    [Fact]
    [Order(3)]
    [Trait("Category", "Integration")]
    public async Task DeletePipeline_ShouldReturnArn()
    {
        // Arrange.

        // Act.
        var arn = await _sageMakerWrapper.DeletePipelineByName("testPipeline");
        await PipelineWorkflow.CleanupResources(false)

        // Assert.
        Assert.False(string.IsNullOrEmpty(arn));
    }
}