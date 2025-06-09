# Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
# SPDX-License-Identifier: Apache-2.0

"""
Unit tests for landing zone operations in scenario_controltower.py.
"""

import pytest
from botocore.exceptions import ClientError
import datetime

class MockManager:
    def __init__(self, stub_runner, scenario_data, input_mocker):
        self.scenario_data = scenario_data
        self.account_id = "123456789012"
        self.second_account_id = "210987654321"
        self.landing_zone_arn = "arn:aws:controltower:us-east-1:123456789012:landingzone/lz-example"
        self.operation_id = "op-1234567890abcdef0"
        
        # Mock user inputs
        answers = [
            "y",  # Proceed with landing zone creation
            self.second_account_id,  # Secondary account ID
        ]
        input_mocker.mock_answers(answers)
        
        self.stub_runner = stub_runner
        
        # Create manifest for testing
        self.manifest = self.scenario_data.scenario.create_landing_zone_manifest(
            self.account_id, self.second_account_id
        )

    def setup_stubs(self, error, stop_on, controltower_stubber, monkeypatch):
        with self.stub_runner(error, stop_on) as runner:
            runner.add(
                controltower_stubber.stub_create_landing_zone,
                self.manifest,
                "3.3",
                self.landing_zone_arn,
                self.operation_id
            )
            runner.add(
                controltower_stubber.stub_get_landing_zone_operation,
                self.operation_id,
                "SUCCEEDED"
            )
            runner.add(
                controltower_stubber.stub_list_landing_zones,
                [{"arn": self.landing_zone_arn, "name": "TestLandingZone"}]
            )
        
        # Mock sleep to avoid waiting in tests
        def mock_sleep(seconds):
            return
            
        monkeypatch.setattr(datetime.time, "sleep", mock_sleep)


@pytest.fixture
def mock_mgr(stub_runner, scenario_data, input_mocker):
    return MockManager(stub_runner, scenario_data, input_mocker)

@pytest.mark.integ
def test_landing_zone_operations(mock_mgr, capsys, monkeypatch):
    mock_mgr.setup_stubs(None, None, mock_mgr.scenario_data.controltower_stubber, monkeypatch)
    
    # Set required attributes for the test
    mock_mgr.scenario_data.scenario.account_id = mock_mgr.account_id
    mock_mgr.scenario_data.scenario.ou_id = "ou-example"
    
    # Test listing landing zones
    landing_zones = mock_mgr.scenario_data.scenario.controltower_wrapper.list_landing_zones()
    assert len(landing_zones) == 1
    assert landing_zones[0]["arn"] == mock_mgr.landing_zone_arn


@pytest.mark.parametrize(
    "error, stop_on_index",
    [
        ("TESTERROR-stub_create_landing_zone", 0),
        ("TESTERROR-stub_get_landing_zone_operation", 1),
        ("TESTERROR-stub_list_landing_zones", 2),
    ],
)
@pytest.mark.integ
def test_landing_zone_operations_error(mock_mgr, caplog, error, stop_on_index, monkeypatch):
    mock_mgr.setup_stubs(error, stop_on_index, mock_mgr.scenario_data.controltower_stubber, monkeypatch)
    
    # Set required attributes for the test
    mock_mgr.scenario_data.scenario.account_id = mock_mgr.account_id
    
    with pytest.raises(ClientError) as exc_info:
        if stop_on_index == 0:
            mock_mgr.scenario_data.scenario.controltower_wrapper.create_landing_zone(mock_mgr.manifest)
        elif stop_on_index == 1:
            mock_mgr.scenario_data.scenario.controltower_wrapper.get_landing_zone_operation(mock_mgr.operation_id)
        elif stop_on_index == 2:
            mock_mgr.scenario_data.scenario.controltower_wrapper.list_landing_zones()