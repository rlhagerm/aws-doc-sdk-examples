# Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
# SPDX-License-Identifier: Apache-2.0

"""
Unit tests for setup_organization in scenario_controltower.py.
"""

import pytest
from botocore.exceptions import ClientError
from botocore import waiter

class MockManager:
    def __init__(self, stub_runner, scenario_data, input_mocker):
        self.scenario_data = scenario_data
        self.org_id = "o-exampleorgid"
        self.root_id = "r-examplerootid"
        self.sandbox_ou_id = "ou-exampleouid"
        self.sandbox_ou_arn = "arn:aws:organizations::123456789012:ou/o-exampleorgid/ou-exampleouid"
        
        self.stub_runner = stub_runner

    def setup_stubs(self, error, stop_on, organizations_stubber, monkeypatch):
        with self.stub_runner(error, stop_on) as runner:
            runner.add(organizations_stubber.stub_describe_organization, self.org_id)
            runner.add(organizations_stubber.stub_list_roots, [{"Id": self.root_id, "Name": "Root"}])
            runner.add(organizations_stubber.stub_list_organizational_units_for_parent, 
                      self.root_id, [{"Id": self.sandbox_ou_id, "Name": "Sandbox", "Arn": self.sandbox_ou_arn}])

        def mock_wait(self, **kwargs):
            return

        monkeypatch.setattr(waiter.Waiter, "wait", mock_wait)


@pytest.fixture
def mock_mgr(stub_runner, scenario_data, input_mocker):
    return MockManager(stub_runner, scenario_data, input_mocker)

@pytest.mark.integ
def test_setup_organization(mock_mgr, capsys, monkeypatch):
    mock_mgr.setup_stubs(None, None, mock_mgr.scenario_data.organizations_stubber, monkeypatch)

    sandbox_ou_id = mock_mgr.scenario_data.scenario.setup_organization()
    assert sandbox_ou_id == mock_mgr.sandbox_ou_id
    assert mock_mgr.scenario_data.scenario.ou_arn == mock_mgr.sandbox_ou_arn


@pytest.mark.parametrize(
    "error, stop_on_index",
    [
        ("TESTERROR-stub_describe_organization", 0),
        ("TESTERROR-stub_list_roots", 1),
        ("TESTERROR-stub_list_organizational_units_for_parent", 2),
    ],
)
@pytest.mark.integ
def test_setup_organization_error(mock_mgr, caplog, error, stop_on_index, monkeypatch):
    mock_mgr.setup_stubs(error, stop_on_index, mock_mgr.scenario_data.organizations_stubber, monkeypatch)

    with pytest.raises(ClientError) as exc_info:
        mock_mgr.scenario_data.scenario.setup_organization()