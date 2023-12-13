﻿// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved. 
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
using RecommendationService;

namespace ResilientService;

/// <summary>
/// The scenario to run for the resilience workflow.
/// </summary>
public static class ResilientServiceWorkflow
{
    public static IAmazonIdentityManagementService _iamClient = null!;
    public static ElasticLoadBalancerWrapper _elasticLoadBalancerWrapper = null!;
    public static AutoScalerWrapper _autoScalerWrapper = null!;
    public static Recommendations _recommendations = null!;
    public static SMParameterWrapper _smParameterWrapper = null!;
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
                    .AddTransient<SMParameterWrapper>()
                    .AddTransient<Recommendations>()
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
            Console.WriteLine("Welcome to the Resilient Architecture Example Scenario.");
            Console.WriteLine(new string('-', 80));
            await Deploy();

            Console.WriteLine("Now let's begin the scenario.");
            Console.WriteLine(new string('-', 80));
            await Demo();

            Console.WriteLine(new string('-', 80));
            Console.WriteLine("Finally, let's clean up our resources.");
            Console.WriteLine(new string('-', 80));

            await DestroyResources(true);

            Console.WriteLine(new string('-', 80));
            Console.WriteLine("Resilient Architecture Example Scenario is complete.");
            Console.WriteLine(new string('-', 80));
        }
        catch (Exception ex)
        {
            Console.WriteLine(new string('-', 80));
            Console.WriteLine($"There was a problem running the scenario: {ex.Message}");
            await DestroyResources(true);
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
        _recommendations = host.Services.GetRequiredService<Recommendations>();
        _autoScalerWrapper = host.Services.GetRequiredService<AutoScalerWrapper>();
        _smParameterWrapper = host.Services.GetRequiredService<SMParameterWrapper>();
    }

    /// <summary>
    /// Deploy necessary resources for the scenario.
    /// </summary>
    /// <returns>Async task.</returns>
    public static async Task Deploy()
    {
        var protocol = "HTTP";
        var port = 80;
        var sshPort = 22;

        Console.WriteLine(
            "\nFor this demo, we'll use the AWS SDK for .NET to create several AWS resources\n" +
            "to set up a load-balanced web service endpoint and explore some ways to make it resilient\n" +
            "against various kinds of failures.\n\n" +
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
        var databaseTableName = _configuration["databaseName"];
        var recommendationsPath = Path.Join(_configuration["resourcePath"],
            "recommendations.json");
        Console.WriteLine($"Creating and populating a DynamoDB table named {databaseTableName}.");
        _recommendations.CreateDatabase(databaseTableName);
        _recommendations.PopulateDatabase(databaseTableName, recommendationsPath);

        Console.WriteLine(new string('-', 80));

        // Create the EC2 Launch Template.

        Console.WriteLine(
            "Creating an EC2 launch template that runs '{startup_script}' when an instance starts.\n"
            + "This script starts a Python web server defined in the `server.py` script. The web server\n"
            + "listens to HTTP requests on port 80 and responds to requests to '/' and to '/healthcheck'.\n"
            + "For demo purposes, this server is run as the root user. In production, the best practice is to\n"
            + "run a web server, such as Apache, with least-privileged credentials.");
        Console.WriteLine(
            "The template also defines an IAM policy that each instance uses to assume a role that grants\n"
            + "permissions to access the DynamoDB recommendation table and Systems Manager parameters\n"
            + "that control the flow of the demo.");

        var startupScriptPath = Path.Join(_configuration["resourcePath"],
            "server_startup_script.sh");
        var instancePolicyPath = Path.Join(_configuration["resourcePath"],
            "instance_policy.json");
        _autoScalerWrapper.CreateTemplate(startupScriptPath, instancePolicyPath);
        Console.WriteLine(new string('-', 80));

        Console.WriteLine(
            "Creating an EC2 Auto Scaling group that maintains three EC2 instances, each in a different\n"
            + "Availability Zone.\n");

        var zones = await _autoScalerWrapper.createGroup(3);
        Console.WriteLine(new string('-', 80));

        Console.WriteLine(
            "At this point, you have EC2 instances created. Once each instance starts, it listens for\n"
            + "HTTP requests. You can see these instances in the console or continue with the demo.\n");

        Console.WriteLine(new string('-', 80));
        Console.WriteLine("Press Enter when you're ready to continue.");
        Console.ReadLine();

        Console.WriteLine("Creating variables that control the flow of the demo.");
        _smParameterWrapper.Reset();

        Console.WriteLine(
            "\nCreating an Elastic Load Balancing target group and load balancer. The target group\n"
            + "defines how the load balancer connects to instances. The load balancer provides a\n"
            + "single endpoint where clients connect and dispatches requests to instances in the group.");

        var defaultVpc = _autoScalerWrapper.GetDefaultVpc();
        var subnets = _autoScalerWrapper.GetSubnets(defaultVpc);
        var targetGroup = _elasticLoadBalancerWrapper.CreateTargetGroup(protocol, port, defaultVpc.VpcId);

        _elasticLoadBalancerWrapper.CreateLoadBalancer(subnets.Select(s => s.SubNetId), targetGroup);
        _elasticLoadBalancerWrapper.AttachLoadBalancer(targetGroup);
        Console.WriteLine("Verifying access to the load balancer endpoint...");
        var loadBalancerAccess = _elasticLoadBalancerWrapper.VerifyLoadBalancerEndpoint();

        if (!loadBalancerAccess)
        {
            Console.WriteLine("Couldn't connect to the load balancer, verifying that the port is open...");
            var httpClient = new HttpClient();
            var ipString = await httpClient.GetStringAsync("https://checkip.amazonaws.com");
            ipString = ipString.Trim();

            var portIsOpen = _autoScalerWrapper.VerifyInboundPort(defaultVpc, port, ipString);
            var sshPortIsOpen = _autoScalerWrapper.VerifyInboundPort(defaultVpc, sshPort, ipString);

            if (!portIsOpen)
            {
                Console.WriteLine(
                    "For this example to work, the default security group for your default VPC must\n"
                    + "allows access from this computer. You can either add it automatically from this\n"
                    + "example or add it yourself using the AWS Management Console.\n");

                if (GetYesNoResponse(
                        "Do you want to add a rule to the security group to allow inbound traffic from your computer's IP address?"))
                {
                    _autoScalerWrapper.OpenInboundPort(securityGroupId, port, ipString);
                }
            }

            if (!sshPortIsOpen)
            {
                if (GetYesNoResponse(
                        "Do you want to add a rule to the security group to allow inbound SSH traffic for debugging from your computer's IP address?"))
                {
                    _autoScalerWrapper.OpenInboundPort(securityGroupId, sshPort, ipString);
                }
            }
            loadBalancerAccess = _elasticLoadBalancerWrapper.VerifyLoadBalancerEndpoint();
        }

        if (loadBalancerAccess)
        {
            Console.WriteLine("Your load balancer is ready. You can access it by browsing to:");
            Console.WriteLine($"\thttp://{_elasticLoadBalancerWrapper.GetEndpoint()}\n");
        }
        else {
            Console.WriteLine(
                "Couldn't get a successful response from the load balancer endpoint. Troubleshoot by\n"
                + "manually verifying that your VPC and security group are configured correctly and that\n"
                + "you can successfully make a GET request to the load balancer endpoint:\n");
            Console.WriteLine($"\thttp://{_elasticLoadBalancerWrapper.GetEndpoint()}\n");
        }
        Console.WriteLine(new string('-', 80));
        Console.WriteLine("Press Enter when you're ready to continue with the demo.");
        Console.ReadLine();
    }

    /// <summary>
    /// Demonstrate the steps of the scenario.
    /// </summary>
    /// <returns>Async task.</returns>
    public static async Task<bool> Demo()
    {
        var ssmOnlyPolicy = Path.Join(_configuration["resourcePath"],
            "ssm_only_policy.json");

        Console.WriteLine(new string('-', 80));
        Console.WriteLine("Resetting parameters to starting values for demo.");
        _smParameterWrapper.Reset();

        Console.WriteLine("\nThis part of the demonstration shows how to toggle different parts of the system\n" +
                          "to create situations where the web service fails, and shows how using a resilient\n" +
                          "architecture can keep the web service running in spite of these failures.");
        Console.WriteLine(new string('-', 88));
        Console.WriteLine("At the start, the load balancer endpoint returns recommendations and reports that all targets are healthy.");
        await DemoActionChoices();

        Console.WriteLine($"The web service running on the EC2 instances gets recommendations by querying a DynamoDB table.\n" +
                          $"The table name is contained in a Systems Manager parameter named '{_smParameterWrapper.Table}'.\n" +
                          $"To simulate a failure of the recommendation service, let's set this parameter to name a non-existent table.\n");
        _smParameterWrapper.PutParameter(_smParameterWrapper.Table, "this-is-not-a-table");
        Console.WriteLine("\nNow, sending a GET request to the load balancer endpoint returns a failure code. But, the service reports as\n" +
                          "healthy to the load balancer because shallow health checks don't check for failure of the recommendation service.");
        await DemoActionChoices();

        Console.WriteLine("Instead of failing when the recommendation service fails, the web service can return a static response.");
        Console.WriteLine("While this is not a perfect solution, it presents the customer with a somewhat better experience than failure.");

        _smParameterWrapper.PutParameter(_smParameterWrapper.FailureResponse, "static");

        Console.WriteLine("\nNow, sending a GET request to the load balancer endpoint returns a static response.");
        Console.WriteLine("The service still reports as healthy because health checks are still shallow.");
        await DemoActionChoices();

        Console.WriteLine("Let's reinstate the recommendation service.\n");
        _smParameterWrapper.PutParameter(_smParameterWrapper.Table, _recommendations.TableName);
        Console.WriteLine(
            "\nLet's also substitute bad credentials for one of the instances in the target group so that it can't\n" +
            "access the DynamoDB recommendation table.\n"
        );
        _autoScalerWrapper.CreateInstanceProfile(
            ssmOnlyPolicy,
            _autoScalerWrapper.BadCredsPolicyName,
            _autoScalerWrapper.BadCredsRoleName,
            _autoScalerWrapper.BadCredsProfileName,
            new List<string> { "AmazonSSMManagedInstanceCore" }
        );
        var instances = _autoScalerWrapper.GetInstances();
        var bad_instance_id = instances[0];
        var instance_profile = _autoScalerWrapper.GetInstanceProfile(bad_instance_id);
        Console.WriteLine(
            $"Replacing the profile for instance {bad_instance_id} with a profile that contains\n" +
            "bad credentials...\n"
        );
        _autoScalerWrapper.ReplaceInstanceProfile(
            bad_instance_id,
            _autoScalerWrapper.BadCredsProfileName,
            instance_profile["AssociationId"]
        );
        Console.WriteLine(
            "Now, sending a GET request to the load balancer endpoint returns either a recommendation or a static response,\n" +
            "depending on which instance is selected by the load balancer.\n"
        );
        await DemoActionChoices();

        Console.WriteLine("\nLet's implement a deep health check. For this demo, a deep health check tests whether");
        Console.WriteLine("the web service can access the DynamoDB table that it depends on for recommendations. Note that");
        Console.WriteLine("the deep health check is only for ELB routing and not for Auto Scaling instance health.");
        Console.WriteLine("This kind of deep health check is not recommended for Auto Scaling instance health, because it");
        Console.WriteLine("risks accidental termination of all instances in the Auto Scaling group when a dependent service fails.");

        Console.WriteLine("\nBy implementing deep health checks, the load balancer can detect when one of the instances is failing");
        Console.WriteLine("and take that instance out of rotation.");

        _smParameterWrapper.PutParameter(_smParameterWrapper.HealthCheck, "deep");

        Console.WriteLine($"\nNow, checking target health indicates that the instance with bad credentials ({bad_instance_id})");
        Console.WriteLine("is unhealthy. Note that it might take a minute or two for the load balancer to detect the unhealthy");
        Console.WriteLine("instance. Sending a GET request to the load balancer endpoint always returns a recommendation, because");
        Console.WriteLine("the load balancer takes unhealthy instances out of its rotation.");

        await DemoActionChoices();

        Console.WriteLine("\nBecause the instances in this demo are controlled by an auto scaler, the simplest way to fix an unhealthy");
        Console.WriteLine("instance is to terminate it and let the auto scaler start a new instance to replace it.");

        _autoScalerWrapper.TerminateInstance(BadInstanceId);

        Console.WriteLine($"\nEven while the instance is terminating and the new instance is starting, sending a GET");
        Console.WriteLine("request to the web service continues to get a successful recommendation response because");
        Console.WriteLine("starts and reports as healthy, it is included in the load balancing rotation.");
        Console.WriteLine("Note that terminating and replacing an instance typically takes several minutes, during which time you");
        Console.WriteLine("can see the changing health check status until the new instance is running and healthy.");

        await DemoActionChoices();

        Console.WriteLine("\nIf the recommendation service fails now, deep health checks mean all instances report as unhealthy.");

        _smParameterWrapper.PutParameter(_smParameterWrapper.Table, "this-is-not-a-table");

        Console.WriteLine($"\nWhen all instances are unhealthy, the load balancer continues to route requests even to");
        Console.WriteLine("unhealthy instances, allowing them to fail open and return a static response rather than fail");
        Console.WriteLine("closed and report failure to the customer.");

        await DemoActionChoices();
        _smParameterWrapper.Reset();

        Console.WriteLine(new string('-', 80));
        return true;
    }

    /// <summary>
    /// Clean up the resources from the scenario.
    /// </summary>
    /// <param name="askUser">True to ask the user for cleanup.</param>
    /// <returns>Async task.</returns>
    public static async Task<bool> DestroyResources(bool askUser)
    {
        Console.WriteLine(new string('-', 80));
        Console.WriteLine(
            "To keep things tidy and to avoid unwanted charges on your account, we can clean up all AWS resources\n" +
            "that were created for this demo."
        );

        if (GetYesNoResponse("Do you want to clean up all demo resources? (y/n) "))
        {
            _elasticLoadBalancerWrapper.DeleteLoadBalancer();
            _elasticLoadBalancerWrapper.DeleteTargetGroup();
            _autoScalerWrapper.DeleteGroup();
            _autoScalerWrapper.DeleteKeyPair();
            _autoScalerWrapper.DeleteTemplate();
            _autoScalerWrapper.DeleteInstanceProfile(
                _autoScalerWrapper.BadCredsProfileName,
                _autoScalerWrapper.BadCredsRoleName
            );
            _recommendations.Destroy();
        }
        else
        {
            Console.WriteLine(
                "Okay, we'll leave the resources intact.\n" +
                "Don't forget to delete them when you're done with them or you might incur unexpected charges."
            );
        }

        Console.WriteLine(new string('-', 80));
        return true;
    }

    /// <summary>
    /// Present the user with the demo action choices.
    /// </summary>
    /// <returns>Async task.</returns>
    public static async Task<bool> DemoActionChoices()
    {
        Console.WriteLine(new string('-', 80));
        var choices = new string[]{
            "Send a GET request to the load balancer endpoint.",
            "Check the health of load balancer targets.",
            "Go to the next part of the demo."};

        var choice = 0;
        // Keep asking the user until they choose to move on.
        while (choice != 2)
        {
            choice = GetChoiceResponse(
                "See the current state of the service by selecting one of the following choices:"
                , choices);

            switch (choice)
            {
                case 0:
                    {
                        Console.WriteLine("GET request response.");
                        var response = _elasticLoadBalancerWrapper.GetEndPointResponse();
                        Console.WriteLine(response);
                        break;
                    }
                case 1:
                    {
                        Console.WriteLine("Checking the health of load balancer targets.");
                        var health = _elasticLoadBalancerWrapper.CheckTargetHealth();
                        // Print the state of the targets.
                        foreach (var target in health)
                        {
                            /*
                             * state = target["TargetHealth"]["State"]
                    print(
                        f"\tTarget {target['Target']['Id']} on port {target['Target']['Port']} is {state}"
                    )
                    if state != "healthy":
                        print(
                            f"\t\t{target['TargetHealth']['Reason']}: {target['TargetHealth']['Description']}\n"
                        )
                             */
                        }

                        Console.WriteLine("Note that it can take a minute or two for the health check to update" +
                                          "\nafter changes are made.");
                        break;

                    }
                default:
                    {
                        Console.WriteLine("Okay, let's move on.");
                        break;
                    }
            }
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

    /// <summary>
    /// Helper method to get a choice response from the user.
    /// </summary>
    /// <param name="question">The question string to print on the console.</param>
    /// <param name="choices">The choices to print on the console.</param>
    /// <returns>The index of the selected choice</returns>
    private static int GetChoiceResponse(string question, string[] choices)
    {
        Console.WriteLine(question);

        for (int i = 0; i < choices.Length; i++)
        {
            Console.WriteLine($"\t{i + 1}. {choices[i]}");
        }

        var choiceNumber = 0;
        while (choiceNumber < 1 || choiceNumber > choices.Length)
        {
            var choice = Console.ReadLine();
            Int32.TryParse(choice, out choiceNumber);
        }

        return choiceNumber - 1;
    }
}