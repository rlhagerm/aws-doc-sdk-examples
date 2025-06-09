# Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
# SPDX-License-Identifier: Apache-2.0

"""
Unit tests for cleanup operations in scenario_controltower.py.
"""

import pytest
from botocore.exceptions import ClientError
from botocore import waiter

class MockManager:
    def __init__(self, stub_runner, scenario_data, input_mocker):
        self.scenario_data = scenario_data
        self.stack_name = "test-stack"
        self.landing_zone_id = "arn:aws:controltower:us-east-1:123456789012:landingzone/lz-example"
        
        # Mock user inputs
        answers = [
            "y",  # Clean up landing zone
        ]
        input_mocker.mock_answers(answers)
        
        self.stub_runner = stub_runner

    def setup_stubs(self, error, stop_on, controltower_stubber, cloud_formation_stubber, monkeypatch):
        with self.stub_runner(error, stop_on) as runner:
            runner.add(
                controltower_stubber.stub_delete_landing_zone,
                self.landing_zone_id
            )
            runner.add(
                cloud_formation_stubber.stub_delete_stack,
                self.stack_name
            )
            runner.add(
                cloud_formation_stubber.stub_describe_stacks,
                self.stack_name,
                "DELETE_COMPLETE"
            )

        def mock_wait(self, **kwargs):
            return

        monkeypatch.setattr(waiter.Waiter, "wait", mock_wait)


@pytest.fixture
def mock_mgr(stub_runner, scenario_data, input_mocker):
    return MockManager(stub_runner, scenario_data, input_mocker)

@pytest.mark.integ
def test_destroy_resources(mock_mgr, capsys, monkeypatch):
    mock_mgr.setup_stubs(
        None, None, 
        mock_mgr.scenario_data.controltower_stubber,
        mock_mgr.scenario_data.cloud_formation_stubber,
        monkeypatch
    )
    
    # Create a mock stack object
    class MockStack:
        def __init__(self, name):
            self.name = name
            
        def delete(self):
            return
    
    stack = MockStack(mock_mgr.stack_name)
    
    # Test cleanup
    mock_mgr.scenario_data.scenario.destroy_resources(stack, mock_mgr.landing_zone_id)


@pytest.mark.parametrize(
    "error, stop_on_index",
    [
        ("TESTERROR-stub_delete_landing_zone", 0),
        ("TESTERROR-stub_delete_stack", 1),
        ("TESTERROR-stub_describe_stacks", 2),
    ],
)
@pytest.mark.integ
def test_destroy_resources_error(mock_mgr, caplog, error, stop_on_index, monkeypatch):
    mock_mgr.setup_stubs(
        error, stop_on_index, 
        mock_mgr.scenario_data.controltower_stubber,
        mock_mgr.scenario_data.cloud_formation_stubber,
        monkeypatch
    )
    
    # Create a mock stack object
    class MockStack:
        def __init__(self, name):
            self.name = name
            
        def delete(self):
            if stop_on_index == 1:
                raise ClientError(
                    {"Error": {"Code": error, "Message": "Test error"}},
                    "DeleteStack"
                )
            return
    
    stack = MockStack(mock_mgr.stack_name)
    
    with pytest.raises(ClientError) as exc_info:
        if stop_on_index == 0:
            mock_mgr.scenario_data.scenario.controltower_wrapper.delete_landing_zone(
                mock_mgr.landing_zone_id
            )
        else:
            mock_mgr.scenario_data.scenario.destroy_resources(stack, mock_mgr.landing_zone_id)