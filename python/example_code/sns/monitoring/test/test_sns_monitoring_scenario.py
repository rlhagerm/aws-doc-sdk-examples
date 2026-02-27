# Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
# SPDX-License-Identifier: Apache-2.0

"""
Integration tests for the CloudWatch SNS Monitoring scenario.

These tests verify that the CloudWatch and SNS wrapper classes work correctly
with the actual AWS services. Note that running these tests will incur AWS charges.
"""

import sys
import time
import uuid
import pytest

# Add parent directory to path to import wrappers
sys.path.insert(0, "..")

from cloudwatch_wrapper import MonitoringCloudWatchWrapper
from sns_wrapper import MonitoringSnsWrapper


class TestMonitoringCloudWatchWrapper:
    """Integration tests for MonitoringCloudWatchWrapper."""

    @pytest.fixture(autouse=True)
    def setup(self, cloudwatch_client, region):
        """Set up test fixtures."""
        self.wrapper = MonitoringCloudWatchWrapper(cloudwatch_client)
        self.region = region
        self.test_id = str(uuid.uuid4())[:8]
        self.created_alarms = []
        self.created_dashboards = []
        yield
        # Cleanup
        if self.created_alarms:
            try:
                self.wrapper.delete_alarms(self.created_alarms)
            except Exception:
                pass
        if self.created_dashboards:
            try:
                self.wrapper.delete_dashboards(self.created_dashboards)
            except Exception:
                pass

    def test_put_metric_data(self):
        """Test publishing metric data to CloudWatch."""
        namespace = f"TestNamespace/{self.test_id}"
        metric_name = f"TestMetric-{self.test_id}"

        # Should not raise an exception
        self.wrapper.put_metric_data(
            namespace=namespace, metric_name=metric_name, value=10.0, unit="Count"
        )

    def test_put_metric_alarm(self):
        """Test creating a CloudWatch alarm."""
        alarm_name = f"test-alarm-{self.test_id}"
        namespace = f"TestNamespace/{self.test_id}"
        metric_name = f"TestMetric-{self.test_id}"

        self.wrapper.put_metric_alarm(
            alarm_name=alarm_name,
            metric_namespace=namespace,
            metric_name=metric_name,
            threshold=10.0,
            comparison_operator="GreaterThanOrEqualToThreshold",
            evaluation_periods=1,
            period=60,
            statistic="Sum",
        )
        self.created_alarms.append(alarm_name)

        # Verify alarm was created
        alarms = self.wrapper.describe_alarms([alarm_name])
        assert len(alarms) == 1
        assert alarms[0]["AlarmName"] == alarm_name

    def test_describe_alarms(self):
        """Test describing CloudWatch alarms."""
        alarm_name = f"test-alarm-{self.test_id}"
        namespace = f"TestNamespace/{self.test_id}"
        metric_name = f"TestMetric-{self.test_id}"

        # Create an alarm first
        self.wrapper.put_metric_alarm(
            alarm_name=alarm_name,
            metric_namespace=namespace,
            metric_name=metric_name,
            threshold=10.0,
            comparison_operator="GreaterThanOrEqualToThreshold",
            evaluation_periods=1,
            period=60,
            statistic="Sum",
        )
        self.created_alarms.append(alarm_name)

        # Describe the alarm
        alarms = self.wrapper.describe_alarms([alarm_name])
        assert len(alarms) == 1
        assert alarms[0]["AlarmName"] == alarm_name
        assert alarms[0]["Namespace"] == namespace
        assert alarms[0]["MetricName"] == metric_name

    def test_describe_alarm_history(self):
        """Test describing alarm history."""
        alarm_name = f"test-alarm-{self.test_id}"
        namespace = f"TestNamespace/{self.test_id}"
        metric_name = f"TestMetric-{self.test_id}"

        # Create an alarm first
        self.wrapper.put_metric_alarm(
            alarm_name=alarm_name,
            metric_namespace=namespace,
            metric_name=metric_name,
            threshold=10.0,
            comparison_operator="GreaterThanOrEqualToThreshold",
            evaluation_periods=1,
            period=60,
            statistic="Sum",
        )
        self.created_alarms.append(alarm_name)

        # History might be empty for a new alarm, but the call should succeed
        history = self.wrapper.describe_alarm_history(alarm_name=alarm_name)
        assert isinstance(history, list)

    def test_put_dashboard(self):
        """Test creating a CloudWatch dashboard."""
        dashboard_name = f"test-dashboard-{self.test_id}"
        namespace = f"TestNamespace/{self.test_id}"
        metric_name = f"TestMetric-{self.test_id}"

        url = self.wrapper.put_dashboard(
            dashboard_name=dashboard_name,
            metric_namespace=namespace,
            metric_name=metric_name,
            region=self.region,
        )
        self.created_dashboards.append(dashboard_name)

        assert dashboard_name in url
        assert self.region in url

    def test_delete_alarms(self):
        """Test deleting CloudWatch alarms."""
        alarm_name = f"test-alarm-{self.test_id}"
        namespace = f"TestNamespace/{self.test_id}"
        metric_name = f"TestMetric-{self.test_id}"

        # Create an alarm
        self.wrapper.put_metric_alarm(
            alarm_name=alarm_name,
            metric_namespace=namespace,
            metric_name=metric_name,
            threshold=10.0,
            comparison_operator="GreaterThanOrEqualToThreshold",
            evaluation_periods=1,
            period=60,
            statistic="Sum",
        )

        # Delete the alarm
        self.wrapper.delete_alarms([alarm_name])

        # Verify alarm was deleted
        alarms = self.wrapper.describe_alarms([alarm_name])
        assert len(alarms) == 0

    def test_delete_dashboards(self):
        """Test deleting CloudWatch dashboards."""
        dashboard_name = f"test-dashboard-{self.test_id}"
        namespace = f"TestNamespace/{self.test_id}"
        metric_name = f"TestMetric-{self.test_id}"

        # Create a dashboard
        self.wrapper.put_dashboard(
            dashboard_name=dashboard_name,
            metric_namespace=namespace,
            metric_name=metric_name,
            region=self.region,
        )

        # Delete the dashboard - should not raise an exception
        self.wrapper.delete_dashboards([dashboard_name])


class TestMonitoringSnsWrapper:
    """Integration tests for MonitoringSnsWrapper."""

    @pytest.fixture(autouse=True)
    def setup(self, sns_client):
        """Set up test fixtures."""
        self.wrapper = MonitoringSnsWrapper(sns_client)
        self.test_id = str(uuid.uuid4())[:8]
        self.created_topics = []
        yield
        # Cleanup
        for topic_arn in self.created_topics:
            try:
                self.wrapper.delete_topic(topic_arn)
            except Exception:
                pass

    def test_create_topic(self):
        """Test creating an SNS topic."""
        topic_name = f"test-topic-{self.test_id}"

        topic_arn = self.wrapper.create_topic(topic_name)
        self.created_topics.append(topic_arn)

        assert topic_arn is not None
        assert topic_name in topic_arn

    def test_subscribe_email(self):
        """Test subscribing an email to an SNS topic."""
        topic_name = f"test-topic-{self.test_id}"
        test_email = "test@example.com"

        # Create topic first
        topic_arn = self.wrapper.create_topic(topic_name)
        self.created_topics.append(topic_arn)

        # Subscribe email
        subscription_arn = self.wrapper.subscribe_email(topic_arn, test_email)

        # Email subscriptions are pending until confirmed
        assert subscription_arn == "pending confirmation" or "PendingConfirmation" in str(
            subscription_arn
        ) or subscription_arn.startswith("arn:")

    def test_delete_topic(self):
        """Test deleting an SNS topic."""
        topic_name = f"test-topic-{self.test_id}"

        # Create topic
        topic_arn = self.wrapper.create_topic(topic_name)

        # Delete topic - should not raise an exception
        self.wrapper.delete_topic(topic_arn)

    def test_unsubscribe_pending(self):
        """Test unsubscribing from a pending subscription."""
        # Should handle pending confirmation gracefully
        self.wrapper.unsubscribe("PendingConfirmation")


class TestScenarioIntegration:
    """Integration tests for the full scenario workflow."""

    @pytest.fixture(autouse=True)
    def setup(self, cloudwatch_client, sns_client, region):
        """Set up test fixtures."""
        self.cloudwatch_wrapper = MonitoringCloudWatchWrapper(cloudwatch_client)
        self.sns_wrapper = MonitoringSnsWrapper(sns_client)
        self.region = region
        self.test_id = str(uuid.uuid4())[:8]
        self.created_resources = {
            "topics": [],
            "alarms": [],
            "dashboards": [],
        }
        yield
        # Cleanup all resources
        for alarm_name in self.created_resources["alarms"]:
            try:
                self.cloudwatch_wrapper.delete_alarms([alarm_name])
            except Exception:
                pass
        for dashboard_name in self.created_resources["dashboards"]:
            try:
                self.cloudwatch_wrapper.delete_dashboards([dashboard_name])
            except Exception:
                pass
        for topic_arn in self.created_resources["topics"]:
            try:
                self.sns_wrapper.delete_topic(topic_arn)
            except Exception:
                pass

    def test_full_workflow(self):
        """Test the full monitoring workflow from setup to cleanup."""
        # Configuration
        topic_name = f"test-monitoring-topic-{self.test_id}"
        alarm_name = f"test-monitoring-alarm-{self.test_id}"
        dashboard_name = f"test-monitoring-dashboard-{self.test_id}"
        namespace = f"TestApp/{self.test_id}"
        metric_name = "ErrorCount"

        # Phase 1: Setup
        # Create SNS topic
        topic_arn = self.sns_wrapper.create_topic(topic_name)
        self.created_resources["topics"].append(topic_arn)
        assert topic_arn is not None

        # Create CloudWatch alarm with SNS action
        self.cloudwatch_wrapper.put_metric_alarm(
            alarm_name=alarm_name,
            metric_namespace=namespace,
            metric_name=metric_name,
            threshold=10.0,
            comparison_operator="GreaterThanOrEqualToThreshold",
            evaluation_periods=1,
            period=60,
            statistic="Sum",
            alarm_actions=[topic_arn],
        )
        self.created_resources["alarms"].append(alarm_name)

        # Create dashboard
        dashboard_url = self.cloudwatch_wrapper.put_dashboard(
            dashboard_name=dashboard_name,
            metric_namespace=namespace,
            metric_name=metric_name,
            region=self.region,
        )
        self.created_resources["dashboards"].append(dashboard_name)
        assert dashboard_name in dashboard_url

        # Phase 2: Publish metric data
        # Publish normal data
        self.cloudwatch_wrapper.put_metric_data(
            namespace=namespace, metric_name=metric_name, value=5.0, unit="Count"
        )

        # Verify alarm exists and can be described
        alarms = self.cloudwatch_wrapper.describe_alarms([alarm_name])
        assert len(alarms) == 1
        assert alarms[0]["AlarmName"] == alarm_name

        # Publish high data to trigger alarm
        self.cloudwatch_wrapper.put_metric_data(
            namespace=namespace, metric_name=metric_name, value=15.0, unit="Count"
        )

        # Phase 3: Verify alarm history can be retrieved
        # Wait a moment for CloudWatch to process
        time.sleep(2)
        history = self.cloudwatch_wrapper.describe_alarm_history(alarm_name=alarm_name)
        assert isinstance(history, list)

        # Phase 4: Cleanup (handled by fixture)
        print(f"Workflow test completed successfully for test ID: {self.test_id}")