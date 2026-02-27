# Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
# SPDX-License-Identifier: Apache-2.0

"""
Purpose

Shows how to use AWS SDK for Python (Boto3) with Amazon CloudWatch and Amazon SNS
to create a monitoring solution that sends alerts when metric thresholds are breached.

This scenario demonstrates:
1. Creating an SNS topic for alarm notifications
2. Subscribing an email address to receive notifications
3. Creating a CloudWatch alarm that monitors a custom metric
4. Creating a CloudWatch dashboard to visualize the metric
5. Publishing metric data to trigger the alarm
6. Viewing alarm history and state changes
7. Cleaning up all created resources

This example uses the AWS SDK for Python (Boto3) to interact with CloudWatch and SNS.
"""

import json
import logging
import sys
import time
from typing import Optional

import boto3

from cloudwatch_wrapper import MonitoringCloudWatchWrapper
from sns_wrapper import MonitoringSnsWrapper

# Add relative path to include demo_tools in this code example without need for setup.
sys.path.append("../../..")
import demo_tools.question as q  # noqa

logger = logging.getLogger(__name__)

# Default configuration
DEFAULT_TOPIC_NAME = "cloudwatch-alarms-topic"
DEFAULT_DASHBOARD_NAME = "monitoring-dashboard"
DEFAULT_METRIC_NAMESPACE = "CustomApp/Monitoring"
DEFAULT_METRIC_NAME = "ErrorCount"
DEFAULT_ALARM_THRESHOLD = 10.0


# snippet-start:[python.example_code.cloudwatch.Scenario_SnsMonitoring]
class CloudWatchSnsMonitoringScenario:
    """
    Runs an interactive scenario that demonstrates CloudWatch monitoring with SNS alerts.
    """

    def __init__(
        self,
        cloudwatch_wrapper: MonitoringCloudWatchWrapper,
        sns_wrapper: MonitoringSnsWrapper,
        region: str,
    ):
        """
        :param cloudwatch_wrapper: An instance of MonitoringCloudWatchWrapper.
        :param sns_wrapper: An instance of MonitoringSnsWrapper.
        :param region: The AWS region for resources.
        """
        self.cloudwatch_wrapper = cloudwatch_wrapper
        self.sns_wrapper = sns_wrapper
        self.region = region

        # Resource tracking
        self.topic_arn: Optional[str] = None
        self.subscription_arn: Optional[str] = None
        self.alarm_name: Optional[str] = None
        self.dashboard_name: Optional[str] = None
        self.metric_namespace: str = DEFAULT_METRIC_NAMESPACE
        self.metric_name: str = DEFAULT_METRIC_NAME
        self.email_address: Optional[str] = None

    def run_scenario(self) -> None:
        """Runs the CloudWatch monitoring with SNS alerts scenario."""
        print("-" * 80)
        print("Welcome to the CloudWatch Monitoring with SNS Alerts Scenario.")
        print("-" * 80)
        print(
            """
This scenario demonstrates how to:
1. Create an SNS topic for alarm notifications
2. Create a CloudWatch alarm that monitors a custom metric
3. Publish metric data to trigger the alarm
4. Receive email notifications when the alarm state changes
"""
        )

        try:
            self._setup_phase()
            self._demonstration_phase()
            self._examination_phase()
        except Exception as e:
            logger.error("Scenario failed: %s", e)
            print(f"\nThe scenario encountered an error: {e}")
        finally:
            self._cleanup_phase()

        print("-" * 80)
        print("CloudWatch Monitoring with SNS Alerts scenario completed.")
        print("-" * 80)

    def _setup_phase(self) -> None:
        """Phase 1: Setup - Create SNS topic, subscription, CloudWatch alarm, and dashboard."""
        print("\n" + "=" * 80)
        print("Phase 1: Setup Resources")
        print("=" * 80)

        # Get email address for notifications
        self.email_address = q.ask(
            "\nEnter an email address to receive alarm notifications: ", q.non_empty
        )

        # Create SNS topic
        print("\nCreating SNS topic...")
        self.topic_arn = self.sns_wrapper.create_topic(DEFAULT_TOPIC_NAME)
        print(f"SNS topic created: {self.topic_arn}")

        # Subscribe email to topic
        self.subscription_arn = self.sns_wrapper.subscribe_email(
            self.topic_arn, self.email_address
        )
        print(
            "Email subscription created. Please check your email and confirm the subscription."
        )

        # Get alarm name from user
        self.alarm_name = q.ask(
            "\nEnter a name for the CloudWatch alarm: ", q.non_empty
        )

        # Get metric namespace (with default)
        namespace_input = input(
            f"\nEnter a name for the custom metric namespace (default: {DEFAULT_METRIC_NAMESPACE}): "
        ).strip()
        if namespace_input:
            self.metric_namespace = namespace_input

        # Create CloudWatch alarm
        print(f"\nCreating CloudWatch alarm '{self.alarm_name}'...")
        print(
            f"Alarm will trigger when {self.metric_name} >= {DEFAULT_ALARM_THRESHOLD} "
            "for 1 evaluation period (1 minute)."
        )

        self.cloudwatch_wrapper.put_metric_alarm(
            alarm_name=self.alarm_name,
            metric_namespace=self.metric_namespace,
            metric_name=self.metric_name,
            threshold=DEFAULT_ALARM_THRESHOLD,
            comparison_operator="GreaterThanOrEqualToThreshold",
            evaluation_periods=1,
            period=60,
            statistic="Sum",
            alarm_actions=[self.topic_arn],
            alarm_description=f"Alarm when {self.metric_name} exceeds {DEFAULT_ALARM_THRESHOLD}",
        )
        print("CloudWatch alarm created successfully.")

        # Create CloudWatch dashboard
        print("\nCreating CloudWatch dashboard...")
        self.dashboard_name = DEFAULT_DASHBOARD_NAME
        dashboard_url = self.cloudwatch_wrapper.put_dashboard(
            dashboard_name=self.dashboard_name,
            metric_namespace=self.metric_namespace,
            metric_name=self.metric_name,
            region=self.region,
        )
        print(f"Dashboard '{self.dashboard_name}' created successfully.")
        print(f"View at: {dashboard_url}")

        print("\n" + "-" * 80)
        print("Setup phase complete!")

    def _demonstration_phase(self) -> None:
        """Phase 2: Demonstration - Publish metric data and observe alarm state changes."""
        print("\n" + "=" * 80)
        print("Phase 2: Publish Metric Data")
        print("=" * 80)

        # Publish normal metric values (below threshold)
        normal_value = 5.0
        print(f"\nPublishing normal metric data ({self.metric_name} = {normal_value})...")
        self.cloudwatch_wrapper.put_metric_data(
            namespace=self.metric_namespace,
            metric_name=self.metric_name,
            value=normal_value,
            unit="Count",
        )
        print("Metric data published successfully.")

        # Check alarm state
        print("\nChecking alarm state...")
        self._display_alarm_state()

        # Ask if user wants to trigger the alarm
        trigger = q.ask(
            "\nWould you like to publish high metric data to trigger the alarm? (y/n): ",
            q.is_yesno,
        )

        if trigger:
            # Publish high metric values (above threshold)
            high_value = 15.0
            print(
                f"\nPublishing high metric data to trigger alarm ({self.metric_name} = {high_value})..."
            )
            self.cloudwatch_wrapper.put_metric_data(
                namespace=self.metric_namespace,
                metric_name=self.metric_name,
                value=high_value,
                unit="Count",
            )
            print("Metric data published successfully.")

            # Wait for alarm state to change
            print("\nWaiting for alarm state to change (this may take up to 1 minute)...")
            self._wait_for_alarm_state("ALARM", max_wait_seconds=90)

            print(f"\nAn email notification has been sent to {self.email_address}")
            print("Check your email for the alarm notification.")

        print("\n" + "-" * 80)
        print("Demonstration phase complete!")

    def _examination_phase(self) -> None:
        """Phase 3: Examination - View alarm history and state transitions."""
        print("\n" + "=" * 80)
        print("Phase 3: Demonstrate Alarm Actions")
        print("=" * 80)

        print("\nRetrieving alarm history...")
        history_items = self.cloudwatch_wrapper.describe_alarm_history(
            alarm_name=self.alarm_name, history_item_type="StateUpdate", max_records=10
        )

        if history_items:
            print("\nRecent alarm state changes:")
            for i, item in enumerate(history_items, 1):
                timestamp = item.get("Timestamp", "Unknown")
                summary = item.get("HistorySummary", "No summary available")
                print(f"\n{i}. {timestamp}")
                print(f"   Summary: {summary}")

                # Parse and display additional details if available
                history_data = item.get("HistoryData")
                if history_data:
                    try:
                        data = json.loads(history_data)
                        old_state = data.get("oldState", {}).get("stateValue", "Unknown")
                        new_state = data.get("newState", {}).get("stateValue", "Unknown")
                        print(f"   State changed from {old_state} to {new_state}")
                    except json.JSONDecodeError:
                        pass
        else:
            print("\nNo alarm history found yet. History may take a few minutes to appear.")

        print("\n" + "-" * 80)
        print("Examination phase complete!")

    def _cleanup_phase(self) -> None:
        """Phase 4: Cleanup - Delete all created resources."""
        print("\n" + "=" * 80)
        print("Phase 4: Cleanup")
        print("=" * 80)

        # Check if there are any resources to clean up
        if not any([self.dashboard_name, self.alarm_name, self.topic_arn]):
            print("\nNo resources to clean up.")
            return

        delete = q.ask(
            "\nDelete all resources created by this scenario? (y/n): ", q.is_yesno
        )

        if not delete:
            print("\nResources will be retained. Remember to delete them manually:")
            if self.dashboard_name:
                print(f"  - Dashboard: {self.dashboard_name}")
            if self.alarm_name:
                print(f"  - Alarm: {self.alarm_name}")
            if self.topic_arn:
                print(f"  - SNS Topic: {self.topic_arn}")
            return

        # Delete dashboard
        if self.dashboard_name:
            print("\nDeleting CloudWatch dashboard...")
            try:
                self.cloudwatch_wrapper.delete_dashboards([self.dashboard_name])
                print("Dashboard deleted successfully.")
            except Exception as e:
                print(f"Error deleting dashboard: {e}")

        # Delete alarm
        if self.alarm_name:
            print("\nDeleting CloudWatch alarm...")
            try:
                self.cloudwatch_wrapper.delete_alarms([self.alarm_name])
                print("Alarm deleted successfully.")
            except Exception as e:
                print(f"Error deleting alarm: {e}")

        # Unsubscribe from SNS topic
        if self.subscription_arn:
            print("\nUnsubscribing from SNS topic...")
            try:
                self.sns_wrapper.unsubscribe(self.subscription_arn)
                print("Subscription removed.")
            except Exception as e:
                print(f"Error unsubscribing: {e}")

        # Delete SNS topic
        if self.topic_arn:
            print("\nDeleting SNS topic...")
            try:
                self.sns_wrapper.delete_topic(self.topic_arn)
                print("SNS topic deleted successfully.")
            except Exception as e:
                print(f"Error deleting topic: {e}")

        print("\nAll resources cleaned up successfully.")

    def _display_alarm_state(self) -> None:
        """Helper method to display the current alarm state."""
        if not self.alarm_name:
            return

        alarms = self.cloudwatch_wrapper.describe_alarms([self.alarm_name])
        if alarms:
            alarm = alarms[0]
            state = alarm.get("StateValue", "Unknown")
            reason = alarm.get("StateReason", "No reason provided")
            print(f"Current alarm state: {state}")
            print(f"Alarm reason: {reason}")
        else:
            print("Alarm not found.")

    def _wait_for_alarm_state(
        self, target_state: str, max_wait_seconds: int = 120
    ) -> bool:
        """
        Waits for the alarm to reach a specific state.

        :param target_state: The target alarm state to wait for.
        :param max_wait_seconds: Maximum time to wait in seconds.
        :return: True if the target state was reached, False otherwise.
        """
        start_time = time.time()
        check_interval = 10

        while time.time() - start_time < max_wait_seconds:
            alarms = self.cloudwatch_wrapper.describe_alarms([self.alarm_name])
            if alarms:
                current_state = alarms[0].get("StateValue", "Unknown")
                print(f"  Current state: {current_state}")

                if current_state == target_state:
                    reason = alarms[0].get("StateReason", "No reason provided")
                    print(f"\nAlarm state changed to: {target_state}")
                    print(f"Alarm reason: {reason}")
                    return True

            time.sleep(check_interval)

        print(f"\nTimeout waiting for alarm state to change to {target_state}.")
        return False


# snippet-end:[python.example_code.cloudwatch.Scenario_SnsMonitoring]


def main() -> None:
    """Main entry point for the scenario."""
    logging.basicConfig(level=logging.INFO, format="%(levelname)s: %(message)s")

    # Suppress verbose boto3 logging
    logging.getLogger("botocore").setLevel(logging.WARNING)

    # Get region from boto3 session
    session = boto3.Session()
    region = session.region_name or "us-east-1"

    # Create wrapper instances
    cloudwatch_wrapper = MonitoringCloudWatchWrapper.from_client()
    sns_wrapper = MonitoringSnsWrapper.from_client()

    # Create and run scenario
    scenario = CloudWatchSnsMonitoringScenario(cloudwatch_wrapper, sns_wrapper, region)
    scenario.run_scenario()


if __name__ == "__main__":
    main()