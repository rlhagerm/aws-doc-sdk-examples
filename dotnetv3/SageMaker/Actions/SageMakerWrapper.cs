// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
// SPDX-License-Identifier:  Apache-2.0

using System.Net;
using System.Text.Json;
using Amazon.SageMaker;
using Amazon.SageMaker.Model;
using Amazon.SageMakerGeospatial.Model;
using Amazon.SageMakerGeospatial;

namespace SageMakerActions;

/// <summary>
/// Wrapper class for SageMaker actions and logic.
/// </summary>
public class SageMakerWrapper
{
    private readonly IAmazonSageMaker _amazonSageMaker;
    public SageMakerWrapper(IAmazonSageMaker amazonSageMaker)
    {
        _amazonSageMaker = amazonSageMaker;
    }

    /// <summary>
    /// Create a new domain and return the domain ARN.
    /// </summary>
    /// <param name="roleArn">The ARN for the execution role.</param>
    /// <param name="vpcId">The VPC for the domain.</param>
    /// <param name="domainName">The name for the new domain.</param>
    /// <param name="subnetIds">The collection of subnets for the domain.</param>
    /// <returns>The ARN of the new domain.</returns>
    public async Task<string> SetupDomain(string roleArn, string vpcId, string domainName, List<string> subnetIds)
    {
       var createResponse = await _amazonSageMaker.CreateDomainAsync(new CreateDomainRequest()
        {
            DomainName = domainName,
            AuthMode = AuthMode.IAM,
            DefaultUserSettings = new UserSettings() { ExecutionRole = roleArn },
            SubnetIds = subnetIds,
            VpcId = vpcId
        });

       return createResponse.DomainArn;
    }

    /// <summary>
    /// Create a pipeline from a JSON definition, or updates it if the pipeline already exists.
    /// </summary>
    /// <returns>The ARN of the pipeline.</returns>
    public async Task<string> SetupPipeline(string pipelineJson, string roleArn, string name, string description, string displayName)
    {
        try
        {
            var updateResponse = await _amazonSageMaker.UpdatePipelineAsync(
                new UpdatePipelineRequest()
                {
                    PipelineDefinition = pipelineJson,
                    PipelineDescription = description,
                    PipelineDisplayName = displayName,
                    PipelineName = name,
                    RoleArn = roleArn
                });
            return updateResponse.PipelineArn;
        }
        catch (Amazon.SageMaker.Model.ResourceNotFoundException)
        {
            var createResponse = await _amazonSageMaker.CreatePipelineAsync(
                new CreatePipelineRequest()
                {
                    PipelineDefinition = pipelineJson,
                    PipelineDescription = description,
                    PipelineDisplayName = displayName,
                    PipelineName = name,
                    RoleArn = roleArn

                });

            return createResponse.PipelineArn;
        }
    }

    /// <summary>
    /// Execute a pipeline with input and output file locations.
    /// </summary>
    /// <param name="queueUrl">The URL for the queue to use for pipeline callbacks.</param>
    /// <param name="inputLocationUrl">The input location in Amazon S3.</param>
    /// <param name="outputLocationUrl">The output location in Amazon S3.</param>
    /// <param name="pipelineName">The name of the pipeline.</param>
    /// <returns>The ARN of the pipeline execution.</returns>
    public async Task<string> ExecutePipeline(string queueUrl, string inputLocationUrl, string outputLocationUrl, string pipelineName)
    {
        var inputConfig = new VectorEnrichmentJobInputConfig()
        {
            DataSourceConfig = new VectorEnrichmentJobDataSourceConfigInput()
            {
                S3Data = new VectorEnrichmentJobS3Data()
                {
                    S3Uri = inputLocationUrl
                }
            },
            DocumentType = VectorEnrichmentJobDocumentType.CSV
        };

        var exportConfig = new ExportVectorEnrichmentJobOutputConfig()
        {
            S3Data = new VectorEnrichmentJobS3Data()
            {
                S3Uri = outputLocationUrl
            }
        };

        var jobConfig = new VectorEnrichmentJobConfig()
        {
            ReverseGeocodingConfig = new ReverseGeocodingConfig()
            {
                XAttributeName = "Longitude",
                YAttributeName = "Latitude"
            }
        };

        var startExecutionResponse = await _amazonSageMaker.StartPipelineExecutionAsync(
            new StartPipelineExecutionRequest()
            {
                PipelineName = pipelineName,
                PipelineExecutionDisplayName = pipelineName + "-example-execution",
                PipelineParameters = new List<Parameter>()
                {
                    new Parameter() { Name = "parameter_queue_url", Value = queueUrl },
                    new Parameter() { Name = "parameter_vej_input_config", Value = JsonSerializer.Serialize(inputConfig) },
                    new Parameter() { Name = "parameter_vej_export_config", Value = JsonSerializer.Serialize(exportConfig) },
                    new Parameter() { Name = "parameter_step_1_vej_config", Value = JsonSerializer.Serialize(jobConfig) }
                }
            });
        return startExecutionResponse.PipelineExecutionArn;
    }

    /// <summary>
    /// Delete a SageMaker pipeline by name.
    /// </summary>
    /// <param name="pipelineName">The name of the pipeline to delete.</param>
    /// <returns>The ARN of the pipeline.</returns>
    public async Task<string> DeletePipelineByName(string pipelineName)
    {
        var deleteResponse = await _amazonSageMaker.DeletePipelineAsync(
            new DeletePipelineRequest()
            {
                PipelineName = pipelineName
            });

        return deleteResponse.PipelineArn;
    }

    /// <summary>
    /// Delete a SageMaker domain by ID.
    /// </summary>
    /// <param name="domainId">The ID of the domain to delete.</param>
    /// <returns>True if successful.</returns>
    public async Task<bool> DeleteDomainById(string domainId)
    {
        var deleteResponse = await _amazonSageMaker.DeleteDomainAsync(
            new DeleteDomainRequest()
            {
                DomainId = domainId
            });

        return deleteResponse.HttpStatusCode == HttpStatusCode.OK;
    }
}