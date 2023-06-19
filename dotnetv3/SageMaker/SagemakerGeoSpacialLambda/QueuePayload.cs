// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
// SPDX-License-Identifier:  Apache-2.0

namespace SageMakerGeoSpacialLambda;

public class QueuePayload
{
    public string Token { get; set; } = null!;
    public Dictionary<string, string> Arguments { get; set; } = null!;
}