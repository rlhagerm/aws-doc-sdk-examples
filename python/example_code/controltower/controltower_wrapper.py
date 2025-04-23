# Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
# SPDX-License-Identifier: Apache-2.0

import logging
import boto3

from botocore.exceptions import ClientError

logger = logging.getLogger(__name__)


# snippet-start:[python.example_code.controltower.ControlTowerWrapper.class]
# snippet-start:[python.example_code.controltower.ControlTowerWrapper.decl]


class ControlTowerWrapper:
    """Encapsulates AWS Control Tower functionality."""

    def __init__(self, control_tower_client):
        """
        :param control_tower_client: A Boto3 Amazon ControlTower client.
        """
        self.control_tower_client = control_tower_client

    @classmethod
    def from_client(cls):
        control_tower_client = boto3.client("controltower")
        return cls(control_tower_client)

    # snippet-end:[python.example_code.controltower.ControlTowerWrapper.decl]