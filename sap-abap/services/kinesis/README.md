# Kinesis code examples for the SDK for SAP ABAP

## Overview

Shows how to use the AWS SDK for SAP ABAP to work with Amazon Kinesis.

<!--custom.overview.start-->
<!--custom.overview.end-->

_Kinesis makes it easy to collect, process, and analyze video and data streams in real time._

## ⚠ Important

* Running this code might result in charges to your AWS account. For more details, see [AWS Pricing](https://aws.amazon.com/pricing/) and [Free Tier](https://aws.amazon.com/free/).
* Running the tests might result in charges to your AWS account.
* We recommend that you grant your code least privilege. At most, grant only the minimum permissions required to perform the task. For more information, see [Grant least privilege](https://docs.aws.amazon.com/IAM/latest/UserGuide/best-practices.html#grant-least-privilege).
* This code is not tested in every AWS Region. For more information, see [AWS Regional Services](https://aws.amazon.com/about-aws/global-infrastructure/regional-product-services).

<!--custom.important.start-->
<!--custom.important.end-->

## Code examples

### Prerequisites

For prerequisites, see the [README](../../README.md#Prerequisites) in the `sap-abap` folder.


<!--custom.prerequisites.start-->
<!--custom.prerequisites.end-->

### Single actions

Code excerpts that show you how to call individual service functions.

- [Create a stream](zcl_aws1_kns_actions.clas.abap#L65) (`CreateStream`)
- [Delete a stream](zcl_aws1_kns_actions.clas.abap#L91) (`DeleteStream`)
- [Describe a stream](zcl_aws1_kns_actions.clas.abap#L114) (`DescribeStream`)
- [Get data in batches from a stream](zcl_aws1_kns_actions.clas.abap#L140) (`GetRecords`)
- [List streams](zcl_aws1_kns_actions.clas.abap#L180) (`ListStreams`)
- [Put data into a stream](zcl_aws1_kns_actions.clas.abap#L203) (`PutRecord`)
- [Register a consumer](zcl_aws1_kns_actions.clas.abap#L241) (`RegisterStreamConsumer`)

### Scenarios

Code examples that show you how to accomplish a specific task by calling multiple
functions within the same service.

- [Get started with data streams](zcl_aws1_kns_scenario.clas.abap)


<!--custom.examples.start-->
<!--custom.examples.end-->

## Run the examples

### Instructions


<!--custom.instructions.start-->
<!--custom.instructions.end-->



#### Get started with data streams

This example shows you how to do the following:

- Create a stream and put a record in it.
- Create a shard iterator.
- Read the record, then clean up resources.

<!--custom.scenario_prereqs.kinesis_Scenario_GettingStarted.start-->
<!--custom.scenario_prereqs.kinesis_Scenario_GettingStarted.end-->


<!--custom.scenarios.kinesis_Scenario_GettingStarted.start-->
<!--custom.scenarios.kinesis_Scenario_GettingStarted.end-->

### Tests

⚠ Running tests might result in charges to your AWS account.


To find instructions for running these tests, see the [README](../../README.md#Tests)
in the `sap-abap` folder.



<!--custom.tests.start-->
<!--custom.tests.end-->

## Additional resources

- [Kinesis Developer Guide](https://docs.aws.amazon.com/streams/latest/dev/introduction.html)
- [Kinesis API Reference](https://docs.aws.amazon.com/kinesis/latest/APIReference/Welcome.html)
- [SDK for SAP ABAP Kinesis reference](https://docs.aws.amazon.com/sdk-for-sap-abap/v1/api/latest/kns/index.html)

<!--custom.resources.start-->
<!--custom.resources.end-->

---

Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.

SPDX-License-Identifier: Apache-2.0