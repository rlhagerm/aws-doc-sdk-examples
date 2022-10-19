// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
// SPDX-License-Identifier:  Apache-2.0

using System.Net;
using System.Text.Json;
using Amazon.RDSDataService;
using Amazon.RDSDataService.Model;

namespace AuroraItemTracker;

/// <summary>
/// Class for working with WorkItems using the Amazon RDS data service.
/// </summary>
public class WorkItemService
{
    private readonly IAmazonRDSDataService _amazonRDSDataService;
    private readonly IConfiguration _configuration;
    private string _databaseName;
    private string _tableName;

    /// <summary>
    /// Constructor that uses the injected Amazon RDS Data Service client.
    /// </summary>
    /// <param name="amazonTranscribeService">Amazon RDS Data Service.</param>
    /// <param name="configuration">App configuration.</param>
    public WorkItemService(IAmazonRDSDataService amazonRDSDataService, IConfiguration configuration)
    {
        _amazonRDSDataService = amazonRDSDataService;
        _configuration = configuration;
        _databaseName = configuration["Database"];
        _tableName = configuration["WorkItemTable"];
    }

    /// <summary>
    /// Get the items with a particular archive state.
    /// </summary>
    /// <param name="archiveState">The archive state of the items to get.</param>
    /// <returns>A collection of WorkItems.</returns>
    public async Task<IList<WorkItem>> GetItemsByArchiveState(ArchiveState archiveState)
    {
        // Set up the parameters.
        var parameters = new List<SqlParameter>
        {
            new()
            {
                Name = "isArchived",
                Value = new Field { LongValue = (long)archiveState }
            }
        };

        var statementResult = await _amazonRDSDataService.ExecuteStatementAsync(new ExecuteStatementRequest
        {
            Database = _databaseName,
            FormatRecordsAs = "json",
            Sql = $"SELECT * FROM {_tableName} WHERE archive = :isArchived",
            Parameters = parameters,
            SecretArn = _configuration["RDSSecretArn"],
            ResourceArn = _configuration["RDSResourceArn"]
        });
        
        var results = JsonSerializer.Deserialize<WorkItem[]>(
            statementResult.FormattedRecords,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        return results!;
    }

    /// <summary>
    /// Get an item by its ID.
    /// </summary>
    /// <param name="itemId">The ID of the item to get.</param>
    /// <returns>A WorkItem instance.</returns>
    public async Task<WorkItem> GetItem(string itemId)
    {
        // Set up the parameters.
        var parameters = new List<SqlParameter>
        {
            new()
            {
                Name = "itemId",
                Value = new Field { StringValue = itemId }
            }
        };

        var statementResult = await _amazonRDSDataService.ExecuteStatementAsync(new ExecuteStatementRequest
        {
            Database = _databaseName,
            FormatRecordsAs = "json",
            Sql = $"SELECT * FROM {_tableName} WHERE itemId = :itemId;",
            Parameters = parameters,
            SecretArn = _configuration["RDSSecretArn"],
            ResourceArn = _configuration["RDSResourceArn"]
        });

        var results = JsonSerializer.Deserialize<WorkItem[]>(
            statementResult.FormattedRecords,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        if (results != null && results.Any())
        {
            return results.First();
        }

        throw new NotFoundException($"Work item could not be found with id {itemId}");
    }

    /// <summary>
    /// Get an item by its ID.
    /// </summary>
    /// <param name="itemId">The ID of the item to get.</param>
    /// <returns>True if successful.</returns>
    public async Task<bool> CreateItem(WorkItem workItem)
    {
        // assign a new ID to the work item.
        workItem.ItemId = Guid.NewGuid().ToString();

        // Set up the parameters.
        var parameters = new List<SqlParameter>
        {
            new()
            {
                Name = "itemId",
                Value = new Field { StringValue = workItem.ItemId }
            },
            new()
            {
                Name = "description",
                Value = new Field { StringValue = workItem.Description }
            },
            new()
            {
                Name = "guide",
                Value = new Field { StringValue = workItem.Guide }
            },
            new()
            {
                Name = "status",
                Value = new Field { StringValue = workItem.Status }
            },
            new()
            {
                Name = "userName",
                Value = new Field { StringValue = workItem.UserName }
            },
            new()
            {
                Name = "archiveState",
                Value = new Field { LongValue = (long)workItem.Archive }
            }
        };

        var statementResult = await _amazonRDSDataService.ExecuteStatementAsync(new ExecuteStatementRequest
        {
            Database = _databaseName,
            FormatRecordsAs = "json",
            Sql = $"INSERT INTO {_tableName} VALUES (" +
                  $":itemId," +
                  $"CURRENT_DATE," +
                  $":description," +
                  $":guide," +
                  $":status," +
                  $":userName," +
                  $":archiveState);",
            Parameters = parameters,
            SecretArn = _configuration["RDSSecretArn"],
            ResourceArn = _configuration["RDSResourceArn"]
        });

        return statementResult.HttpStatusCode == HttpStatusCode.OK &&
               statementResult.NumberOfRecordsUpdated == 1;
    }

    /// <summary>
    /// Archive a work item
    /// </summary>
    /// <param name="itemId">The ID of the item to archive.</param>
    /// <returns>True if successful.</returns>
    public async Task<bool> ArchiveItem(string itemId)
    {
        // Set up the parameters.
        var parameters = new List<SqlParameter>
        {
            new()
            {
                Name = "itemId",
                Value = new Field { StringValue = itemId}
            },
            new()
            {
                Name = "archiveState",
                Value = new Field { LongValue = (long)ArchiveState.Archived }
            }
        };

        var statementResult = await _amazonRDSDataService.ExecuteStatementAsync(new ExecuteStatementRequest
        {
            Database = _databaseName,
            FormatRecordsAs = "json",
            Sql = $"UPDATE {_tableName} SET archive = :archiveState WHERE itemId = :itemId;",
            Parameters = parameters,
            SecretArn = _configuration["RDSSecretArn"],
            ResourceArn = _configuration["RDSResourceArn"]
        });

        return statementResult.HttpStatusCode == HttpStatusCode.OK &&
               statementResult.NumberOfRecordsUpdated == 1;
    }

    /// <summary>
    /// This is a test request we will remove later.
    /// </summary>
    /// <returns></returns>
    public async Task<string> TestRequest()
    {
        var testResult = await _amazonRDSDataService.ExecuteStatementAsync(new ExecuteStatementRequest()
        {
            Database = _databaseName,
            FormatRecordsAs = "json",
            Sql = $"Select * FROM {_tableName}",
            SecretArn = _configuration["RDSSecretArn"],
            ResourceArn = _configuration["RDSResourceArn"]
        });
        return testResult.FormattedRecords;
    }
}