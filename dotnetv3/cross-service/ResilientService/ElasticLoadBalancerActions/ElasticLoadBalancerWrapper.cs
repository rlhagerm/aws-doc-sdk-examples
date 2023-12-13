// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved. 
// SPDX-License-Identifier:  Apache-2.0

namespace ElasticLoadBalancerActions;

public class ElasticLoadBalancerWrapper
{
    public string GetEndpoint()
    {
        throw new NotImplementedException();
    }

    public string GetEndPointResponse()
    {
        throw new NotImplementedException();
    }

    public object[] CheckTargetHealth()
    {
        throw new NotImplementedException();
    }

    public object CreateTargetGroup(string protocol, int port, object vpcId)
    {
        throw new NotImplementedException();
    }

    public void CreateLoadBalancer(object select, object targetGroup)
    {
        throw new NotImplementedException();
    }

    public void AttachLoadBalancer(object targetGroup)
    {
        throw new NotImplementedException();
    }

    public bool VerifyLoadBalancerEndpoint()
    {
        throw new NotImplementedException();
    }

    public void DeleteLoadBalancer()
    {
        throw new NotImplementedException();
    }

    public void DeleteTargetGroup()
    {
        throw new NotImplementedException();
    }
}