// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved. 
// SPDX-License-Identifier:  Apache-2.0

using Amazon.AutoScaling;
using Amazon.DynamoDBv2;
using Amazon.ElasticLoadBalancing;
using Amazon.IdentityManagement;
using Amazon.IdentityManagement.Model;
using Amazon.SimpleSystemsManagement;
using AutoScalerActions;
using ElasticLoadBalancerActions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging.Debug;
using ParameterActions;

namespace ResilientService;

/// <summary>
/// The scenario to run for the resilience workflow.
/// </summary>
public static class ResilientServiceWorkflow
{
    public static IAmazonIdentityManagementService _iamClient = null!;
    public static ElasticLoadBalancerWrapper _elasticLoadBalancerWrapper = null!;
    public static IConfiguration _configuration = null!;
    static async Task Main(string[] args)
    {
        // Set up dependency injection for the AWS services.
        using var host = Host.CreateDefaultBuilder(args)
            .ConfigureLogging(logging =>
                logging.AddFilter("System", LogLevel.Debug)
                    .AddFilter<DebugLoggerProvider>("Microsoft", LogLevel.Information)
                    .AddFilter<ConsoleLoggerProvider>("Microsoft", LogLevel.Trace))
            .ConfigureServices((_, services) =>
                services.AddAWSService<IAmazonIdentityManagementService>()
                    .AddAWSService<IAmazonDynamoDB>()
                    .AddAWSService<IAmazonElasticLoadBalancing>()
                    .AddAWSService<IAmazonSimpleSystemsManagement>()
                    .AddAWSService<IAmazonAutoScaling>()
                    .AddTransient<AutoScalerWrapper>()
                    .AddTransient<ElasticLoadBalancerWrapper>()
                    .AddTransient<SystemsManagerParameterWrapper>()
                    .AddTransient<RecommendationService.RecommendationService>()
            )
            .Build();

        _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("settings.json") // Load settings from .json file.
            .AddJsonFile("settings.local.json",
                true) // Optionally, load local settings.
            .Build();

        ServicesSetup(host);

        try
        {
            Console.WriteLine(new string('-', 80));
            Console.WriteLine(
                "Welcome to the Amazon Resilient architecture example scenario.");
            Console.WriteLine(
                "\nFor this demo, we'll use the AWS SDK for .NET to create several AWS resources\n" +
                "to set up a load-balanced web service endpoint and explore some ways to make it resilient\n" +
                "against various kinds of failures.\n\n"+
                "Some of the resources create by this demo are:\n");

            Console.WriteLine(
                "\t* A DynamoDB table that the web service depends on to provide book, movie, and song recommendations.");
            Console.WriteLine(
                "\t* An EC2 launch template that defines EC2 instances that each contain a Python web server.");
            Console.WriteLine(
                "\t* An EC2 Auto Scaling group that manages EC2 instances across several Availability Zones.");
            Console.WriteLine(
                "\t* An Elastic Load Balancing (ELB) load balancer that targets the Auto Scaling group to distribute requests.");
            Console.WriteLine(new string('-', 80));
            Console.WriteLine("Press Enter when you're ready to start deploying resources.");
            Console.ReadLine();

            // Create and populate the DynamoDB table.

            // Create the EC2 Launch Template.

            // Create the parameters to control the demo.




            Console.WriteLine(new string('-', 80));
            Console.WriteLine("Finally, let's clean up our resources.");
            Console.WriteLine(new string('-', 80));

            await CleanupResources(true);

            Console.WriteLine(new string('-', 80));
            Console.WriteLine("SageMaker pipeline scenario is complete.");
            Console.WriteLine(new string('-', 80));
        }
        catch (Exception ex)
        {
            Console.WriteLine(new string('-', 80));
            Console.WriteLine($"There was a problem running the scenario: {ex.Message}");
            await CleanupResources(true);
            Console.WriteLine(new string('-', 80));
        }
    }

    /// <summary>
    /// Populate the services for use within the console application.
    /// </summary>
    /// <param name="host">The services host.</param>
    private static void ServicesSetup(IHost host)
    {
        _elasticLoadBalancerWrapper = host.Services.GetRequiredService<ElasticLoadBalancerWrapper>();
        _iamClient = host.Services.GetRequiredService<IAmazonIdentityManagementService>();
    }

    /// <summary>
    /// Clean up the resources from the scenario.
    /// </summary>
    /// <param name="askUser">True to ask the user for cleanup.</param>
    /// <returns>Async task.</returns>
    public static async Task<bool> CleanupResources(bool askUser)
    {
        Console.WriteLine(new string('-', 80));
        Console.WriteLine($"Clean up resources.");


        if (!askUser || GetYesNoResponse($"\tDelete role? (y/n)"))
        {
            Console.WriteLine($"\tDetaching policies and deleting role.");

        }

        Console.WriteLine(new string('-', 80));
        return true;
    }
    /// <summary>
    /// Helper method to get a role's ARN if it already exists.
    /// </summary>
    /// <param name="roleName">The name of the AWS Identity and Access Management (IAM) Role to look for.</param>
    /// <returns>The role ARN if it exists, otherwise an empty string.</returns>
    private static async Task<string> GetRoleArnIfExists(string roleName)
    {
        Console.WriteLine($"Checking for role named {roleName}.");

        try
        {
            var existingRole = await _iamClient.GetRoleAsync(new GetRoleRequest()
            {
                RoleName = roleName
            });
            return existingRole.Role.Arn;
        }
        catch (NoSuchEntityException)
        {
            return string.Empty;
        }
    }

    /// <summary>
    /// Helper method to get a yes or no response from the user.
    /// </summary>
    /// <param name="question">The question string to print on the console.</param>
    /// <returns>True if the user responds with a yes.</returns>
    private static bool GetYesNoResponse(string question)
    {
        Console.WriteLine(question);
        var ynResponse = Console.ReadLine();
        var response = ynResponse != null &&
                       ynResponse.Equals("y",
                           StringComparison.InvariantCultureIgnoreCase);
        return response;
    }

}