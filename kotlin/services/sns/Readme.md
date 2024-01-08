# Amazon SNS code examples for the SDK for Kotlin

## Overview

Shows how to use the AWS SDK for Kotlin to work with Amazon Simple Notification Service (Amazon SNS).

<!--custom.overview.start-->
<!--custom.overview.end-->

_Amazon SNS is a web service that enables applications, end-users, and devices to instantly send and receive notifications from the cloud._

## ⚠ Important

* Running this code might result in charges to your AWS account. For more details, see [AWS Pricing](https://aws.amazon.com/pricing/?aws-products-pricing.sort-by=item.additionalFields.productNameLowercase&aws-products-pricing.sort-order=asc&awsf.Free%20Tier%20Type=*all&awsf.tech-category=*all) and [Free Tier](https://aws.amazon.com/free/?all-free-tier.sort-by=item.additionalFields.SortRank&all-free-tier.sort-order=asc&awsf.Free%20Tier%20Types=*all&awsf.Free%20Tier%20Categories=*all).
* Running the tests might result in charges to your AWS account.
* We recommend that you grant your code least privilege. At most, grant only the minimum permissions required to perform the task. For more information, see [Grant least privilege](https://docs.aws.amazon.com/IAM/latest/UserGuide/best-practices.html#grant-least-privilege).
* This code is not tested in every AWS Region. For more information, see [AWS Regional Services](https://aws.amazon.com/about-aws/global-infrastructure/regional-product-services).

<!--custom.important.start-->
<!--custom.important.end-->

## Code examples

### Prerequisites

For prerequisites, see the [README](../../README.md#Prerequisites) in the `kotlin` folder.


<!--custom.prerequisites.start-->
<!--custom.prerequisites.end-->

### Get started

- [Hello Amazon SNS](src/main/kotlin/com/kotlin/sns/HelloSNS.kt#L11) (`ListTopics`)


### Single actions

Code excerpts that show you how to call individual service functions.

- [Add tags to a topic](src/main/kotlin/com/kotlin/sns/AddTags.kt#L46) (`TagResource`)
- [Create a topic](src/main/kotlin/com/kotlin/sns/CreateTopic.kt#L46) (`CreateTopic`)
- [Delete a subscription](src/main/kotlin/com/kotlin/sns/Unsubscribe.kt#L44) (`Unsubscribe`)
- [Delete a topic](src/main/kotlin/com/kotlin/sns/DeleteTopic.kt#L45) (`DeleteTopic`)
- [Get the properties of a topic](src/main/kotlin/com/kotlin/sns/GetTopicAttributes.kt#L45) (`GetTopicAttributes`)
- [List the subscribers of a topic](src/main/kotlin/com/kotlin/sns/ListSubscriptions.kt#L27) (`ListSubscriptions`)
- [List topics](src/main/kotlin/com/kotlin/sns/ListTopics.kt#L29) (`ListTopics`)
- [Publish an SMS text message](src/main/kotlin/com/kotlin/sns/PublishTextSMS.kt#L47) (`Publish`)
- [Publish to a topic](src/main/kotlin/com/kotlin/sns/PublishTopic.kt#L45) (`Publish`)
- [Set topic attributes](src/main/kotlin/com/kotlin/sns/SetTopicAttributes.kt#L48) (`SetTopicAttributes`)
- [Subscribe a Lambda function to a topic](src/main/kotlin/com/kotlin/sns/SubscribeLambda.kt#L46) (`Subscribe`)
- [Subscribe an email address to a topic](src/main/kotlin/com/kotlin/sns/SubscribeEmail.kt#L47) (`Subscribe`)


<!--custom.examples.start-->

### Custom Examples

- **DeleteTag** - Demonstrates how to delete tags from an Amazon SNS topic.
- **ListTags** - Demonstrates how to retrieve tags from an Amazon SNS topic.
- **SubscribeTextSMS** - Demonstrates how to subscribe to an Amazon SNS text endpoint.
- **Unsubscribe** - Demonstrates how to remove an Amazon SNS subscription.
<!--custom.examples.end-->

## Run the examples

### Instructions


<!--custom.instructions.start-->
<!--custom.instructions.end-->

#### Hello Amazon SNS

This example shows you how to get started using Amazon SNS.



### Tests

⚠ Running tests might result in charges to your AWS account.


To find instructions for running these tests, see the [README](../../README.md#Tests)
in the `kotlin` folder.



<!--custom.tests.start-->
<!--custom.tests.end-->

## Additional resources

- [Amazon SNS Developer Guide](https://docs.aws.amazon.com/sns/latest/dg/welcome.html)
- [Amazon SNS API Reference](https://docs.aws.amazon.com/sns/latest/api/welcome.html)
- [SDK for Kotlin Amazon SNS reference](https://sdk.amazonaws.com/kotlin/api/latest/sns/index.html)

<!--custom.resources.start-->
<!--custom.resources.end-->

---

Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.

SPDX-License-Identifier: Apache-2.0