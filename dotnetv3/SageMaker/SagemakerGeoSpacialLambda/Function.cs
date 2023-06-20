using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Amazon.SageMaker;
using Amazon.SageMaker.Model;
using Amazon.SageMakerGeospatial;
using Amazon.SageMakerGeospatial.Model;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace SageMakerGeoSpacialLambda;

public class Function
{
    /// <summary>
    /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
    /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
    /// region the Lambda function is executed in.
    /// </summary>
    public Function()
    {

    }

    ///// <summary>
    ///// This method is called for every Lambda invocation. This method takes in an SQS event object and can be used 
    ///// to respond to SQS messages.
    ///// </summary>
    ///// <param name="evnt"></param>
    ///// <param name="context"></param>
    ///// <returns></returns>
    //public async Task FunctionHandler(SQSEvent evnt, ILambdaContext context)
    //{
    //    // Get a client
    //    context.Logger.LogInformation("queue handler");
    //    var geoSpacialClient = new AmazonSageMakerGeospatialClient();
    //    var sageMakerClient = new AmazonSageMakerClient();
    //    context.Logger.LogInformation(JsonSerializer.Serialize(evnt));
    //    context.Logger.LogInformation(JsonSerializer.Serialize(context));
    //    foreach (var message in evnt.Records)
    //    {
    //        await ProcessMessageAsync(message, context, geoSpacialClient, sageMakerClient);
    //    }
    //}

    /// <summary>
    /// This method is called for every Lambda invocation. This method takes in an SQS event object and can be used 
    /// to respond to SQS messages.
    /// </summary>
    /// <param name="evnt"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task<Dictionary<string, string>> FunctionHandler(PipelineRequest request, ILambdaContext context)
    {
        // Get a client
        var geoSpacialClient = new AmazonSageMakerGeospatialClient();
        var sageMakerClient = new AmazonSageMakerClient();
        context.Logger.LogInformation("start handler");
        context.Logger.LogInformation(JsonSerializer.Serialize(request));
        context.Logger.LogInformation(JsonSerializer.Serialize(context));
        if (request.Records != null && request.Records.Any())
        {
            context.Logger.LogInformation("Records found, this is a queue event");
            foreach (var message in request.Records)
            {
                await ProcessMessageAsync(message, context, geoSpacialClient, sageMakerClient);
            }
        }
        else if (!string.IsNullOrEmpty(request.vej_export_config))
        {
            context.Logger.LogInformation("export found, this is an export");

            var outputConfig =
                JsonSerializer.Deserialize<ExportVectorEnrichmentJobOutputConfig>(
                    request.vej_export_config);

            var exportResponse = await geoSpacialClient.ExportVectorEnrichmentJobAsync(
                new ExportVectorEnrichmentJobRequest()
                {
                    Arn = request.vej_arn,
                    ExecutionRoleArn = request.Role,
                    OutputConfig = outputConfig
                });
            context.Logger.LogInformation($"export response: {JsonSerializer.Serialize(exportResponse)}");
            var response3 = new Dictionary<string, string>
            {
                { "export_eoj_status", exportResponse.ExportStatus.ToString() },
                { "vej_arn", exportResponse.Arn }
            };
            return response3;
        }
        else if (!string.IsNullOrEmpty(request.vej_name))
        {
            context.Logger.LogInformation("starting a job");
            var inputconfig =
                JsonSerializer.Deserialize<VectorEnrichmentJobInputConfig>(
                    request.vej_input_config);

            var jobconfig =
                JsonSerializer.Deserialize<VectorEnrichmentJobConfig>(
                    request.vej_config);

            var jobResponse = await geoSpacialClient.StartVectorEnrichmentJobAsync(
                new StartVectorEnrichmentJobRequest()
                {
                    ExecutionRoleArn = request.Role,
                    InputConfig = inputconfig,
                    Name = request.vej_name,
                    JobConfig = jobconfig

                });
            context.Logger.LogInformation(JsonSerializer.Serialize(jobResponse));
            var response = new Dictionary<string, string>
            {
                { "vej_arn", jobResponse.Arn },
                { "statusCode", jobResponse.HttpStatusCode.ToString() }
            };
            return response;
        }

        
        var response2 = new Dictionary<string, string>
        {
            //{ "vej_arn", jobResponse.Arn },
            //{ "statusCode", jobResponse.HttpStatusCode.ToString() }
        };
        return response2;
    }

    private async Task ProcessMessageAsync(SQSEvent.SQSMessage message, ILambdaContext context, 
        AmazonSageMakerGeospatialClient geoClient, AmazonSageMakerClient sageMakerClient)
    {
        context.Logger.LogInformation($"Processed message {message.Body}");
        
        // Get information about the SageMaker job.
        var payload = JsonSerializer.Deserialize<QueuePayload>(message.Body);
        context.Logger.LogInformation($"payload token {payload.token}");
        var token = payload.token;

        if (payload.arguments.ContainsKey("vej_arn"))
        {
            var job_arn = payload.arguments["vej_arn"];
            context.Logger.LogInformation($"token: {token}, arn {job_arn}");

            var jobInfo = geoClient.GetVectorEnrichmentJobAsync(
                new GetVectorEnrichmentJobRequest()
                {
                    Arn = job_arn
                });
            context.Logger.LogInformation(JsonSerializer.Serialize(jobInfo));
            if (jobInfo.Result.Status == VectorEnrichmentJobStatus.COMPLETED)
            {
                context.Logger.LogInformation($"Status Completed, resuming pipeline...");
                await sageMakerClient.SendPipelineExecutionStepSuccessAsync(
                    new SendPipelineExecutionStepSuccessRequest()
                    {
                        CallbackToken = token,
                        OutputParameters = new List<OutputParameter>()
                        {
                            new OutputParameter()
                                { Name = "export_status", Value = jobInfo.Result.Status }
                        }
                    });
            }
            else if (jobInfo.Result.Status == VectorEnrichmentJobStatus.FAILED)
            {
                context.Logger.LogInformation($"Status failed, stopping pipeline...");
                await sageMakerClient.SendPipelineExecutionStepFailureAsync(
                    new SendPipelineExecutionStepFailureRequest()
                    {
                        CallbackToken = token,
                        FailureReason = jobInfo.Result.ErrorDetails.ErrorMessage
                    });
            }
            else if (jobInfo.Result.Status == VectorEnrichmentJobStatus.IN_PROGRESS)
            {
                context.Logger.LogInformation(
                    $"Status still in progress, check back later.");
                throw new("Job still running.");
            }
        }

    }
}