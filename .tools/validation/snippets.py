# Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
# SPDX-License-Identifier: Apache-2.0

from dataclasses import dataclass


@dataclass
class Snippet:
    id: str
    file: str
    line_start: int
    line_end: int
