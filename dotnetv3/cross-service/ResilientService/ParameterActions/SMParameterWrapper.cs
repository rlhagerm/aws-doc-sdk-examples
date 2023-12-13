// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved. 
// SPDX-License-Identifier:  Apache-2.0

namespace ParameterActions;

public class SMParameterWrapper
{
    public void Reset()
    {
        throw new NotImplementedException();
    }

    public string Table { get; set; }
    public string FailureResponse { get; set; }

    public void PutParameter(string table, string thisIsNotATable)
    {
        throw new NotImplementedException();
    }
}