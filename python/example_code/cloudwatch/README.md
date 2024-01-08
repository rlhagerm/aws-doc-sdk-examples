# CloudWatch code examples for the SDK for Python

## Overview

Shows how to use the AWS SDK for Python (Boto3) to work with Amazon CloudWatch.

<!--custom.overview.start-->
<!--custom.overview.end-->

_CloudWatch provides a reliable, scalable, and flexible monitoring solution that you can start using within minutes._

## ⚠ Important

* Running this code might result in charges to your AWS account. For more details, see [AWS Pricing](https://aws.amazon.com/pricing/) and [Free Tier](https://aws.amazon.com/free/).
* Running the tests might result in charges to your AWS account.
* We recommend that you grant your code least privilege. At most, grant only the minimum permissions required to perform the task. For more information, see [Grant least privilege](https://docs.aws.amazon.com/IAM/latest/UserGuide/best-practices.html#grant-least-privilege).
* This code is not tested in every AWS Region. For more information, see [AWS Regional Services](https://aws.amazon.com/about-aws/global-infrastructure/regional-product-services).

<!--custom.important.start-->
<!--custom.important.end-->

## Code examples

### Prerequisites

For prerequisites, see the [README](../../README.md#Prerequisites) in the `python` folder.

Install the packages required by these examples by running the following in a virtual environment:

```
python -m pip install -r requirements.txt
```

<!--custom.prerequisites.start-->
<!--custom.prerequisites.end-->

### Single actions

Code excerpts that show you how to call individual service functions.

- [Create a metric alarm](cloudwatch_basics.py#L158) (`PutMetricAlarm`)
- [Delete alarms](cloudwatch_basics.py#L264) (`DeleteAlarms`)
- [Describe alarms for a metric](cloudwatch_basics.py#L216) (`DescribeAlarmsForMetric`)
- [Disable alarm actions](cloudwatch_basics.py#L232) (`DisableAlarmActions`)
- [Enable alarm actions](cloudwatch_basics.py#L232) (`EnableAlarmActions`)
- [Get metric statistics](cloudwatch_basics.py#L123) (`GetMetricStatistics`)
- [List metrics](cloudwatch_basics.py#L37) (`ListMetrics`)
- [Put a set of data into a metric](cloudwatch_basics.py#L88) (`PutMetricData`)
- [Put data into a metric](cloudwatch_basics.py#L64) (`PutMetricData`)

### Scenarios

Code examples that show you how to accomplish a specific task by calling multiple
functions within the same service.

- [Manage metrics and alarms](cloudwatch_basics.py)


<!--custom.examples.start-->
<!--custom.examples.end-->

## Run the examples

### Instructions


<!--custom.instructions.start-->
<!--custom.instructions.end-->



#### Manage metrics and alarms

This example shows you how to do the following:

- Create an alarm to watch a CloudWatch metric.
- Put data into a metric and trigger the alarm.
- Get data from the alarm.
- Delete the alarm.

<!--custom.scenario_prereqs.cloudwatch_Usage_MetricsAlarms.start-->
<!--custom.scenario_prereqs.cloudwatch_Usage_MetricsAlarms.end-->

Start the example by running the following at a command prompt:

```
python cloudwatch_basics.py
```


<!--custom.scenarios.cloudwatch_Usage_MetricsAlarms.start-->
<!--custom.scenarios.cloudwatch_Usage_MetricsAlarms.end-->

### Tests

⚠ Running tests might result in charges to your AWS account.


To find instructions for running these tests, see the [README](../../README.md#Tests)
in the `python` folder.



<!--custom.tests.start-->
<!--custom.tests.end-->

## Additional resources

- [CloudWatch User Guide](https://docs.aws.amazon.com/AmazonCloudWatch/latest/monitoring/WhatIsCloudWatch.html)
- [CloudWatch API Reference](https://docs.aws.amazon.com/AmazonCloudWatch/latest/APIReference/Welcome.html)
- [SDK for Python CloudWatch reference](https://boto3.amazonaws.com/v1/documentation/api/latest/reference/services/cloudwatch.html)

<!--custom.resources.start-->
<!--custom.resources.end-->

---

Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.

SPDX-License-Identifier: Apache-2.0