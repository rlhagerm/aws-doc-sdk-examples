// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved. 
// SPDX-License-Identifier:  Apache-2.0

namespace RecommendationService;

/// <summary>
/// Encapsulates a DynamoDB table to use as a service that recommends books, movies, and songs.
/// </summary>
public class Recommendations
{
    public void CreateDatabase(string? databaseTableName)
    {
        throw new NotImplementedException();
    }

    public void PopulateDatabase(string? databaseTableName, string recommendationsPath)
    {
        throw new NotImplementedException();
    }

    public void Destroy()
    {
        throw new NotImplementedException();
    }
}