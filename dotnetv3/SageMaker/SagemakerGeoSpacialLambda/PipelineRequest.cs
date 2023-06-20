using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.Lambda.SQSEvents;
using Amazon.SageMaker.Model;
using Amazon.SageMakerGeospatial.Model;

namespace SageMakerGeoSpacialLambda;

public class PipelineRequest : SQSEvent
{
    public string Role { get; set; }
    public string Region { get; set; }
    public string vej_name { get; set; }
    public string vej_arn{ get; set; }

    public string vej_input_config { get; set; }

    public string vej_config { get; set; }

    public string vej_export_config { get; set; }

    public string queue_url { get; set; }

    public string ExecutionRoleArn { get; set; }
}