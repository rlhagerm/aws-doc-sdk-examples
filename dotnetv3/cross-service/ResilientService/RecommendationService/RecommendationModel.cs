// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved. 
// SPDX-License-Identifier:  Apache-2.0

namespace RecommendationService;

/// <summary>
/// Data model for the recommendation service.
/// </summary>
public class RecommendationModel
{
    public string Creator { get; set; }
    public int ItemId { get; set; }
    public string MediaType { get; set; }
    public string Title { get; set; }
}