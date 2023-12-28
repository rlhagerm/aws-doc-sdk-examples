﻿// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved. 
// SPDX-License-Identifier:  Apache-2.0

using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;

namespace RecommendationService;

// snippet-start:[ResilientService.dotnetv3.RecommendationService]
/// <summary>
/// Encapsulates a DynamoDB table to use as a service that recommends books, movies, and songs.
/// </summary>
public class Recommendations
{
    private readonly IAmazonDynamoDB _amazonDynamoDb;
    private readonly string _tableName;

    public string TableName => _tableName;

    /// <summary>
    /// Constructor for the Recommendations service.
    /// </summary>
    /// <param name="amazonDynamoDb">The injected DynamoDb client.</param>
    /// <param name="tableName">The name of the DynamoDB table used for the recommendation service.</param>
    public Recommendations(IAmazonDynamoDB amazonDynamoDb, string tableName)
    {
        _amazonDynamoDb = amazonDynamoDb;
        _tableName = tableName;
    }

    /// <summary>
    /// Create the DynamoDb table with a specified name.
    /// </summary>
    /// <param name="tableName">The name for the table.</param>
    /// <returns>True when ready.</returns>
    public async Task<bool> CreateDatabaseWithName(string? tableName)
    {
        try
        {
            await _amazonDynamoDb.CreateTableAsync(
            new CreateTableRequest()
            {
                TableName = tableName,
                AttributeDefinitions = new List<AttributeDefinition>()
                {
                    new()
                    {
                        AttributeName = "MediaType",
                        AttributeType = ScalarAttributeType.S
                    },
                    new()
                    {
                        AttributeName = "ItemId",
                        AttributeType = ScalarAttributeType.N
                    }
                },
                KeySchema = new List<KeySchemaElement>()
                {
                    new()
                    {
                        AttributeName = "MediaType",
                        KeyType = KeyType.HASH
                    },
                    new()
                    {
                        AttributeName = "ItemId",
                        KeyType = KeyType.RANGE
                    }
                },
                ProvisionedThroughput =
                {
                    ReadCapacityUnits = 5,
                    WriteCapacityUnits = 5
                }
            });

            // Wait until the table is ACTIVE and then report success.
            Console.Write("Waiting for table to become active...");

            var request = new DescribeTableRequest
            {
                TableName = tableName
            };

            TableStatus status;
            do
            {
                Thread.Sleep(2000);

                var describeTableResponse = await _amazonDynamoDb.DescribeTableAsync(request);
                status = describeTableResponse.Table.TableStatus;

                Console.Write(".");
            }
            while (status != "ACTIVE");

            return status == TableStatus.ACTIVE;
        }
        catch (ResourceInUseException)
        {
            Console.WriteLine($"Table {tableName} already exists.");
            return false;
        }
    }

    /// <summary>
    /// Populate the database table with data from a specified path.
    /// </summary>
    /// <param name="databaseTableName">The name fo the table.</param>
    /// <param name="recommendationsPath">The path of the recommendations data.</param>
    /// <returns>Async task.</returns>
    public async Task PopulateDatabase(string databaseTableName, string recommendationsPath)
    {
        var recommendationsText = await File.ReadAllTextAsync(recommendationsPath);
        var records = Document.FromJson(recommendationsText);
        var table = Table.LoadTable(_amazonDynamoDb, databaseTableName);

        foreach (var record in records.AsArrayOfDynamoDBEntry())
        {
            await table.PutItemAsync(record.AsDocument());
        }
    }

    /// <summary>
    /// Delete the recommendation table by name.
    /// </summary>
    /// <param name="tableName">The name of the recommendation table.</param>
    /// <returns>Async task.</returns>
    public async Task DestroyDatabaseByName(string tableName)
    {
        try
        {
            await _amazonDynamoDb.DeleteTableAsync(
                new DeleteTableRequest() { TableName = tableName });
            Console.WriteLine($"Table {tableName} was deleted.");
        }
        catch (ResourceNotFoundException)
        {
            Console.WriteLine($"Table {tableName} not found");
        }
    }
}
// snippet-end:[ResilientService.dotnetv3.RecommendationService]