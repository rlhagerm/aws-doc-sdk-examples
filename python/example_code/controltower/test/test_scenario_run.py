# Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
# SPDX-License-Identifier: Apache-2.0

"""
Unit tests for the run_scenario method in scenario_controltower.py.
"""

import pytest
from botocore.exceptions import ClientError
import datetime
import boto3

class MockManager:
    def __init__(self, stub_runner, scenario_data, input_mocker):
        self.scenario_data = scenario_data
        self.account_id = "123456789012"
        self.second_account_id = "210987654321"
        self.stack_name = "test-stack"
        self.org_id = "o-exampleorgid"
        self.root_id = "r-examplerootid"
        self.sandbox_ou_id = "ou-exampleouid"
        self.sandbox_ou_arn = "arn:aws:organizations::123456789012:ou/o-exampleorgid/ou-exampleouid"
        self.landing_zone_arn = "arn:aws:controltower:us-east-1:123456789012:landingzone/lz-example"
        self.operation_id = "op-1234567890abcdef01234567890abcdef"
        self.lz_operation_id = "op-1234567890abcdef01234567890abcdef"
        self.baseline_operation_id = "op-1234567890abcdef01234567890abcdef"
        self.stack_id = "arn:aws:cloudformation:us-east-1:123456789012:stack/test-stack/abcdef"
        self.baseline_arn = "arn:aws:controltower:us-east-1:123456789012:baseline/AWSControlTowerBaseline"
        self.enabled_baseline_arn = "arn:aws:controltower:us-east-1:123456789012:baseline/AWSControlTowerBaseline/enabled"
        self.control_arn = "arn:aws:controlcatalog:us-east-1:123456789012:control/aws-control-1234"
        
        self.landing_zones = [
            {
                "arn": self.landing_zone_arn
             }
        ]
        
        self.baselines = [
            {
                "name": "AWSControlTowerBaseline",
                "arn": self.baseline_arn
            }
        ]
        
        self.controls = [
            {
                "Arn": self.control_arn,
                "Name": "TestControl1",
                "Description": "Test control description",
            }
        ]
        
        self.stub_runner = stub_runner
        self.input_mocker = input_mocker

    def setup_stubs_create_new(self, error, stop_on, monkeypatch):
        """Setup stubs for the scenario that creates a new landing zone"""
        # Mock user inputs for creating a new landing zone.
        answers = [
            "n",  # Don't use existing landing zone.
            self.stack_name,  # Stack name.
            "y",  # Proceed with landing zone creation.
            self.second_account_id,  # Secondary account ID.
            "y",  # Clean up resources.
            "y",  # Delete landing zone.
        ]
        self.input_mocker.mock_answers(answers)
        
        # Mock STS get_caller_identity.
        def mock_get_caller_identity():
            return {"Account": self.account_id}
            
        monkeypatch.setattr(boto3.client("sts"), "get_caller_identity", mock_get_caller_identity)
        
        with self.stub_runner(error, stop_on) as runner:
            # List landing zones (empty)
            runner.add(
                self.scenario_data.controltower_stubber.stub_list_landing_zones,
                []
            )
            
            # Organization setup
            runner.add(
                self.scenario_data.organizations_stubber.stub_describe_organization,
                self.org_id
            )
            runner.add(
                self.scenario_data.organizations_stubber.stub_list_roots,
                [{"Id": self.root_id, "Name": "Root"}]
            )
            runner.add(
                self.scenario_data.organizations_stubber.stub_list_organizational_units_for_parent,
                self.root_id,
                [{"Id": self.sandbox_ou_id, "Name": "Sandbox", "Arn": self.sandbox_ou_arn}]
            )
            
            # Deploy stack
            runner.add(
                self.scenario_data.cloud_formation_stubber.stub_create_stack,
                self.stack_name,
                ANY,  # Template body
                ["CAPABILITY_NAMED_IAM"],
                self.stack_id,
                [{"ParameterKey": "ParentOrganizationId", "ParameterValue": self.sandbox_ou_id}]
            )
            runner.add(
                self.scenario_data.cloud_formation_stubber.stub_describe_stacks,
                self.stack_name,
                "CREATE_COMPLETE",
                []
            )
            
            # Create landing zone
            manifest = self.scenario_data.scenario.create_landing_zone_manifest(
                self.account_id, self.second_account_id
            )
            runner.add(
                self.scenario_data.controltower_stubber.stub_create_landing_zone,
                manifest,
                "3.3",
                self.landing_zone_arn,
                self.operation_id
            )
            runner.add(
                self.scenario_data.controltower_stubber.stub_get_landing_zone_operation,
                self.operation_id,
                "SUCCEEDED"
            )
            
            # List and enable baselines
            runner.add(
                self.scenario_data.controltower_stubber.stub_list_baselines,
                self.baselines
            )
            runner.add(
                self.scenario_data.controltower_stubber.stub_enable_baseline,
                self.baseline_arn,
                "4.0",
                self.sandbox_ou_arn,
                self.enabled_baseline_arn,
                self.baseline_operation_id
            )
            
            # List and enable controls
            runner.add(
                self.scenario_data.controlcatalog_stubber.stub_list_controls,
                self.controls
            )
            runner.add(
                self.scenario_data.controltower_stubber.stub_enable_control,
                self.control_arn,
                self.sandbox_ou_arn,
                self.operation_id
            )
            runner.add(
                self.scenario_data.controltower_stubber.stub_get_control_operation,
                self.operation_id,
                "SUCCEEDED"
            )
            runner.add(
                self.scenario_data.controltower_stubber.stub_disable_control,
                self.control_arn,
                self.sandbox_ou_arn,
                self.operation_id
            )
            
            # Cleanup
            runner.add(
                self.scenario_data.controltower_stubber.stub_delete_landing_zone,
                self.landing_zone_arn,
                self.lz_operation_id
            )
            runner.add(
                self.scenario_data.cloud_formation_stubber.stub_delete_stack,
                self.stack_name
            )
            runner.add(
                self.scenario_data.cloud_formation_stubber.stub_describe_stacks,
                self.stack_name,
                "DELETE_COMPLETE"
            )

    def setup_stubs_use_suggested(self, error, stop_on, monkeypatch):
        """Setup stubs for the scenario that uses the suggested landing zone"""
        # Mock user inputs for using the suggested landing zone
        answers = [
            "y",  # Use first landing zone in the list
            "y",  # Clean up resources
            "y",  # Clean up resources
        ]
        self.input_mocker.mock_answers(answers)
        
        # Mock STS get_caller_identity
        def mock_get_caller_identity():
            return {"Account": self.account_id}
            
        monkeypatch.setattr(boto3.client("sts"), "get_caller_identity", mock_get_caller_identity)
        
        with self.stub_runner(error, stop_on) as runner:
            # List landing zones
            runner.add(
                self.scenario_data.controltower_stubber.stub_list_landing_zones,
                self.landing_zones
            )
            
            # Organization setup
            runner.add(
                self.scenario_data.organizations_stubber.stub_describe_organization,
                self.org_id
            )
            runner.add(
                self.scenario_data.organizations_stubber.stub_list_roots,
                [{"Id": self.root_id, "Name": "Root"}]
            )
            runner.add(
                self.scenario_data.organizations_stubber.stub_list_organizational_units_for_parent,
                self.root_id,
                [{"Id": self.sandbox_ou_id, "Name": "Sandbox", "Arn": self.sandbox_ou_arn}]
            )
            
            # List and enable baselines
            runner.add(
                self.scenario_data.controltower_stubber.stub_list_baselines,
                self.baselines
            )
            runner.add(
                self.scenario_data.controltower_stubber.stub_enable_baseline,
                self.baseline_arn,
                "4.0",
                self.sandbox_ou_arn,
                self.enabled_baseline_arn,
                self.baseline_operation_id
            )
            
            # List and enable controls
            runner.add(
                self.scenario_data.controlcatalog_stubber.stub_list_controls,
                self.controls
            )
            runner.add(
                self.scenario_data.controltower_stubber.stub_enable_control,
                self.control_arn,
                self.sandbox_ou_arn,
                self.operation_id
            )
            runner.add(
                self.scenario_data.controltower_stubber.stub_get_control_operation,
                self.operation_id,
                "SUCCEEDED"
            )
            runner.add(
                self.scenario_data.controltower_stubber.stub_disable_control,
                self.control_arn,
                self.sandbox_ou_arn,
                self.operation_id
            )
            
            # Cleanup
            runner.add(
                self.scenario_data.controltower_stubber.stub_delete_landing_zone,
                self.landing_zone_arn,
                self.lz_operation_id
            )


@pytest.fixture
def mock_mgr(stub_runner, scenario_data, input_mocker):
    return MockManager(stub_runner, scenario_data, input_mocker)

# Define ANY constant for template body matching
ANY = object()

def test_run_scenario_create_new(mock_mgr, capsys, monkeypatch):
    """Test the scenario that creates a new landing zone"""
    mock_mgr.setup_stubs_create_new(None, None, monkeypatch)
    
    # Run the scenario
    mock_mgr.scenario_data.scenario.run_scenario()
    
    # Verify the scenario completed successfully
    captured = capsys.readouterr()
    assert "This concludes the scenario." in captured.out


def test_run_scenario_use_suggested(mock_mgr, capsys, monkeypatch):
    """Test the scenario that uses the suggested landing zone"""
    mock_mgr.setup_stubs_use_suggested(None, None, monkeypatch)
    
    # Run the scenario
    mock_mgr.scenario_data.scenario.run_scenario()
    
    # Verify the scenario completed successfully
    captured = capsys.readouterr()
    assert "This concludes the scenario." in captured.out