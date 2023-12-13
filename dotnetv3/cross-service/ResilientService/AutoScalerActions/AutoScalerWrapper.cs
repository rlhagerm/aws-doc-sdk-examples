// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved. 
// SPDX-License-Identifier:  Apache-2.0

namespace AutoScalerActions;

public class AutoScalerWrapper
{
    public void CreateTemplate(string startupScriptPath, string instancePolicyPath)
    {
        throw new NotImplementedException();
    }

    public async Task createGroup(int i)
    {
        throw new NotImplementedException();
    }

    public object GetDefaultVpc()
    {
        throw new NotImplementedException();
    }

    public object GetSubnets(object defaultVpc)
    {
        throw new NotImplementedException();
    }

    public void DeleteGroup()
    {
        throw new NotImplementedException();
    }

    public void DeleteKeyPair()
    {
        throw new NotImplementedException();
    }

    public void DeleteTemplate()
    {
        throw new NotImplementedException();
    }

    public void DeleteInstanceProfile(object badCredsProfileName, object badCredsRoleName)
    {
        throw new NotImplementedException();
    }

    public object BadCredsProfileName { get; set; }
    public object BadCredsRoleName { get; set; }
    public object BadCredsPolicyName { get; set; }

    public void CreateInstanceProfile(string ssmOnlyPolicy, object badCredsPolicyName, object badCredsRoleName, object badCredsProfileName, List<string> list)
    {
        throw new NotImplementedException();
    }

    public object[] GetInstances()
    {
        throw new NotImplementedException();
    }

    public object GetInstanceProfile(object badInstanceId)
    {
        throw new NotImplementedException();
    }

    public void ReplaceInstanceProfile(object badInstanceId, object badCredsProfileName, object o)
    {
        throw new NotImplementedException();
    }

    public void TerminateInstance(object badInstanceId)
    {
        throw new NotImplementedException();
    }
}