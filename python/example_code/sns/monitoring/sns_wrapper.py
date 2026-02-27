# Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
# SPDX-License-Identifier: Apache-2.0

"""
Purpose

Shows how to use the AWS SDK for Python (Boto3) with Amazon SNS to create
topics and subscriptions for the monitoring scenario.
"""

import logging
from typing import Optional

import boto3
from botocore.exceptions import ClientError

logger = logging.getLogger(__name__)


# snippet-start:[python.example_code.sns.MonitoringSnsWrapper.class]
# snippet-start:[python.example_code.sns.MonitoringSnsWrapper.decl]
class MonitoringSnsWrapper:
    """Encapsulates Amazon SNS functions for the monitoring scenario."""

    def __init__(self, sns_client: boto3.client):
        """
        :param sns_client: A Boto3 SNS client.
        """
        self.sns_client = sns_client

    @classmethod
    def from_client(cls) -> "MonitoringSnsWrapper":
        """
        Creates a MonitoringSnsWrapper instance with a default SNS client.

        :return: An instance of MonitoringSnsWrapper.
        """
        sns_client = boto3.client("sns")
        return cls(sns_client)

    # snippet-end:[python.example_code.sns.MonitoringSnsWrapper.decl]

    # snippet-start:[python.example_code.sns.CreateTopic_Monitoring]
    def create_topic(self, topic_name: str) -> str:
        """
        Creates an SNS topic.

        :param topic_name: The name of the topic to create.
        :return: The ARN of the created topic.
        :raises ClientError: If the topic creation fails.
        """
        try:
            response = self.sns_client.create_topic(Name=topic_name)
            topic_arn = response["TopicArn"]
            logger.info("Created topic %s with ARN %s.", topic_name, topic_arn)
            return topic_arn
        except ClientError as err:
            if err.response["Error"]["Code"] == "InvalidParameter":
                logger.error(
                    "Couldn't create topic %s. Invalid topic name format.", topic_name
                )
            else:
                logger.error(
                    "Couldn't create topic %s. Here's why: %s: %s",
                    topic_name,
                    err.response["Error"]["Code"],
                    err.response["Error"]["Message"],
                )
            raise

    # snippet-end:[python.example_code.sns.CreateTopic_Monitoring]

    # snippet-start:[python.example_code.sns.Subscribe_Monitoring]
    def subscribe_email(self, topic_arn: str, email_address: str) -> str:
        """
        Subscribes an email address to an SNS topic.

        :param topic_arn: The ARN of the topic to subscribe to.
        :param email_address: The email address to subscribe.
        :return: The subscription ARN (may be 'PendingConfirmation' until confirmed).
        :raises ClientError: If the subscription fails.
        """
        try:
            response = self.sns_client.subscribe(
                TopicArn=topic_arn,
                Protocol="email",
                Endpoint=email_address,
                ReturnSubscriptionArn=True,
            )
            subscription_arn = response["SubscriptionArn"]
            logger.info(
                "Subscribed %s to topic %s. Subscription ARN: %s",
                email_address,
                topic_arn,
                subscription_arn,
            )
            return subscription_arn
        except ClientError as err:
            if err.response["Error"]["Code"] == "InvalidParameter":
                logger.error(
                    "Couldn't subscribe %s to topic. Invalid email format.",
                    email_address,
                )
            else:
                logger.error(
                    "Couldn't subscribe %s to topic %s. Here's why: %s: %s",
                    email_address,
                    topic_arn,
                    err.response["Error"]["Code"],
                    err.response["Error"]["Message"],
                )
            raise

    # snippet-end:[python.example_code.sns.Subscribe_Monitoring]

    # snippet-start:[python.example_code.sns.Unsubscribe_Monitoring]
    def unsubscribe(self, subscription_arn: str) -> None:
        """
        Unsubscribes from an SNS topic.

        :param subscription_arn: The ARN of the subscription to remove.
        :raises ClientError: If the unsubscription fails.
        """
        try:
            # Skip if subscription is still pending confirmation
            if subscription_arn == "PendingConfirmation":
                logger.info("Subscription is pending confirmation, skipping unsubscribe.")
                return

            self.sns_client.unsubscribe(SubscriptionArn=subscription_arn)
            logger.info("Unsubscribed from %s.", subscription_arn)
        except ClientError as err:
            logger.error(
                "Couldn't unsubscribe from %s. Here's why: %s: %s",
                subscription_arn,
                err.response["Error"]["Code"],
                err.response["Error"]["Message"],
            )
            raise

    # snippet-end:[python.example_code.sns.Unsubscribe_Monitoring]

    # snippet-start:[python.example_code.sns.DeleteTopic_Monitoring]
    def delete_topic(self, topic_arn: str) -> None:
        """
        Deletes an SNS topic and all its subscriptions.

        :param topic_arn: The ARN of the topic to delete.
        :raises ClientError: If the topic deletion fails.
        """
        try:
            self.sns_client.delete_topic(TopicArn=topic_arn)
            logger.info("Deleted topic %s.", topic_arn)
        except ClientError as err:
            logger.error(
                "Couldn't delete topic %s. Here's why: %s: %s",
                topic_arn,
                err.response["Error"]["Code"],
                err.response["Error"]["Message"],
            )
            raise

    # snippet-end:[python.example_code.sns.DeleteTopic_Monitoring]

    # snippet-start:[python.example_code.sns.GetSubscriptionAttributes_Monitoring]
    def get_subscription_status(self, subscription_arn: str) -> Optional[str]:
        """
        Gets the status of a subscription.

        :param subscription_arn: The ARN of the subscription.
        :return: The subscription status or None if pending.
        :raises ClientError: If the attribute retrieval fails.
        """
        try:
            if subscription_arn == "PendingConfirmation":
                return "PendingConfirmation"

            response = self.sns_client.get_subscription_attributes(
                SubscriptionArn=subscription_arn
            )
            return response.get("Attributes", {}).get("PendingConfirmation", "false")
        except ClientError as err:
            logger.error(
                "Couldn't get subscription status for %s. Here's why: %s: %s",
                subscription_arn,
                err.response["Error"]["Code"],
                err.response["Error"]["Message"],
            )
            raise

    # snippet-end:[python.example_code.sns.GetSubscriptionAttributes_Monitoring]


# snippet-end:[python.example_code.sns.MonitoringSnsWrapper.class]