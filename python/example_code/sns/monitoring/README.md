# CloudWatch Monitoring with SNS Alerts Scenario

## Overview

This example demonstrates how to use the AWS SDK for Python (Boto3) to create a CloudWatch monitoring solution that sends alerts via Amazon SNS when metric thresholds are breached.

This scenario shows how to:

1. Create an SNS topic for alarm notifications
2. Subscribe an email address to receive notifications
3. Create a CloudWatch alarm that monitors a custom metric
4. Create a CloudWatch dashboard to visualize the metric
5. Publish metric data to trigger the alarm
6. View alarm history and state changes
7. Clean up all created resources

## ⚠ Important

- Running this code might result in charges to your AWS account. For more details, see [AWS Pricing](https://aws.amazon.com/pricing/) and [Free Tier](https://aws.amazon.com/free/).
- Running the tests might result in charges to your AWS account.
- We recommend that you grant your code least privilege. At most, grant only the minimum permissions required to perform the task. For more information, see [Grant least privilege](https://docs.aws.amazon.com/IAM/latest/UserGuide/best-practices.html#grant-least-privilege).
- This code is not tested in every AWS Region. For more information, see [AWS Regional Services](https://aws.amazon.com/about-aws/global-infrastructure/regional-product-services).

## Prerequisites

For prerequisites, see the [README](../../../README.md#Prerequisites) in the `python` folder.

Install the packages required by this example:

```bash
python -m pip install -r requirements.txt
```

## Running the Scenario

Run the scenario from the command line:

```bash
python scenario_sns_monitoring.py
```

The scenario will guide you through the following phases:

1. **Setup Phase**: Create SNS topic, email subscription, CloudWatch alarm, and dashboard
2. **Demonstration Phase**: Publish metric data and observe alarm state changes
3. **Examination Phase**: View alarm history and state transitions
4. **Cleanup Phase**: Delete all created resources

## Files

- `scenario_sns_monitoring.py` - Main scenario file that orchestrates the demonstration
- `cloudwatch_wrapper.py` - Wrapper class for CloudWatch operations
- `sns_wrapper.py` - Wrapper class for SNS operations
- `requirements.txt` - Python dependencies
- `test/` - Integration tests

## API Operations Demonstrated

### Amazon SNS
- `CreateTopic` - Create an SNS topic for notifications
- `Subscribe` - Subscribe an email address to the topic
- `Unsubscribe` - Remove subscription from the topic
- `DeleteTopic` - Delete the SNS topic

### Amazon CloudWatch
- `PutMetricAlarm` - Create an alarm for the custom metric
- `PutMetricData` - Publish custom metric data
- `DescribeAlarms` - Get current alarm state
- `DescribeAlarmHistory` - View alarm state changes
- `DeleteAlarms` - Delete the alarm
- `PutDashboard` - Create a dashboard
- `DeleteDashboards` - Delete the dashboard

## Additional Resources

- [Amazon CloudWatch User Guide](https://docs.aws.amazon.com/AmazonCloudWatch/latest/monitoring/WhatIsCloudWatch.html)
- [Amazon SNS Developer Guide](https://docs.aws.amazon.com/sns/latest/dg/welcome.html)
- [Boto3 CloudWatch documentation](https://boto3.amazonaws.com/v1/documentation/api/latest/reference/services/cloudwatch.html)
- [Boto3 SNS documentation](https://boto3.amazonaws.com/v1/documentation/api/latest/reference/services/sns.html)

---

Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
SPDX-License-Identifier: Apache-2.0