# Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
# SPDX-License-Identifier: Apache-2.0

import pytest
import subprocess
import sys

files_under_test = [
    # Text models
    "hello_bedrock.py"
]


@pytest.mark.integ
@pytest.mark.parametrize("file", files_under_test)
def test_hello_bedrock(file):
    result = subprocess.run(
        [sys.executable, file],
        capture_output=True,
        text=True,
    )
    assert result.stdout != ""
    assert result.returncode == 0
