# Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
# SPDX-License-Identifier: Apache-2.0

import logging
import boto3

from botocore.exceptions import ClientError

logger = logging.getLogger(__name__)


# snippet-start:[python.example_code.controltower.ControlTowerWrapper.class]
# snippet-start:[python.example_code.controltower.ControlTowerWrapper.decl]


class ControlTowerWrapper:
    """Encapsulates AWS Control Tower and Control Catalog functionality."""

    def __init__(self, controltower_client, controlcatalog_client):
        """
        :param controltower_client: A Boto3 Amazon ControlTower client.
        :param controlcatalog_client: A Boto3 Amazon ControlCatalog client.
        """
        self.controltower_client = controltower_client
        self.controlcatalog_client = controlcatalog_client

    @classmethod
    def from_client(cls):
        controltower_client = boto3.client("controltower")
        controlcatalog_client = boto3.client("controlcatalog")
        return cls(controltower_client, controlcatalog_client)

    # snippet-end:[python.example_code.controltower.ControlTowerWrapper.decl]

    # snippet-start:[python.example_code.controltower.SetupLandingZone]
    def create_landing_zone(self, manifest):
        """
        Sets up a landing zone using the provided manifest.

        :param manifest: The landing zone manifest containing configuration details.
        :return: Dictionary containing the landing zone ARN and operation ID.
        :raises ClientError: If the landing zone setup fails.

        """
        try:
            response = self.controltower_client.create_landing_zone(
                manifest=manifest,
                version='3.3'
            )
            return response
        except self.controltower_client.exceptions.AccessDeniedException:
            print("Access denied. Please ensure you have the necessary permissions.")
       # except self.controltower_client.exceptions.ValidationException:
        #    print("Deleting landing zone")
        #    raise
        except ClientError as err:
            print(err)
            logger.error(
                "Couldn't set up landing zone. Here's why: %s: %s",
                err.response["Error"]["Code"],
                err.response["Error"]["Message"]
            )
            raise

    # snippet-end:[python.example_code.controltower.SetupLandingZone]

    # snippet-start:[python.example_code.controltower.DeleteLandingZone]
    def delete_landing_zone(self, landing_zone_identifier):
        """
        Deletes a landing zone by its identifier.

        :param landing_zone_identifier: The landing zone identifier to delete.
        :raises ClientError: If the landing zone delete fails.

        """
        try:
            self.controltower_client.delete_landing_zone(
                landingZoneIdentifier=landing_zone_identifier
            )
        except self.controltower_client.exceptions.ResourceNotFoundException:
            print("Landing Zone not found.")
        except ClientError as err:
            logger.error(
                "Couldn't delete landing zone. Here's why: %s: %s",
                err.response["Error"]["Code"],
                err.response["Error"]["Message"]
            )
            raise

    # snippet-end:[python.example_code.controltower.DeleteLandingZone]

    # snippet-start:[python.example_code.controltower.ListBaselines]
    def list_baselines(self):
        """
        Lists all baselines.

        :return: List of baselines.
        :raises ClientError: If the listing operation fails.
        """
        try:
            paginator = self.controltower_client.get_paginator('list_baselines')
            baselines = []
            for page in paginator.paginate():
                baselines.extend(page['baselines'])
            return baselines

        except ClientError as err:
            logger.error(
                "Couldn't list baselines. Here's why: %s: %s",
                err.response["Error"]["Code"],
                err.response["Error"]["Message"]
            )
            raise

    # snippet-end:[python.example_code.controltower.ListBaselines]

    # snippet-start:[python.example_code.controltower.EnableBaseline]
    def enable_baseline(self, target_identifier, baseline_identifier, baseline_version):
        """
        Enables a baseline for the specified target if it's not already enabled.

        :param target_identifier: The ARN of the target.
        :param baseline_identifier: The identifier of baseline to enable.
        :param baseline_version: The version of baseline to enable.
        :return: The enabled baseline ARN or None if already enabled.
        :raises ClientError: If enabling the baseline fails for reasons other than it being already enabled.
        """
        try:
            response = self.controltower_client.enable_baseline(
                baselineIdentifier=baseline_identifier,
                baselineVersion=baseline_version,
                targetIdentifier=target_identifier
            )
            return response['arn']
        except ClientError as err:
            if err.response["Error"]["Code"] == "ValidationException" and "already enabled" in err.response["Error"]["Message"]:
                logger.info("Baseline is already enabled for this target")
                return None
            logger.error(
                "Couldn't enable baseline. Here's why: %s: %s",
                err.response["Error"]["Code"],
                err.response["Error"]["Message"]
            )
            raise
    # snippet-end:[python.example_code.controltower.EnableBaseline]

    # snippet-start:[python.example_code.controltower.ListControls]
    def list_controls(self):
        """
        Lists all controls in the Control Tower control catalog.

        :return: List of controls.
        :raises ClientError: If the listing operation fails.
        """
        try:
            paginator = self.controlcatalog_client.get_paginator('list_controls')
            controls = []
            for page in paginator.paginate():
                controls.extend(page['Controls'])
            return controls

        except ClientError as err:
            logger.error(
                "Couldn't list controls. Here's why: %s: %s",
                err.response["Error"]["Code"],
                err.response["Error"]["Message"]
            )
            raise

    # snippet-end:[python.example_code.controltower.ListControls]

    # snippet-start:[python.example_code.controltower.EnableControl]
    def enable_control(self, control_arn, target_identifier):
        """
        Enables a control for a specified target.

        :param control_arn: The ARN of the control to enable.
        :param target_identifier: The identifier of the target (e.g., OU ARN).
        :return: The operation ID.
        :raises ClientError: If enabling the control fails.
        """
        try:
            print(control_arn)
            print(target_identifier)
            response = self.controltower_client.enable_control(
                controlIdentifier=control_arn,
                targetIdentifier=target_identifier
            )
            return response['operationIdentifier']

        except ClientError as err:
            if (err.response["Error"]["Code"] == "ValidationException" and
                    "already enabled" in err.response["Error"][
                "Message"]):
                logger.info("Control is already enabled for this target")
                return None
            logger.error(
                "Couldn't enable control. Here's why: %s: %s",
                err.response["Error"]["Code"],
                err.response["Error"]["Message"]
            )
            raise

    # snippet-end:[python.example_code.controltower.EnableControl]

    # snippet-start:[python.example_code.controltower.GetControlOperation]
    def get_control_operation(self, operation_id):
        """
        Gets the status of a control operation.

        :param operation_id: The ID of the control operation.
        :return: The operation status.
        :raises ClientError: If getting the operation status fails.
        """
        try:
            response = self.controltower_client.get_control_operation(
                operationIdentifier=operation_id
            )
            return response['controlOperation']['status']
        except self.controltower_client.exceptions.ResourceNotFoundException:
            print("Control not found.")
        except ClientError as err:
            logger.error(
                "Couldn't get control operation status. Here's why: %s: %s",
                err.response["Error"]["Code"],
                err.response["Error"]["Message"]
            )
            raise

    # snippet-end:[python.example_code.controltower.GetControlOperation]

    # snippet-start:[python.example_code.controltower.DisableControl]
    def disable_control(self, control_arn, target_identifier):
        """
        Disables a control for a specified target.

        :param control_arn: The ARN of the control to disable.
        :param target_identifier: The identifier of the target (e.g., OU ARN).
        :return: The operation ID.
        :raises ClientError: If disabling the control fails.
        """
        try:
            response = self.controltower_client.disable_control(
                controlIdentifier=control_arn,
                targetIdentifier=target_identifier
            )
            return response['operationIdentifier']
        except self.controltower_client.exceptions.ResourceNotFoundException:
            print("Control not found.")
        except ClientError as err:
            logger.error(
                "Couldn't disable control. Here's why: %s: %s",
                err.response["Error"]["Code"],
                err.response["Error"]["Message"]
            )
            raise

    # snippet-end:[python.example_code.controltower.DisableControl]

    # snippet-start:[python.example_code.controltower.GetLandingZoneOperation]
    def get_landing_zone_operation(self, operation_id):
        """
        Gets the status of a landing zone operation.

        :param operation_id: The ID of the landing zone operation.
        :return: The operation status.
        :raises ClientError: If getting the operation status fails.
        """
        try:
            response = self.controltower_client.get_landing_zone_operation(
                operationIdentifier=operation_id
            )
            return response['operationDetails']['status']
        except self.controltower_client.exceptions.ResourceNotFoundException:
            print("Landing zone operation not found.")
            raise
        except ClientError as err:
            logger.error(
                "Couldn't get landing zone operation status. Here's why: %s: %s",
                err.response["Error"]["Code"],
                err.response["Error"]["Message"]
            )
            raise

# snippet-end:[python.example_code.controltower.GetLandingZoneOperation]

    # snippet-start:[python.example_code.controltower.ListLandingZones]
    def list_landing_zones(self):
        """
        Lists all landing zones.

        :return: List of landing zones.
        :raises ClientError: If the listing operation fails.
        """
        try:
            paginator = self.controltower_client.get_paginator('list_landing_zones')
            landing_zones = []
            for page in paginator.paginate():
                landing_zones.extend(page['landingZones'])
            return landing_zones

        except ClientError as err:
            logger.error(
                "Couldn't list landing zones. Here's why: %s: %s",
                err.response["Error"]["Code"],
                err.response["Error"]["Message"]
            )
            raise
    # snippet-end:[python.example_code.controltower.ListLandingZones]

# snippet-end:[python.example_code.controltower.ControlTowerWrapper.class]