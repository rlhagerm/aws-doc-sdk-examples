// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
// SPDX-License-Identifier:  Apache-2.0

using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Amazon.SQS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging.Debug;
using SNSActions;
using SQSActions;

namespace TopicsAndQueuesScenario;

/*
 * Before running this code example, set up your development environment, including your credentials.
 *
 * Purpose
 *
 * This example demonstrates messaging with topics and queues using Amazon Simple Notification
 * Service (Amazon SNS) and Amazon Simple Queue Service (Amazon SQS).
 *
 * 1.  Create an SNS topic, either FIFO (First-In-First-Out) or non-FIFO. (CreateTopic)
 * 2.  Create an SQS queue. (CreateQueue)
 * 3.  Get the SQS queue ARN attribute. (GetQueueAttributes)
 * 4.  Set the SQS queue policy attribute with a policy enabling the receipt of SNS messages. (SetQueueAttributes)
 * 5.  Subscribe the SQS queue to the SNS topic. (Subscribe)
 * 6.  Publish a message to the SNS topic. (Publish)
 * 7.  Poll an SQS queue for its messages. (ReceiveMessage)
 * 8.  Delete a batch of messages from an SQS queue. (DeleteMessageBatch)
 * 9.  Delete an SQS queue. (DeleteQueue)
 * 10. Unsubscribe an SNS subscription. (Unsubscribe)
 * 11. Delete an SNS topic. (DeleteTopic)
 *
 */

// snippet-start:[TopicsAndQueues.dotnetv3.Scenario]
public class TopicsAndQueues
{
    private static ILogger logger = null!;
    private static SNSWrapper _snsWrapper = null!;
    private static SQSWrapper _sqsWrapper = null!;
    private static IConfiguration _configuration = null!;
    private static int _queueCount = 2;
    private static bool _useFifoTopic = false;
    private static string _topicName = null!;

    static async Task Main(string[] args)
    {
        // Set up dependency injection for Amazon EventBridge.
        using var host = Host.CreateDefaultBuilder(args)
            .ConfigureLogging(logging =>
                logging.AddFilter("System", LogLevel.Debug)
                    .AddFilter<DebugLoggerProvider>("Microsoft", LogLevel.Information)
                    .AddFilter<ConsoleLoggerProvider>("Microsoft", LogLevel.Trace))
            .ConfigureServices((_, services) =>
                services.AddAWSService<IAmazonSQS>()
                    .AddAWSService<IAmazonSimpleNotificationService>()
                    .AddTransient<SNSWrapper>()
                    .AddTransient<SQSWrapper>()
            )
            .Build();

        _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("settings.json") // Load settings from .json file.
            .AddJsonFile("settings.local.json",
                true) // Optionally, load local settings.
            .Build();

        logger = LoggerFactory.Create(builder => { builder.AddConsole(); })
            .CreateLogger<TopicsAndQueues>();

        PrintDescription();



        ServicesSetup(host);
    }

    /// <summary>
    /// Populate the services for use within the console application.
    /// </summary>
    /// <param name="host">The services host.</param>
    private static void ServicesSetup(IHost host)
    {
        _snsWrapper = host.Services.GetRequiredService<SNSWrapper>();
        _sqsWrapper = host.Services.GetRequiredService<SQSWrapper>();
    }

    // snippet-start:[TopicsAndQueues.dotnetv3.CreatePolicy]
    /// <summary>
    /// Create a policy for the queue.
    /// </summary>
    /// <returns>The role Amazon Resource Name (ARN).</returns>
    public static async Task<string> CreatePolicy(string queueArn, string topicArn)
    {
        Console.WriteLine(new string('-', 80));
        Console.WriteLine("Creating a role to use with EventBridge and attaching managed policy AmazonEventBridgeFullAccess.");
        Console.WriteLine(new string('-', 80));


        // Allow time for the role to be ready.
        Thread.Sleep(10000);
        return "";
    }
    // snippet-end:[EventBridge.dotnetv3.CreateRole]

    /// <summary>
    /// Clean up the resources from the scenario.
    /// </summary>
    /// <returns>Async task.</returns>
    private static void PrintDescription()
    {
        Console.WriteLine(new string('-', 80));
        Console.WriteLine($"Welcome to messaging with topics and queues.");

        Console.WriteLine(new string('-', 80));
        Console.WriteLine($"In this workflow, you will create an SNS topic and subscribe {_queueCount} SQS queues to the topic." +
                          $"\r\nYou can select from several options for configuring the topic and the subscriptions for the 2 queues." +
                          $"\r\nYou can then post to the topic and see the results in the queues.\r\n");

        Console.WriteLine(new string('-', 80));
        Console.WriteLine($"SNS topics can be configured as FIFO (First-In-First-Out)." +
                          $"\r\nFIFO topics deliver messages in order and support deduplication and message filtering." +
                          $"\r\nYou can then post to the topic and see the results in the queues.\r\n");

        Console.WriteLine(new string('-', 80));
    }

    /// <summary>
    /// Set up the SNS topic to be used with the queues.
    /// </summary>
    /// <returns>Async task.</returns>
    private static async Task SetupTopic()
    {
        Console.WriteLine(new string('-', 80));
        Console.WriteLine($"SNS topics can be configured as FIFO (First-In-First-Out)." +
                          $"\r\nFIFO topics deliver messages in order and support deduplication and message filtering." +
                          $"\r\nYou can then post to the topic and see the results in the queues.\r\n");

        _useFifoTopic = GetYesNoResponse("Would you like to work with FIFO topics?");

        if (_useFifoTopic)
        {
            Console.Write("Enter a name for your SNS topic:");
            _topicName = Console.ReadLine();
            Console.WriteLine(
                "Because you have selected a FIFO topic, '.fifo' must be appended to the topic name.");
            _topicName += ".fifo";
        }

        var topicArn = await _snsWrapper.CreateTopic(_topicName, _useFifoTopic);

        Console.WriteLine(new string('-', 80));
    }

    /// <summary>
    /// Set up the queues.
    /// </summary>
    /// <returns>Async task.</returns>
    private static async Task SetupQueues()
    {
        Console.WriteLine(new string('-', 80));
        Console.WriteLine($"Now you will create 2 SQS queues to subscribe to the topic.");
        Console.WriteLine(new string('-', 80));

        // Repeat this section for each queue.
        for (int i = 0; i < _queueCount; i++)
        {
            Console.Write("Enter a name for an SQS queue");
            var queueName = Console.ReadLine();
            if (_useFifoTopic)
            {
                Console.WriteLine(
                    "Because you have selected a FIFO topic, '.fifo' must be appended to the topic name.");
                _topicName += ".fifo";
            }
        }

        Console.WriteLine(new string('-', 80));
    }

    /// <summary>
    /// Clean up the resources from the scenario.
    /// </summary>
    /// <returns>Async task.</returns>
    private static async Task CleanupResources(string topicArn)
    {
        Console.WriteLine(new string('-', 80));
        Console.WriteLine($"Clean up resources.");


        Console.WriteLine(new string('-', 80));
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
// snippet-end:[TopicsAndQueues.dotnetv3.Scenario]