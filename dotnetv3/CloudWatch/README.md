# CloudWatch code examples for the SDK for .NET

## Overview

Shows how to use the AWS SDK for .NET to work with Amazon CloudWatch.

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

For prerequisites, see the [README](../README.md#Prerequisites) in the `dotnetv3` folder.


<!--custom.prerequisites.start-->
To enable billing metrics and statistics for these examples, make sure to
[enable billing alerts](https://docs.aws.amazon.com/AmazonCloudWatch/latest/monitoring/monitor_estimated_charges_with_cloudwatch.html#turning_on_billing_metrics) for your account.
<!--custom.prerequisites.end-->

### Get started

- [Hello CloudWatch](Actions/HelloCloudWatch.cs#L4) (`ListMetrics`)


### Single actions

Code excerpts that show you how to call individual service functions.

- [Create a dashboard](Actions/CloudWatchWrapper.cs#L91) (`PutDashboard`)
- [Create a metric alarm](Actions/CloudWatchWrapper.cs#L265) (`PutMetricAlarm`)
- [Create an anomaly detector](Actions/CloudWatchWrapper.cs#L450) (`PutAnomalyDetector`)
- [Delete alarms](Actions/CloudWatchWrapper.cs#L396) (`DeleteAlarms`)
- [Delete an anomaly detector](Actions/CloudWatchWrapper.cs#L494) (`DeleteAnomalyDetector`)
- [Delete dashboards](Actions/CloudWatchWrapper.cs#L512) (`DeleteDashboards`)
- [Describe alarm history](Actions/CloudWatchWrapper.cs#L369) (`DescribeAlarmHistory`)
- [Describe alarms](Actions/CloudWatchWrapper.cs#L326) (`DescribeAlarms`)
- [Describe alarms for a metric](Actions/CloudWatchWrapper.cs#L349) (`DescribeAlarmsForMetric`)
- [Describe anomaly detectors](Actions/CloudWatchWrapper.cs#L468) (`DescribeAnomalyDetectors`)
- [Disable alarm actions](Actions/CloudWatchWrapper.cs#L414) (`DisableAlarmActions`)
- [Enable alarm actions](Actions/CloudWatchWrapper.cs#L432) (`EnableAlarmActions`)
- [Get a metric data image](Actions/CloudWatchWrapper.cs#L175) (`GetMetricWidgetImage`)
- [Get dashboard details](Actions/CloudWatchWrapper.cs#L115) (`GetDashboard`)
- [Get metric data](Actions/CloudWatchWrapper.cs#L226) (`GetMetricData`)
- [Get metric statistics](Actions/CloudWatchWrapper.cs#L61) (`GetMetricStatistics`)
- [List dashboards](Actions/CloudWatchWrapper.cs#L134) (`ListDashboards`)
- [List metrics](Actions/CloudWatchWrapper.cs#L33) (`ListMetrics`)
- [Put data into a metric](Actions/CloudWatchWrapper.cs#L154) (`PutMetricData`)

### Scenarios

Code examples that show you how to accomplish a specific task by calling multiple
functions within the same service.

- [Get started with metrics, dashboards, and alarms](Scenarios/CloudWatchScenario.cs)


<!--custom.examples.start-->
<!--custom.examples.end-->

## Run the examples

### Instructions

For general instructions to run the examples, see the
[README](../README.md#building-and-running-the-code-examples) in the `dotnetv3` folder.

Some projects might include a settings.json file. Before compiling the project,
you can change these values to match your own account and resources. Alternatively,
add a settings.local.json file with your local settings, which will be loaded automatically
when the application runs.

After the example compiles, you can run it from the command line. To do so, navigate to
the folder that contains the .csproj file and run the following command:

```
dotnet run
```

Alternatively, you can run the example from within your IDE.


<!--custom.instructions.start-->
<!--custom.instructions.end-->

#### Hello CloudWatch

This example shows you how to get started using CloudWatch.



#### Get started with metrics, dashboards, and alarms

This example shows you how to do the following:

- List CloudWatch namespaces and metrics.
- Get statistics for a metric and for estimated billing.
- Create and update a dashboard.
- Create and add data to a metric.
- Create and trigger an alarm, then view alarm history.
- Add an anomaly detector.
- Get a metric image, then clean up resources.

<!--custom.scenario_prereqs.cloudwatch_GetStartedMetricsDashboardsAlarms.start-->
<!--custom.scenario_prereqs.cloudwatch_GetStartedMetricsDashboardsAlarms.end-->


<!--custom.scenarios.cloudwatch_GetStartedMetricsDashboardsAlarms.start-->
<!--custom.scenarios.cloudwatch_GetStartedMetricsDashboardsAlarms.end-->

### Tests

⚠ Running tests might result in charges to your AWS account.


To find instructions for running these tests, see the [README](../README.md#Tests)
in the `dotnetv3` folder.



<!--custom.tests.start-->
<!--custom.tests.end-->

## Additional resources

- [CloudWatch User Guide](https://docs.aws.amazon.com/AmazonCloudWatch/latest/monitoring/WhatIsCloudWatch.html)
- [CloudWatch API Reference](https://docs.aws.amazon.com/AmazonCloudWatch/latest/APIReference/Welcome.html)
- [SDK for .NET CloudWatch reference](https://docs.aws.amazon.com/sdkfornet/v3/apidocs/items/CloudWatch/NCloudWatch.html)

<!--custom.resources.start-->
<!--custom.resources.end-->

---

Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.

SPDX-License-Identifier: Apache-2.0