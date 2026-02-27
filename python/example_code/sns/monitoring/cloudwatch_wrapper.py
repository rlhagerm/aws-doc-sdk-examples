# Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
# SPDX-License-Identifier: Apache-2.0

"""
Purpose

Shows how to use the AWS SDK for Python (Boto3) with Amazon CloudWatch to create
and manage custom metrics, alarms, and dashboards for the monitoring scenario.
"""

import json
import logging
from datetime import datetime
from typing import Any, Dict, List, Optional

import boto3
from botocore.exceptions import ClientError

logger = logging.getLogger(__name__)


# snippet-start:[python.example_code.cloudwatch.MonitoringCloudWatchWrapper.class]
# snippet-start:[python.example_code.cloudwatch.MonitoringCloudWatchWrapper.decl]
class MonitoringCloudWatchWrapper:
    """Encapsulates Amazon CloudWatch functions for the monitoring scenario."""

    def __init__(self, cloudwatch_client: boto3.client):
        """
        :param cloudwatch_client: A Boto3 CloudWatch client.
        """
        self.cloudwatch_client = cloudwatch_client

    @classmethod
    def from_client(cls) -> "MonitoringCloudWatchWrapper":
        """
        Creates a MonitoringCloudWatchWrapper instance with a default CloudWatch client.

        :return: An instance of MonitoringCloudWatchWrapper.
        """
        cloudwatch_client = boto3.client("cloudwatch")
        return cls(cloudwatch_client)

    # snippet-end:[python.example_code.cloudwatch.MonitoringCloudWatchWrapper.decl]

    # snippet-start:[python.example_code.cloudwatch.PutMetricAlarm_Monitoring]
    def put_metric_alarm(
        self,
        alarm_name: str,
        metric_namespace: str,
        metric_name: str,
        threshold: float,
        comparison_operator: str,
        evaluation_periods: int,
        period: int,
        statistic: str,
        alarm_actions: Optional[List[str]] = None,
        alarm_description: Optional[str] = None,
    ) -> None:
        """
        Creates or updates a CloudWatch alarm that monitors a metric.

        :param alarm_name: The name of the alarm.
        :param metric_namespace: The namespace of the metric.
        :param metric_name: The name of the metric.
        :param threshold: The threshold value for the alarm.
        :param comparison_operator: The comparison operator for the alarm
                                   (e.g., 'GreaterThanOrEqualToThreshold').
        :param evaluation_periods: The number of periods to evaluate.
        :param period: The period in seconds.
        :param statistic: The statistic to apply (e.g., 'Sum', 'Average').
        :param alarm_actions: Optional list of ARNs to notify when alarm triggers.
        :param alarm_description: Optional description for the alarm.
        :raises ClientError: If the alarm creation fails.
        """
        try:
            kwargs: Dict[str, Any] = {
                "AlarmName": alarm_name,
                "MetricName": metric_name,
                "Namespace": metric_namespace,
                "Threshold": threshold,
                "ComparisonOperator": comparison_operator,
                "EvaluationPeriods": evaluation_periods,
                "Period": period,
                "Statistic": statistic,
            }

            if alarm_actions:
                kwargs["AlarmActions"] = alarm_actions

            if alarm_description:
                kwargs["AlarmDescription"] = alarm_description

            self.cloudwatch_client.put_metric_alarm(**kwargs)
            logger.info("Created alarm %s for metric %s.", alarm_name, metric_name)
        except ClientError as err:
            if err.response["Error"]["Code"] == "LimitExceededException":
                logger.error(
                    "Couldn't create alarm %s. You've reached the alarm limit.",
                    alarm_name,
                )
            else:
                logger.error(
                    "Couldn't create alarm %s. Here's why: %s: %s",
                    alarm_name,
                    err.response["Error"]["Code"],
                    err.response["Error"]["Message"],
                )
            raise

    # snippet-end:[python.example_code.cloudwatch.PutMetricAlarm_Monitoring]

    # snippet-start:[python.example_code.cloudwatch.PutMetricData_Monitoring]
    def put_metric_data(
        self, namespace: str, metric_name: str, value: float, unit: str = "Count"
    ) -> None:
        """
        Publishes a single metric data point to CloudWatch.

        :param namespace: The namespace for the metric.
        :param metric_name: The name of the metric.
        :param value: The value of the metric.
        :param unit: The unit of the metric (default: 'Count').
        :raises ClientError: If the metric data publish fails.
        """
        try:
            self.cloudwatch_client.put_metric_data(
                Namespace=namespace,
                MetricData=[
                    {
                        "MetricName": metric_name,
                        "Value": value,
                        "Unit": unit,
                        "Timestamp": datetime.utcnow(),
                    }
                ],
            )
            logger.info(
                "Published metric data: %s.%s = %s %s",
                namespace,
                metric_name,
                value,
                unit,
            )
        except ClientError as err:
            if err.response["Error"]["Code"] == "InvalidParameterValue":
                logger.error(
                    "Couldn't publish metric data. Invalid parameter value: %s",
                    err.response["Error"]["Message"],
                )
            else:
                logger.error(
                    "Couldn't publish metric data. Here's why: %s: %s",
                    err.response["Error"]["Code"],
                    err.response["Error"]["Message"],
                )
            raise

    # snippet-end:[python.example_code.cloudwatch.PutMetricData_Monitoring]

    # snippet-start:[python.example_code.cloudwatch.DescribeAlarms_Monitoring]
    def describe_alarms(self, alarm_names: List[str]) -> List[Dict[str, Any]]:
        """
        Retrieves information about specified alarms.

        :param alarm_names: List of alarm names to describe.
        :return: List of alarm details.
        :raises ClientError: If the alarm description fails.
        """
        try:
            response = self.cloudwatch_client.describe_alarms(AlarmNames=alarm_names)
            alarms = response.get("MetricAlarms", [])
            logger.info("Retrieved %s alarm(s).", len(alarms))
            return alarms
        except ClientError as err:
            if err.response["Error"]["Code"] == "ResourceNotFoundException":
                logger.error("One or more alarms not found: %s", alarm_names)
            else:
                logger.error(
                    "Couldn't describe alarms. Here's why: %s: %s",
                    err.response["Error"]["Code"],
                    err.response["Error"]["Message"],
                )
            raise

    # snippet-end:[python.example_code.cloudwatch.DescribeAlarms_Monitoring]

    # snippet-start:[python.example_code.cloudwatch.DescribeAlarmHistory_Monitoring]
    def describe_alarm_history(
        self,
        alarm_name: str,
        history_item_type: str = "StateUpdate",
        max_records: int = 10,
    ) -> List[Dict[str, Any]]:
        """
        Retrieves history for an alarm.

        :param alarm_name: The name of the alarm.
        :param history_item_type: The type of history to retrieve
                                  (default: 'StateUpdate').
        :param max_records: Maximum number of records to retrieve.
        :return: List of alarm history items.
        :raises ClientError: If the history retrieval fails.
        """
        try:
            paginator = self.cloudwatch_client.get_paginator("describe_alarm_history")
            history_items: List[Dict[str, Any]] = []

            for page in paginator.paginate(
                AlarmName=alarm_name,
                HistoryItemType=history_item_type,
                PaginationConfig={"MaxItems": max_records},
            ):
                history_items.extend(page.get("AlarmHistoryItems", []))

            logger.info(
                "Retrieved %s history items for alarm %s.",
                len(history_items),
                alarm_name,
            )
            return history_items
        except ClientError as err:
            logger.error(
                "Couldn't get history for alarm %s. Here's why: %s: %s",
                alarm_name,
                err.response["Error"]["Code"],
                err.response["Error"]["Message"],
            )
            raise

    # snippet-end:[python.example_code.cloudwatch.DescribeAlarmHistory_Monitoring]

    # snippet-start:[python.example_code.cloudwatch.DeleteAlarms_Monitoring]
    def delete_alarms(self, alarm_names: List[str]) -> None:
        """
        Deletes specified alarms.

        :param alarm_names: List of alarm names to delete.
        :raises ClientError: If the alarm deletion fails.
        """
        try:
            self.cloudwatch_client.delete_alarms(AlarmNames=alarm_names)
            logger.info("Deleted alarm(s): %s", alarm_names)
        except ClientError as err:
            logger.error(
                "Couldn't delete alarm(s) %s. Here's why: %s: %s",
                alarm_names,
                err.response["Error"]["Code"],
                err.response["Error"]["Message"],
            )
            raise

    # snippet-end:[python.example_code.cloudwatch.DeleteAlarms_Monitoring]

    # snippet-start:[python.example_code.cloudwatch.PutDashboard_Monitoring]
    def put_dashboard(
        self, dashboard_name: str, metric_namespace: str, metric_name: str, region: str
    ) -> str:
        """
        Creates or updates a CloudWatch dashboard.

        :param dashboard_name: The name of the dashboard.
        :param metric_namespace: The namespace of the metric to display.
        :param metric_name: The name of the metric to display.
        :param region: The AWS region for the dashboard.
        :return: The dashboard URL.
        :raises ClientError: If the dashboard creation fails.
        """
        try:
            dashboard_body = {
                "widgets": [
                    {
                        "type": "metric",
                        "x": 0,
                        "y": 0,
                        "width": 12,
                        "height": 6,
                        "properties": {
                            "metrics": [[metric_namespace, metric_name]],
                            "period": 60,
                            "stat": "Sum",
                            "region": region,
                            "title": f"{metric_name} Over Time",
                        },
                    },
                    {
                        "type": "metric",
                        "x": 12,
                        "y": 0,
                        "width": 12,
                        "height": 6,
                        "properties": {
                            "metrics": [[metric_namespace, metric_name]],
                            "period": 60,
                            "stat": "Average",
                            "region": region,
                            "title": f"{metric_name} Average",
                            "view": "gauge",
                            "yAxis": {"left": {"min": 0, "max": 20}},
                        },
                    },
                ]
            }

            self.cloudwatch_client.put_dashboard(
                DashboardName=dashboard_name, DashboardBody=json.dumps(dashboard_body)
            )

            dashboard_url = (
                f"https://console.aws.amazon.com/cloudwatch/home?region={region}"
                f"#dashboards:name={dashboard_name}"
            )

            logger.info("Created dashboard %s.", dashboard_name)
            return dashboard_url
        except ClientError as err:
            logger.error(
                "Couldn't create dashboard %s. Here's why: %s: %s",
                dashboard_name,
                err.response["Error"]["Code"],
                err.response["Error"]["Message"],
            )
            raise

    # snippet-end:[python.example_code.cloudwatch.PutDashboard_Monitoring]

    # snippet-start:[python.example_code.cloudwatch.DeleteDashboards_Monitoring]
    def delete_dashboards(self, dashboard_names: List[str]) -> None:
        """
        Deletes specified dashboards.

        :param dashboard_names: List of dashboard names to delete.
        :raises ClientError: If the dashboard deletion fails.
        """
        try:
            self.cloudwatch_client.delete_dashboards(DashboardNames=dashboard_names)
            logger.info("Deleted dashboard(s): %s", dashboard_names)
        except ClientError as err:
            logger.error(
                "Couldn't delete dashboard(s) %s. Here's why: %s: %s",
                dashboard_names,
                err.response["Error"]["Code"],
                err.response["Error"]["Message"],
            )
            raise

    # snippet-end:[python.example_code.cloudwatch.DeleteDashboards_Monitoring]


# snippet-end:[python.example_code.cloudwatch.MonitoringCloudWatchWrapper.class]