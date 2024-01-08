# EventBridge code examples for the SDK for Java 2.x

## Overview

Shows how to use the AWS SDK for Java 2.x to work with Amazon EventBridge.

<!--custom.overview.start-->
<!--custom.overview.end-->

_EventBridge is a serverless event bus service that makes it easy to connect your applications with data from a variety of sources._

## ⚠ Important

* Running this code might result in charges to your AWS account. For more details, see [AWS Pricing](https://aws.amazon.com/pricing/?aws-products-pricing.sort-by=item.additionalFields.productNameLowercase&aws-products-pricing.sort-order=asc&awsf.Free%20Tier%20Type=*all&awsf.tech-category=*all) and [Free Tier](https://aws.amazon.com/free/?all-free-tier.sort-by=item.additionalFields.SortRank&all-free-tier.sort-order=asc&awsf.Free%20Tier%20Types=*all&awsf.Free%20Tier%20Categories=*all).
* Running the tests might result in charges to your AWS account.
* We recommend that you grant your code least privilege. At most, grant only the minimum permissions required to perform the task. For more information, see [Grant least privilege](https://docs.aws.amazon.com/IAM/latest/UserGuide/best-practices.html#grant-least-privilege).
* This code is not tested in every AWS Region. For more information, see [AWS Regional Services](https://aws.amazon.com/about-aws/global-infrastructure/regional-product-services).

<!--custom.important.start-->
<!--custom.important.end-->

## Code examples

### Prerequisites

For prerequisites, see the [README](../../README.md#Prerequisites) in the `javav2` folder.


<!--custom.prerequisites.start-->
<!--custom.prerequisites.end-->

### Get started

- [Hello EventBridge](src/main/java/com/example/eventbridge/HelloEventBridge.java#L22) (`ListEventBuses`)


### Single actions

Code excerpts that show you how to call individual service functions.

- [Add a target](src/main/java/com/example/eventbridge/EventbridgeMVP.java#L601) (`PutTargets`)
- [Create a rule](src/main/java/com/example/eventbridge/CreateRuleSchedule.java#L66) (`PutRule`)
- [Delete a rule](src/main/java/com/example/eventbridge/EventbridgeMVP.java#L371) (`DeleteRule`)
- [Describe a rule](src/main/java/com/example/eventbridge/EventbridgeMVP.java#L505) (`DescribeRule`)
- [Disable a rule](src/main/java/com/example/eventbridge/EventbridgeMVP.java#L522) (`DisableRule`)
- [Enable a rule](src/main/java/com/example/eventbridge/EventbridgeMVP.java#L522) (`EnableRule`)
- [List rule names for a target](src/main/java/com/example/eventbridge/EventbridgeMVP.java#L573) (`ListRuleNamesByTarget`)
- [List rules](src/main/java/com/example/eventbridge/EventbridgeMVP.java#L641) (`ListRules`)
- [List targets for a rule](src/main/java/com/example/eventbridge/EventbridgeMVP.java#L587) (`ListTargetsByRule`)
- [Remove targets from a rule](src/main/java/com/example/eventbridge/EventbridgeMVP.java#L382) (`RemoveTargets`)
- [Send events](src/main/java/com/example/eventbridge/EventbridgeMVP.java#L405) (`PutEvents`)

### Scenarios

Code examples that show you how to accomplish a specific task by calling multiple
functions within the same service.

- [Get started with rules and targets](src/main/java/com/example/eventbridge/EventbridgeMVP.java)


<!--custom.examples.start-->
<!--custom.examples.end-->

## Run the examples

### Instructions


<!--custom.instructions.start-->
<!--custom.instructions.end-->

#### Hello EventBridge

This example shows you how to get started using EventBridge.



#### Get started with rules and targets

This example shows you how to do the following:

- Create a rule and add a target to it.
- Enable and disable rules.
- List and update rules and targets.
- Send events, then clean up resources.

<!--custom.scenario_prereqs.eventbridge_Scenario_GettingStarted.start-->
<!--custom.scenario_prereqs.eventbridge_Scenario_GettingStarted.end-->


<!--custom.scenarios.eventbridge_Scenario_GettingStarted.start-->
<!--custom.scenarios.eventbridge_Scenario_GettingStarted.end-->

### Tests

⚠ Running tests might result in charges to your AWS account.


To find instructions for running these tests, see the [README](../../README.md#Tests)
in the `javav2` folder.



<!--custom.tests.start-->
<!--custom.tests.end-->

## Additional resources

- [EventBridge User Guide](https://docs.aws.amazon.com/eventbridge/latest/userguide/eb-what-is.html)
- [EventBridge API Reference](https://docs.aws.amazon.com/eventbridge/latest/APIReference/Welcome.html)
- [SDK for Java 2.x EventBridge reference](https://sdk.amazonaws.com/java/api/latest/software/amazon/awssdk/services/eventbridge/package-summary.html)

<!--custom.resources.start-->
<!--custom.resources.end-->

---

Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.

SPDX-License-Identifier: Apache-2.0