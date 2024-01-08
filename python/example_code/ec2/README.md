# Amazon EC2 code examples for the SDK for Python

## Overview

Shows how to use the AWS SDK for Python (Boto3) to work with Amazon Elastic Compute Cloud (Amazon EC2).

<!--custom.overview.start-->
<!--custom.overview.end-->

_Amazon EC2 is a web service that provides resizable computing capacity—literally, servers in Amazon's data centers—that you use to build and host your software systems._

## ⚠ Important

* Running this code might result in charges to your AWS account. For more details, see [AWS Pricing](https://aws.amazon.com/pricing/) and [Free Tier](https://aws.amazon.com/free/).
* Running the tests might result in charges to your AWS account.
* We recommend that you grant your code least privilege. At most, grant only the minimum permissions required to perform the task. For more information, see [Grant least privilege](https://docs.aws.amazon.com/IAM/latest/UserGuide/best-practices.html#grant-least-privilege).
* This code is not tested in every AWS Region. For more information, see [AWS Regional Services](https://aws.amazon.com/about-aws/global-infrastructure/regional-product-services).

<!--custom.important.start-->
<!--custom.important.end-->

## Code examples

### Prerequisites

For prerequisites, see the [README](../../README.md#Prerequisites) in the `python` folder.

Install the packages required by these examples by running the following in a virtual environment:

```
python -m pip install -r requirements.txt
```

<!--custom.prerequisites.start-->
<!--custom.prerequisites.end-->

### Get started

- [Hello Amazon EC2](hello.py#L4) (`DescribeSecurityGroups`)


### Single actions

Code excerpts that show you how to call individual service functions.

- [Allocate an Elastic IP address](elastic_ip.py#L34) (`AllocateAddress`)
- [Associate an Elastic IP address with an instance](elastic_ip.py#L59) (`AssociateAddress`)
- [Create a launch template](../../cross_service/resilient_service/auto_scaler.py#L346) (`CreateLaunchTemplate`)
- [Create a security group](security_group.py#L35) (`CreateSecurityGroup`)
- [Create a security key pair](key_pair.py#L40) (`CreateKeyPair`)
- [Create and run an instance](instance.py#L34) (`RunInstances`)
- [Delete a launch template](../../cross_service/resilient_service/auto_scaler.py#L409) (`DeleteLaunchTemplate`)
- [Delete a security group](security_group.py#L129) (`DeleteSecurityGroup`)
- [Delete a security key pair](key_pair.py#L91) (`DeleteKeyPair`)
- [Describe Availability Zones](../../cross_service/resilient_service/auto_scaler.py#L438) (`DescribeAvailabilityZones`)
- [Describe instances](instance.py#L85) (`DescribeInstances`)
- [Disassociate an Elastic IP address from an instance](elastic_ip.py#L89) (`DisassociateAddress`)
- [Get data about Amazon Machine Images](instance.py#L195) (`DescribeImages`)
- [Get data about a security group](security_group.py#L102) (`DescribeSecurityGroups`)
- [Get data about instance types](instance.py#L217) (`DescribeInstanceTypes`)
- [Get data about the instance profile associated with an instance](../../cross_service/resilient_service/auto_scaler.py#L184) (`DescribeIamInstanceProfileAssociations`)
- [Get the default VPC](../../cross_service/resilient_service/auto_scaler.py#L630) (`DescribeVpcs`)
- [Get the default subnets for a VPC](../../cross_service/resilient_service/auto_scaler.py#L731) (`DescribeSubnets`)
- [List security key pairs](key_pair.py#L70) (`DescribeKeyPairs`)
- [Reboot an instance](../../cross_service/resilient_service/auto_scaler.py#L21) (`RebootInstances`)
- [Release an Elastic IP address](elastic_ip.py#L112) (`ReleaseAddress`)
- [Replace the instance profile associated with an instance](../../cross_service/resilient_service/auto_scaler.py#L205) (`ReplaceIamInstanceProfileAssociation`)
- [Set inbound rules for a security group](security_group.py#L62) (`AuthorizeSecurityGroupIngress`)
- [Start an instance](instance.py#L141) (`StartInstances`)
- [Stop an instance](instance.py#L168) (`StopInstances`)
- [Terminate an instance](instance.py#L116) (`TerminateInstances`)

### Scenarios

Code examples that show you how to accomplish a specific task by calling multiple
functions within the same service.

- [Build and manage a resilient service](../../cross_service/resilient_service/runner.py)
- [Get started with instances](scenario_get_started_instances.py)


<!--custom.examples.start-->
<!--custom.examples.end-->

## Run the examples

### Instructions


<!--custom.instructions.start-->
<!--custom.instructions.end-->

#### Hello Amazon EC2

This example shows you how to get started using Amazon EC2.

```
python hello.py
```


#### Build and manage a resilient service

This example shows you how to create a load-balanced web service that returns book, movie, and song recommendations. The example shows how the service responds to failures, and how to restructure the service for more resilience when failures occur.

- Use an Amazon EC2 Auto Scaling group to create Amazon Elastic Compute Cloud (Amazon EC2) instances based on a launch template and to keep the number of instances in a specified range.
- Handle and distribute HTTP requests with Elastic Load Balancing.
- Monitor the health of instances in an Auto Scaling group and forward requests only to healthy instances.
- Run a Python web server on each EC2 instance to handle HTTP requests. The web server responds with recommendations and health checks.
- Simulate a recommendation service with an Amazon DynamoDB table.
- Control web server response to requests and health checks by updating AWS Systems Manager parameters.

<!--custom.scenario_prereqs.cross_ResilientService.start-->
<!--custom.scenario_prereqs.cross_ResilientService.end-->

Start the example by running the following at a command prompt:

```
python ../../cross_service/resilient_service/runner.py
```


<!--custom.scenarios.cross_ResilientService.start-->
Complete details and instructions on how to run this example can be found in the
[README](../../cross_service/resilient_service/README.md) for the example.
<!--custom.scenarios.cross_ResilientService.end-->

#### Get started with instances

This example shows you how to do the following:

- Create a key pair and security group.
- Select an Amazon Machine Image (AMI) and compatible instance type, then create an instance.
- Stop and restart the instance.
- Associate an Elastic IP address with your instance.
- Connect to your instance with SSH, then clean up resources.

<!--custom.scenario_prereqs.ec2_Scenario_GetStartedInstances.start-->
<!--custom.scenario_prereqs.ec2_Scenario_GetStartedInstances.end-->

Start the example by running the following at a command prompt:

```
python scenario_get_started_instances.py
```


<!--custom.scenarios.ec2_Scenario_GetStartedInstances.start-->
<!--custom.scenarios.ec2_Scenario_GetStartedInstances.end-->

### Tests

⚠ Running tests might result in charges to your AWS account.


To find instructions for running these tests, see the [README](../../README.md#Tests)
in the `python` folder.



<!--custom.tests.start-->
<!--custom.tests.end-->

## Additional resources

- [Amazon EC2 User Guide](https://docs.aws.amazon.com/AWSEC2/latest/UserGuide/concepts.html)
- [Amazon EC2 API Reference](https://docs.aws.amazon.com/AWSEC2/latest/APIReference/Welcome.html)
- [SDK for Python Amazon EC2 reference](https://boto3.amazonaws.com/v1/documentation/api/latest/reference/services/ec2.html)

<!--custom.resources.start-->
<!--custom.resources.end-->

---

Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.

SPDX-License-Identifier: Apache-2.0