// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
// SPDX-License-Identifier:  Apache-2.0

namespace SageMakerGeoSpacialLambda;

public class QueuePayload
{
    public string token { get; set; } = null!;
    public string pipelineExecutionArn { get; set; }
    public string status { get; set; }
    public Dictionary<string, string> arguments { get; set; } = null!;
}