# Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
# SPDX-License-Identifier: Apache-2.0

"""
Tests for baseline management in scenario_controltower.py.
"""

import pytest
from botocore.exceptions import ClientError

class MockManager:
    def __init__(self, stub_runner, scenario_data, input_mocker):
        self.scenario_data = scenario_data
        self.ou_arn = "arn:aws:organizations::123456789012:ou/o-exampleorgid/ou-exampleouid"
        self.baseline_arn = "arn:aws:controltower:us-east-1:123456789012:baseline/AWSControlTowerBaseline"
        self.enabled_baseline_arn = "arn:aws:controltower:us-east-1:123456789012:baseline/AWSControlTowerBaseline/enabled"
        
        self.baselines = [
            {
                "name": "AWSControlTowerBaseline",
                "arn": self.baseline_arn
            },
            {
                "name": "OtherBaseline",
                "arn": "arn:aws:controltower:us-east-1:123456789012:baseline/OtherBaseline"
            }
        ]
        
        self.stub_runner = stub_runner

    def setup_stubs(self, error, stop_on, controltower_stubber, monkeypatch):
        with self.stub_runner(error, stop_on) as runner:
            runner.add(
                controltower_stubber.stub_list_baselines,
                self.baselines
            )
            runner.add(
                controltower_stubber.stub_enable_baseline,
                self.baseline_arn,
                "4.0",
                self.ou_arn,
                self.enabled_baseline_arn
            )


@pytest.fixture
def mock_mgr(stub_runner, scenario_data, input_mocker):
    return MockManager(stub_runner, scenario_data, input_mocker)

@pytest.mark.integ
def test_baseline_management(mock_mgr, capsys, monkeypatch):
    mock_mgr.setup_stubs(None, None, mock_mgr.scenario_data.controltower_stubber, monkeypatch)
    
    # Set required attributes for the test
    mock_mgr.scenario_data.scenario.ou_arn = mock_mgr.ou_arn
    mock_mgr.scenario_data.scenario.use_landing_zone = True
    
    # Test listing baselines
    baselines = mock_mgr.scenario_data.scenario.controltower_wrapper.list_baselines()
    assert len(baselines) == 2
    assert baselines[0]["name"] == "AWSControlTowerBaseline"
    
    # Test enabling baseline
    baseline_arn = mock_mgr.scenario_data.scenario.controltower_wrapper.enable_baseline(
        mock_mgr.ou_arn,
        mock_mgr.baseline_arn,
        "4.0"
    )
    assert baseline_arn == mock_mgr.enabled_baseline_arn


@pytest.mark.parametrize(
    "error, stop_on_index",
    [
        ("TESTERROR-stub_list_baselines", 0),
        ("TESTERROR-stub_enable_baseline", 1),
    ],
)
@pytest.mark.integ
def test_baseline_management_error(mock_mgr, caplog, error, stop_on_index, monkeypatch):
    mock_mgr.setup_stubs(error, stop_on_index, mock_mgr.scenario_data.controltower_stubber, monkeypatch)
    
    # Set required attributes for the test
    mock_mgr.scenario_data.scenario.ou_arn = mock_mgr.ou_arn
    
    with pytest.raises(ClientError) as exc_info:
        if stop_on_index == 0:
            mock_mgr.scenario_data.scenario.controltower_wrapper.list_baselines()
        elif stop_on_index == 1:
            mock_mgr.scenario_data.scenario.controltower_wrapper.enable_baseline(
                mock_mgr.ou_arn,
                mock_mgr.baseline_arn,
                "4.0"
            )