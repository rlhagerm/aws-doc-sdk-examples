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
    private static bool _useContentBasedDeduplication = false;
    private static string _topicName = null!;
    private static string _topicArn = null!;
    private static string[] _queueUrls = new string[_queueCount];

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

            Console.WriteLine($"Because you have chosen a FIFO topic, deduplication is supported." +
                              $"\r\nDeduplication IDs are either set in the message or automatically generated " +
                              $"\r\nfrom content using a hash function.\r\n" +
                              $"If a message is successfully published to an SNS FIFO topic, any message " +
                              $"published and determined to have the same deduplication ID, " +
                              $"within the five-minute deduplication interval, is accepted but not delivered.\r\n" +
                              $"For more information about deduplication, " +
                              $"see https://docs.aws.amazon.com/sns/latest/dg/fifo-message-dedup.html.");

            _useContentBasedDeduplication = GetYesNoResponse("Use content-based deduplication instead of entering a deduplication ID?");
        }

        _topicArn = await _snsWrapper.CreateTopicWithName(_topicName, _useFifoTopic, _useContentBasedDeduplication);

        Console.WriteLine($"Your new topic with the name {_topicName}" +
                          $"\r\nand Amazon Resource Name (ARN) {_topicArn}" +
                          $"\r\nhas been created.\r\n");

        Console.WriteLine(new string('-', 80));
    }

    /// <summary>
    /// Set up the queues.
    /// </summary>
    /// <returns>Async task.</returns>
    private static async Task SetupQueues()
    {
        Console.WriteLine(new string('-', 80));
        Console.WriteLine($"Now you will create {_queueCount} Amazon Simple Queue Service (Amazon SQS) queues to subscribe to the topic.");
        Console.WriteLine(new string('-', 80));

        // Repeat this section for each queue.
        for (int i = 0; i < _queueCount; i++)
        {
            Console.Write("Enter a name for an Amazon SQS queue");
            var queueName = Console.ReadLine();
            if (_useFifoTopic)
            {
                // Only explain this once.
                if (i == 0)
                {
                    Console.WriteLine(
                        "Because you have selected a FIFO topic, '.fifo' must be appended to the queue name.");
                    queueName += ".fifo";
                }

                var queueUrl = await _sqsWrapper.CreateQueueWithName(queueName, _useFifoTopic);

               _queueUrls[i] = queueUrl;

               Console.WriteLine($"Your new queue with the name {queueName}" +
                                 $"\r\nand queue URL {queueUrl}" +
                                 $"\r\nhas been created.\r\n");

               if (i == 0)
               {
                   Console.WriteLine(
                       $"The queue URL is used to retrieve the queue ARN,\r\n" +
                       $"which is used to create a subscription.");
               }

               var queueArn = await _sqsWrapper.GetQueueArnByUrl(queueUrl);

               if (i == 0)
               {
                   Console.WriteLine(
                       $"An AWS Identity and Access Management (IAM) policy must be attached to an SQS queue, enabling it to receive\r\n" +
                       $"messages from an SNS topic");
               }

               await _sqsWrapper.SetQueuePolicyForTopic(queueArn, _topicArn, queueUrl);

               await SetupFilters(i, queueArn, queueName);
            }
        }

        Console.WriteLine(new string('-', 80));
    }

    public static async Task SetupFilters(int queueCount, string queueARN, string queueName)
    {
        if (_useFifoTopic)
        {
            // Only explain this once.
            if (queueCount == 0)
            {
                Console.WriteLine(
                    "Subscriptions to a FIFO topic can have filters." +
                    "If you add a filter to this subscription, then only the filtered messages " +
                    "will be received in the queue.");

                Console.WriteLine(
                    "For information about message filtering, " +
                    "see https://docs.aws.amazon.com/sns/latest/dg/sns-message-filtering.html");

                Console.WriteLine(
                    "For this example, you can filter messages by a" +
                    "TONE attribute.");

                var useFilter = GetYesNoResponse($"Filter messages for {queueName}'s subscription to the topic?");
            }
        }
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