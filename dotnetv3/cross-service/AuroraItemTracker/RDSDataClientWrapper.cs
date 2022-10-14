// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
// SPDX-License-Identifier:  Apache-2.0

using Amazon.RDSDataService;
using Amazon.RDSDataService.Model;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;

namespace AuroraItemTracker;

public class RDSDataClientWrapper
{
    private readonly IAmazonRDSDataService _amazonRDSDataService;
    private readonly IConfiguration _configuration;
    private readonly IAmazonSecretsManager _secretsManager;

    /// <summary>
    /// Constructor that uses the injected Amazon RDS Data Service clientr.
    /// </summary>
    /// <param name="amazonTranscribeService">Amazon RDS Data Service</param>
    /// <param name="secretsManager">Amazon Secrets Manager</param>
    public RDSDataClientWrapper(IAmazonRDSDataService amazonRDSDataService, IConfiguration configuration)
    {
        _amazonRDSDataService = amazonRDSDataService;
        _configuration = configuration;
        //_secretsManager = secretsManager;
    }

    public async Task<string> TestRequest()
    {
        // todo: put the secret name in a config file
        //var testSecretResponse = await _secretsManager.GetSecretValueAsync(new GetSecretValueRequest()
        //{
        //    SecretId = "docexampleauroraappsecret8B-pn3506tFqGqZ"
        //});

        // todo: we won't do it this way!
        // todo: move to a config before I push it.
        var testResult = await _amazonRDSDataService.ExecuteStatementAsync(new ExecuteStatementRequest()
        {
            Database = "auroraappdb",
            FormatRecordsAs = "json",
            Sql = "Select * FROM work_items_test1",
            SecretArn = _configuration["RDSSecretArn"],
            ResourceArn = _configuration["RDSResourceArn"]
        });
        return testResult.FormattedRecords;
    }
}