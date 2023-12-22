// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved. 
// SPDX-License-Identifier:  Apache-2.0

using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;

namespace ParameterActions;

/// <summary>
/// Encapsulates Systems Manager parameter operations. This example uses these parameters
/// to drive the demonstration of resilient architecture, such as failure of a dependency or
/// how the service responds to a health check.
/// </summary>
public class SmParameterWrapper
{
    private readonly IAmazonSimpleSystemsManagement _amazonSimpleSystemsManagement;

    private readonly string _tableParameter = "doc-example-resilient-architecture-table";
    private readonly string _failureResponseParameter = "doc-example-resilient-architecture-failure-response";
    private readonly string _healthCheckParameter = "doc-example-resilient-architecture-health-check";
    private readonly string _tableName;

    /// <summary>
    /// Constructor for the SMParameterWrapper.
    /// </summary>
    /// <param name="amazonSimpleSystemsManagement">The injection ssm client.</param>
    /// <param name="tableName">The name of the DynamoDB table used for the recommendation service.</param>
    public SmParameterWrapper(IAmazonSimpleSystemsManagement amazonSimpleSystemsManagement, string tableName)
    {
        _amazonSimpleSystemsManagement = amazonSimpleSystemsManagement;
        _tableName = tableName;
    }

    /// <summary>
    /// Reset the Systems Manager parameters to starting values for the demo.
    /// </summary>
    /// <returns>Async task.</returns>
    public async Task Reset()
    {
        await this.PutParameterByName(_tableParameter, _tableName);
        await this.PutParameterByName(_failureResponseParameter, "none");
        await this.PutParameterByName(_healthCheckParameter, "shallow");
    }

    /// <summary>
    /// Set the value of a named Systems Manager parameter.
    /// </summary>
    /// <param name="name">The name of the parameter.</param>
    /// <param name="value">The value to set.</param>
    /// <returns>Async task.</returns>
    public async Task PutParameterByName(string name, string value)
    {
        await _amazonSimpleSystemsManagement.PutParameterAsync(
            new PutParameterRequest() { Name = name, Value = value });
    }
}