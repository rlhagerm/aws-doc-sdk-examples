# Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
# SPDX-License-Identifier: Apache-2.0

"""
Unit tests for control management in scenario_controltower.py.
"""

import pytest
from botocore.exceptions import ClientError
import datetime

class MockManager:
    def __init__(self, stub_runner, scenario_data, input_mocker):
        self.scenario_data = scenario_data
        self.ou_arn = "arn:aws:organizations::123456789012:ou/o-exampleorgid/ou-exampleouid"
        self.control_arn = "arn:aws:controlcatalog:us-east-1:123456789012:control/aws-control-1234"
        self.operation_id = "op-1234567890abcdef0"
        
        self.controls = [
            {
                "Arn": self.control_arn,
                "Name": "TestControl1"
            },
            {
                "Arn": "arn:aws:controlcatalog:us-east-1:123456789012:control/aws-control-5678",
                "Name": "TestControl2"
            }
        ]
        
        self.stub_runner = stub_runner

    def setup_stubs(self, error, stop_on, controlcatalog_stubber, controltower_stubber, monkeypatch):
        with self.stub_runner(error, stop_on) as runner:
            runner.add(
                controlcatalog_stubber.stub_list_controls,
                self.controls
            )
            runner.add(
                controltower_stubber.stub_enable_control,
                self.control_arn,
                self.ou_arn,
                self.operation_id
            )
            runner.add(
                controltower_stubber.stub_get_control_operation,
                self.operation_id,
                "SUCCEEDED"
            )
            runner.add(
                controltower_stubber.stub_disable_control,
                self.control_arn,
                self.ou_arn,
                self.operation_id
            )
        
        # Mock sleep to avoid waiting in tests
        def mock_sleep(seconds):
            return
            
        monkeypatch.setattr(datetime.time, "sleep", mock_sleep)


@pytest.fixture
def mock_mgr(stub_runner, scenario_data, input_mocker):
    return MockManager(stub_runner, scenario_data, input_mocker)

@pytest.mark.integ
def test_control_management(mock_mgr, capsys, monkeypatch):
    mock_mgr.setup_stubs(
        None, None, 
        mock_mgr.scenario_data.controlcatalog_stubber,
        mock_mgr.scenario_data.controltower_stubber,
        monkeypatch
    )
    
    # Set required attributes for the test
    mock_mgr.scenario_data.scenario.ou_arn = mock_mgr.ou_arn
    mock_mgr.scenario_data.scenario.use_landing_zone = True
    
    # Test listing controls
    controls = mock_mgr.scenario_data.scenario.controltower_wrapper.list_controls()
    assert len(controls) == 2
    assert controls[0]["Arn"] == mock_mgr.control_arn
    
    # Test enabling control
    operation_id = mock_mgr.scenario_data.scenario.controltower_wrapper.enable_control(
        mock_mgr.control_arn,
        mock_mgr.ou_arn
    )
    assert operation_id == mock_mgr.operation_id
    
    # Test getting control operation status
    status = mock_mgr.scenario_data.scenario.controltower_wrapper.get_control_operation(
        mock_mgr.operation_id
    )
    assert status == "SUCCEEDED"
    
    # Test disabling control
    operation_id = mock_mgr.scenario_data.scenario.controltower_wrapper.disable_control(
        mock_mgr.control_arn,
        mock_mgr.ou_arn
    )
    assert operation_id == mock_mgr.operation_id


@pytest.mark.parametrize(
    "error, stop_on_index",
    [
        ("TESTERROR-stub_list_controls", 0),
        ("TESTERROR-stub_enable_control", 1),
        ("TESTERROR-stub_get_control_operation", 2),
        ("TESTERROR-stub_disable_control", 3),
    ],
)
@pytest.mark.integ
def test_control_management_error(mock_mgr, caplog, error, stop_on_index, monkeypatch):
    mock_mgr.setup_stubs(
        error, stop_on_index, 
        mock_mgr.scenario_data.controlcatalog_stubber,
        mock_mgr.scenario_data.controltower_stubber,
        monkeypatch
    )
    
    # Set required attributes for the test
    mock_mgr.scenario_data.scenario.ou_arn = mock_mgr.ou_arn
    
    with pytest.raises(ClientError) as exc_info:
        if stop_on_index == 0:
            mock_mgr.scenario_data.scenario.controltower_wrapper.list_controls()
        elif stop_on_index == 1:
            mock_mgr.scenario_data.scenario.controltower_wrapper.enable_control(
                mock_mgr.control_arn,
                mock_mgr.ou_arn
            )
        elif stop_on_index == 2:
            mock_mgr.scenario_data.scenario.controltower_wrapper.get_control_operation(
                mock_mgr.operation_id
            )
        elif stop_on_index == 3:
            mock_mgr.scenario_data.scenario.controltower_wrapper.disable_control(
                mock_mgr.control_arn,
                mock_mgr.ou_arn
            )