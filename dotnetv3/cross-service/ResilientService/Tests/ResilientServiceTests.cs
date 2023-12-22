// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved. 
// SPDX-License-Identifier:  Apache-2.0

using Amazon.AutoScaling;
using Amazon.EC2;
using Amazon.ElasticLoadBalancingV2;
using Amazon.IdentityManagement;
using Amazon.SimpleSystemsManagement;
using AutoScalerActions;
using ElasticLoadBalancerActions;
using Microsoft.Extensions.Configuration;
using ParameterActions;
using RecommendationService;
using Xunit.Extensions.Ordering;

namespace ResilientServiceTests;

/// <summary>
/// Tests for the Resilient Service example.
/// </summary>
public class ResilientServiceTests
{
    private readonly IConfiguration _configuration;

    private readonly ElasticLoadBalancerWrapper _elasticLoadBalancerWrapper = null!;
    private readonly AutoScalerWrapper _autoScalerWrapper = null!;
    private readonly Recommendations _recommendations = null!;
    private readonly SmParameterWrapper _smParameterWrapper = null!;

    private readonly string _resourcePath;
    private readonly string _databaseName;
    private readonly string _resourcePrefix;
    private readonly string _instanceType;
    private readonly string _amiParam;

    /// <summary>
    /// Constructor for the test class.
    /// </summary>
    public ResilientServiceTests()
    {
        _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("testsettings.json") // Load test settings from .json file.
            .AddJsonFile("testsettings.local.json",
                true) // Optionally, load local settings.
            .Build();

        _resourcePath = _configuration["resourcePath"];
        _databaseName = _configuration["databaseName"];
        _resourcePrefix = _configuration["resourcePrefix"];
        _instanceType = _configuration["instanceType"];
        _amiParam = _configuration["amiParam"];

        _elasticLoadBalancerWrapper = new ElasticLoadBalancerWrapper(
            new AmazonElasticLoadBalancingV2Client(),
            _configuration);
        _autoScalerWrapper = new AutoScalerWrapper(
            new AmazonAutoScalingClient(),
            new AmazonEC2Client(),
            new AmazonSimpleSystemsManagementClient(),
            new AmazonIdentityManagementServiceClient(),
            _configuration);
        _recommendations = new Recommendations();
        _smParameterWrapper =
            new SmParameterWrapper(new AmazonSimpleSystemsManagementClient(),
                _databaseName);

    }


    [Fact]
    [Order(1)]
    [Trait("Category", "Integration")]
    public void TestDemo()
    {

    }

    [Fact]
    [Order(2)]
    [Trait("Category", "Integration")]
    public void TestDeploy()
    {

    }

    [Fact]
    [Order(3)]
    [Trait("Category", "Integration")]
    public void TestDestroy()
    {

    }
}