# IAM code examples for the SDK for .NET

## Overview

Shows how to use the AWS SDK for .NET to work with AWS Identity and Access Management (IAM).

<!--custom.overview.start-->
<!--custom.overview.end-->

_IAM is a web service for securely controlling access to AWS services. With IAM, you can centrally manage permissions in your AWS account._

## ⚠ Important

* Running this code might result in charges to your AWS account. For more details, see [AWS Pricing](https://aws.amazon.com/pricing/) and [Free Tier](https://aws.amazon.com/free/).
* Running the tests might result in charges to your AWS account.
* We recommend that you grant your code least privilege. At most, grant only the minimum permissions required to perform the task. For more information, see [Grant least privilege](https://docs.aws.amazon.com/IAM/latest/UserGuide/best-practices.html#grant-least-privilege).
* This code is not tested in every AWS Region. For more information, see [AWS Regional Services](https://aws.amazon.com/about-aws/global-infrastructure/regional-product-services).

<!--custom.important.start-->
<!--custom.important.end-->

## Code examples

### Prerequisites

For prerequisites, see the [README](../README.md#Prerequisites) in the `dotnetv3` folder.


<!--custom.prerequisites.start-->
<!--custom.prerequisites.end-->

### Get started

- [Hello IAM](Actions/HelloIAM.cs#L4) (`ListPolicies`)


### Single actions

Code excerpts that show you how to call individual service functions.

- [Add a user to a group](Actions/IAMWrapper.cs#L22) (`AddUserToGroup`)
- [Attach a policy to a role](Actions/IAMWrapper.cs#L42) (`AttachRolePolicy`)
- [Attach an inline policy to a role](Actions/IAMWrapper.cs#L571) (`PutRolePolicy`)
- [Create a group](Actions/IAMWrapper.cs#L82) (`CreateGroup`)
- [Create a policy](Actions/IAMWrapper.cs#L96) (`CreatePolicy`)
- [Create a role](Actions/IAMWrapper.cs#L116) (`CreateRole`)
- [Create a service-linked role](Actions/IAMWrapper.cs#L138) (`CreateServiceLinkedRole`)
- [Create a user](Actions/IAMWrapper.cs#L159) (`CreateUser`)
- [Create an access key](Actions/IAMWrapper.cs#L62) (`CreateAccessKey`)
- [Create an inline policy for a group](Actions/IAMWrapper.cs#L548) (`PutGroupPolicy`)
- [Delete a group](Actions/IAMWrapper.cs#L194) (`DeleteGroup`)
- [Delete a group policy](Actions/IAMWrapper.cs#L208) (`DeleteGroupPolicy`)
- [Delete a policy](Actions/IAMWrapper.cs#L230) (`DeletePolicy`)
- [Delete a role](Actions/IAMWrapper.cs#L245) (`DeleteRole`)
- [Delete a role policy](Actions/IAMWrapper.cs#L259) (`DeleteRolePolicy`)
- [Delete a user](Actions/IAMWrapper.cs#L279) (`DeleteUser`)
- [Delete an access key](Actions/IAMWrapper.cs#L173) (`DeleteAccessKey`)
- [Delete an inline policy from a user](Actions/IAMWrapper.cs#L294) (`DeleteUserPolicy`)
- [Detach a policy from a role](Actions/IAMWrapper.cs#L310) (`DetachRolePolicy`)
- [Get a policy](Actions/IAMWrapper.cs#L343) (`GetPolicy`)
- [Get a role](Actions/IAMWrapper.cs#L358) (`GetRole`)
- [Get a user](Actions/IAMWrapper.cs#L377) (`GetUser`)
- [Get the account password policy](Actions/IAMWrapper.cs#L330) (`GetAccountPasswordPolicy`)
- [List SAML providers](Actions/IAMWrapper.cs#L493) (`ListSAMLProviders`)
- [List groups](Actions/IAMWrapper.cs#L412) (`ListGroups`)
- [List inline policies for a role](Actions/IAMWrapper.cs#L452) (`ListRolePolicies`)
- [List policies](Actions/IAMWrapper.cs#L432) (`ListPolicies`)
- [List policies attached to a role](Actions/IAMWrapper.cs#L391) (`ListAttachedRolePolicies`)
- [List roles](Actions/IAMWrapper.cs#L473) (`ListRoles`)
- [List users](Actions/IAMWrapper.cs#L506) (`ListUsers`)
- [Remove a user from a group](Actions/IAMWrapper.cs#L526) (`RemoveUserFromGroup`)

### Scenarios

Code examples that show you how to accomplish a specific task by calling multiple
functions within the same service.

- [Create a group and add a user](Scenarios/IamScenariosCommon/UIWrapper.cs)
- [Create a user and assume a role](Scenarios/IamScenariosCommon/UIWrapper.cs)


<!--custom.examples.start-->
<!--custom.examples.end-->

## Run the examples

### Instructions

For general instructions to run the examples, see the
[README](../README.md#building-and-running-the-code-examples) in the `dotnetv3` folder.

Some projects might include a settings.json file. Before compiling the project,
you can change these values to match your own account and resources. Alternatively,
add a settings.local.json file with your local settings, which will be loaded automatically
when the application runs.

After the example compiles, you can run it from the command line. To do so, navigate to
the folder that contains the .csproj file and run the following command:

```
dotnet run
```

Alternatively, you can run the example from within your IDE.


<!--custom.instructions.start-->
To run the examples, see the [README](../README.md#building-and-running-the-code-examples) in the `dotnetv3` folder.
<!--custom.instructions.end-->

#### Hello IAM

This example shows you how to get started using IAM.



#### Create a group and add a user

This example shows you how to do the following:

- Create a group and grant full Amazon S3 access permissions to it.
- Create a new user with no permissions to access Amazon S3.
- Add the user to the group and show that they now have permissions for Amazon S3, then clean up resources.

<!--custom.scenario_prereqs.iam_Scenario_GroupBasics.start-->
<!--custom.scenario_prereqs.iam_Scenario_GroupBasics.end-->


<!--custom.scenarios.iam_Scenario_GroupBasics.start-->
<!--custom.scenarios.iam_Scenario_GroupBasics.end-->

#### Create a user and assume a role

This example shows you how to create a user and assume a role. 

- Create a user with no permissions.
- Create a role that grants permission to list Amazon S3 buckets for the account.
- Add a policy to let the user assume the role.
- Assume the role and list S3 buckets using temporary credentials, then clean up resources.

<!--custom.scenario_prereqs.iam_Scenario_CreateUserAssumeRole.start-->
<!--custom.scenario_prereqs.iam_Scenario_CreateUserAssumeRole.end-->


<!--custom.scenarios.iam_Scenario_CreateUserAssumeRole.start-->
<!--custom.scenarios.iam_Scenario_CreateUserAssumeRole.end-->

### Tests

⚠ Running tests might result in charges to your AWS account.


To find instructions for running these tests, see the [README](../README.md#Tests)
in the `dotnetv3` folder.



<!--custom.tests.start-->
<!--custom.tests.end-->

## Additional resources

- [IAM User Guide](https://docs.aws.amazon.com/IAM/latest/UserGuide/introduction.html)
- [IAM API Reference](https://docs.aws.amazon.com/IAM/latest/APIReference/welcome.html)
- [SDK for .NET IAM reference](https://docs.aws.amazon.com/sdkfornet/v3/apidocs/items/IAM/NIAM.html)

<!--custom.resources.start-->
<!--custom.resources.end-->

---

Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.

SPDX-License-Identifier: Apache-2.0