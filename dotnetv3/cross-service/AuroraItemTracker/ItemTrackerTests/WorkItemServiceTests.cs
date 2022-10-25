// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
// SPDX-License-Identifier:  Apache-2.0

using Amazon.RDSDataService;
using AuroraItemTracker;
using Microsoft.Extensions.Configuration;
namespace ItemTrackerTests;

/// <summary>
/// Tests for WorkItemService.
/// </summary>
public class WorkItemServiceTests
{
    private readonly IConfiguration _configuration;
    private readonly WorkItemService _workItemService;

    /// <summary>
    /// Constructor for the test class.
    /// </summary>
    public WorkItemServiceTests()
    {
        _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("testsettings.json") // Load test settings from .json file.
            .AddJsonFile("testsettings.local.json",
                true) // Optionally load local settings.
            .Build();

        _workItemService = new WorkItemService(new AmazonRDSDataServiceClient(), _configuration);
    }

    [Fact]
    [Order(1)]
    public async Task VerifyCanExecuteRDSStatement_ShouldReturnResult()
    {
        var tableName = _configuration["WorkItemTable"];
        var result = await _workItemService.ExecuteRDSStatement($"SELECT * FROM {tableName};");
        Assert.NotNull(result.FormattedRecords);
    }
}
