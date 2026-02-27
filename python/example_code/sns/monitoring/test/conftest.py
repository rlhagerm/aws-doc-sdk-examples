# Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
# SPDX-License-Identifier: Apache-2.0

"""
Pytest configuration and fixtures for the CloudWatch SNS Monitoring scenario tests.
"""

import sys
import pytest
import boto3


# Add parent directory to path to import wrappers
sys.path.insert(0, "..")


@pytest.fixture(scope="module")
def cloudwatch_client():
    """Provides a CloudWatch client for testing."""
    return boto3.client("cloudwatch")


@pytest.fixture(scope="module")
def sns_client():
    """Provides an SNS client for testing."""
    return boto3.client("sns")


@pytest.fixture(scope="module")
def region():
    """Provides the AWS region for testing."""
    session = boto3.Session()
    return session.region_name or "us-east-1"