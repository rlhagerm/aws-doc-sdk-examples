#!/bin/bash
# Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
# SPDX-License-Identifier: Apache-2.0

echo "Running tests..."
dotnet vstest --logger:trx/LogFileName=dotnetv4_results.trx '@/dotnetv4/testsettings.txt'
dotnet new tool-manifest
dotnet tool install DotnetWeathertopJsonReporter --local
dotnet tool run DotnetWeathertopJsonReporter -t TestResults/dotnet4_results.trx
echo "Test run complete."
