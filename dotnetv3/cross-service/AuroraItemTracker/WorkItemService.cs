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
    private readonly string _databaseName;
    private readonly string _tableName;

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
    /// Execute a SQL statement using the Amazon RDS Data Service
    /// </summary>
    /// <param name="sql">The SQL statement.</param>
    /// <param name="parameters">Optional parameters for the statement.</param>
    /// <returns>The statement response.</returns>
    public async Task<ExecuteStatementResponse> ExecuteRDSStatement(string sql, List<SqlParameter> parameters = null!)
    {
        var statementResult = await _amazonRDSDataService.ExecuteStatementAsync(
            new ExecuteStatementRequest
            {
                Database = _databaseName,
                FormatRecordsAs = "json",
                Sql = sql,
                Parameters = parameters,
                SecretArn = _configuration["RDSSecretArn"],
                ResourceArn = _configuration["RDSResourceArn"]
            });
        return statementResult;
    }

    /// <summary>
    /// Convert a statement response to a collection of WorkItems.
    /// </summary>
    /// <param name="statementResult">The response from the data service.</param>
    /// <returns>The collection of WorkItems.</returns>
    public WorkItem[] GetItemsFromResponse(ExecuteStatementResponse statementResult)
    {
        var results = JsonSerializer.Deserialize<WorkItem[]>(
            statementResult.FormattedRecords,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        return results!;
    }

    /// <summary>
    /// Add a string parameter to a parameter collection.
    /// </summary>
    /// <param name="parameters">The parameter collection.</param>
    /// <param name="name">Name of the parameter.</param>
    /// <param name="value">Value for the parameter.</param>
    public void AddStringParameter(List<SqlParameter> parameters, string name, string value)
    {
        parameters.Add( new SqlParameter()
        {
            Name = name,
            Value = new Field { StringValue = value }
        });
    }

    /// <summary>
    /// Add a numeric parameter to a parameter collection.
    /// </summary>
    /// <param name="parameters">The parameter collection.</param>
    /// <param name="name">Name of the parameter.</param>
    /// <param name="value">Value for the parameter.</param>
    public void AddNumericParameter(List<SqlParameter> parameters, string name, long value)
    {
        parameters.Add(new SqlParameter()
        {
            Name = name,
            Value = new Field { LongValue = value }
        });
    }

    /// <summary>
    /// Get all items.
    /// </summary>
    /// <returns>A collection of WorkItems.</returns>
    public async Task<IList<WorkItem>> GetAllItems()
    {
        var statementResult = await ExecuteRDSStatement($"SELECT * FROM {_tableName};");

        var results = GetItemsFromResponse(statementResult);

        return results!;
    }

    /// <summary>
    /// Get the items with a particular archive state.
    /// </summary>
    /// <param name="archiveState">The archive state of the items to get.</param>
    /// <returns>A collection of WorkItems.</returns>
    public async Task<IList<WorkItem>> GetItemsByArchiveState(ArchiveState archiveState)
    {
        // Set up the parameters.
        var parameters = new List<SqlParameter>();
        AddNumericParameter(parameters, "isArchived", (long)archiveState);
        
        var statementResult = await ExecuteRDSStatement(
            $"SELECT * FROM {_tableName} WHERE archived = :isArchived;",
            parameters);

        var results = GetItemsFromResponse(statementResult);
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
        var parameters = new List<SqlParameter>();
        AddStringParameter(parameters, "itemId", itemId);

        var statementResult = await ExecuteRDSStatement(
            $"SELECT * FROM {_tableName} WHERE iditem = :itemId;",
            parameters);

        var results = GetItemsFromResponse(statementResult);

        return results.Any()
            ? results.First()
            : throw new NotFoundException(
                $"Work item could not be found with id {itemId}");
    }

    /// <summary>
    /// Get an item by its ID.
    /// </summary>
    /// <param name="itemId">The ID of the item to get.</param>
    /// <returns>True if successful.</returns>
    public async Task<bool> CreateItem(WorkItem workItem)
    {
        // Assign a new ID to the work item.
        workItem.IdItem = Guid.NewGuid().ToString();

        // Set up the parameters.
        var parameters = new List<SqlParameter>();
        AddStringParameter(parameters, "itemId", workItem.IdItem);
        AddStringParameter(parameters, "description", workItem.Description);
        AddStringParameter(parameters, "guide", workItem.Guide);
        AddStringParameter(parameters, "status", workItem.Status);
        AddStringParameter(parameters, "userName", workItem.Name);
        AddNumericParameter(parameters, "archiveState", (long)workItem.Archived);

        var statementResult = await ExecuteRDSStatement(
            $"INSERT INTO {_tableName} VALUES (" +
            $":itemId," +
            $":description," +
            $":guide," +
            $":status," +
            $":userName," +
            $":archiveState);",
            parameters);

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
        var parameters = new List<SqlParameter>();
        AddStringParameter(parameters, "itemId", itemId);
        AddNumericParameter(parameters, "archiveState", (long)ArchiveState.Archived);

        var statementResult = await ExecuteRDSStatement(
            $"UPDATE {_tableName} SET archived = :archiveState WHERE iditem = :itemId;",
            parameters);

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