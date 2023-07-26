// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
// SPDX-License-Identifier:  Apache-2.0

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Logging;

namespace SQSActions;

// snippet-start:[TopicsAndQueues.dotnetv3.SQSWrapper]

/// <summary>
/// Wrapper for Amazon Simple Queue Service (SQS) operations.
/// </summary>
public class SQSWrapper
{
    private readonly IAmazonSQS _amazonSQSClient;
    private readonly ILogger<SQSWrapper> _logger;

    /// <summary>
    /// Constructor for the Amazon SQS wrapper.
    /// </summary>
    /// <param name="amazonSQS">The injected Amazon SQS client.</param>
    /// <param name="logger">The injected logger for the wrapper.</param>
    public SQSWrapper(IAmazonSQS amazonSQS, ILogger<SQSWrapper> logger)

    {
        _amazonSQSClient = amazonSQS;
        _logger = logger;
    }


    // snippet-start:[TopicsAndQueues.dotnetv3.CreateQueue]
    /// <summary>
    /// Create a queue with a specific name.
    /// </summary>
    /// <param name="queueName">The name for the queue.</param>
    /// <param name="useFifoQueue">True to use a FIFO queue.</param>
    /// <returns>The url for the queue.</returns>
    public async Task<string> CreateQueueWithName(string queueName, bool useFifoQueue)
    {
        var createQueueRequest = new CreateQueueRequest()
        {
            QueueName = queueName,
        };

        if (useFifoQueue)
        {
            // Update the name if it is not correct for a FIFO queue.
            if (!queueName.EndsWith(".fifo"))
            {
                createQueueRequest.QueueName = queueName + ".fifo";
            }

            // Add the attributes from the method parameters.
            createQueueRequest.Attributes = new Dictionary<string, string>
            {
                { QueueAttributeName.FifoQueue, "true" }
            };
        }

        var createResponse = await _amazonSQSClient.CreateQueueAsync(
            new CreateQueueRequest()
            {
                QueueName = queueName
            });
        return createResponse.QueueUrl;
    }
    // snippet-end:[TopicsAndQueues.dotnetv3.CreateQueue]

    // snippet-start:[TopicsAndQueues.dotnetv3.GetQueueAttributes]
    /// <summary>
    /// Get the ARN for a queue from its URL.
    /// </summary>
    /// <param name="queueUrl">The URL of the queue.</param>
    /// <returns>The ARN of the queue.</returns>
    public async Task<string> GetQueueArnByUrl(string queueUrl)
    {
        var getAttributesRequest = new GetQueueAttributesRequest()
        {
            QueueUrl = queueUrl,
            AttributeNames = new List<string>() {QueueAttributeName.QueueArn}
        };

        var getAttributesResponse = await _amazonSQSClient.GetQueueAttributesAsync(
            getAttributesRequest);

        return getAttributesResponse.QueueARN;
    }
    // snippet-end:[TopicsAndQueues.dotnetv3.GetQueueAttributes]

    // snippet-start:[TopicsAndQueues.dotnetv3.GetQueueAttributes]
    /// <summary>
    /// Get the policy by requesting the attributes for the queue.
    /// </summary>
    /// <param name="queueUrl">The url for the queue.</param>
    /// <returns>The policy for the queue.</returns>
    public async Task<string> GetQueuePolicyByUrl(string queueUrl)
    {
        var attributesResponse = await _amazonSQSClient.GetQueueAttributesAsync(
            new GetQueueAttributesRequest()
            {
                QueueUrl = queueUrl
            });
        return attributesResponse.Policy;
    }
    // snippet-end:[TopicsAndQueues.dotnetv3.GetQueueAttributes]

    // snippet-start:[TopicsAndQueues.dotnetv3.SetQueueAttributes]
    /// <summary>
    /// Set the policy by setting the attributes for the queue using the topic and queue ARNs.
    /// </summary>
    /// <param name="queueArn">The ARN of the queue.</param>
    /// <param name="topicArn">The ARN of the topic.</param>
    /// <param name="queueUrl">The url for the queue.</param>
    /// <returns>True if successful.</returns>
    public async Task<bool> SetQueuePolicyForTopic(string queueArn, string topicArn, string queueUrl)
    {
        var queuePolicy = "{" +
                                "\"Version\": \"2012-10-17\"," +
                                "\"Statement\": [{" +
                                     "\"Effect\": \"Allow\"," +
                                     "\"Principal\": {" +
                                         $"\"Service\": [" +
                                             "\"sns.amazonaws.com\"," +
                                         "]" +
                                     "}," +
                                     "\"Action\": \"sqs:SendMessage\"" +
                                     $"\"Resource\": \"{queueArn}\"" +
                                      "\"Condition\": {" +
                                           "\"ArnEquals\": {" +
                                                $"\"aws:SourceArn\": \"{topicArn}\"" +
                                            "}" +
                                        "}"+
                                "}]" +
                             "}";
        var attributesResponse = await _amazonSQSClient.SetQueueAttributesAsync(
            new SetQueueAttributesRequest()
            {
                QueueUrl = queueUrl,
                Attributes = new Dictionary<string, string>(){{"Policy", queuePolicy } }
            });
        return attributesResponse.HttpStatusCode == HttpStatusCode.OK;
    }
    // snippet-end:[TopicsAndQueues.dotnetv3.SetQueueAttributes]

    // snippet-start:[TopicsAndQueues.dotnetv3.DeleteQueue]
    /// <summary>
    /// Delete a queue by its url.
    /// </summary>
    /// <param name="queueUrl">The url of the queue.</param>
    /// <returns>True if successful.</returns>
    public async Task<bool> DeleteQueueByUrl(string queueUrl)
    {
        var deleteResponse = await _amazonSQSClient.DeleteQueueAsync(
            new DeleteQueueRequest()
            {
                QueueUrl = queueUrl
            });
        return deleteResponse.HttpStatusCode == HttpStatusCode.OK;
    }
    // snippet-end:[TopicsAndQueues.dotnetv3.DeleteQueue]
}
// snippet-end:[TopicsAndQueues.dotnetv3.SQSWrapper]