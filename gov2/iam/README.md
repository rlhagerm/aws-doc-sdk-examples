# IAM code examples for the SDK for Go V2

## Overview

Shows how to use the AWS SDK for Go V2 to work with AWS Identity and Access Management (IAM).

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

For prerequisites, see the [README](../README.md#Prerequisites) in the `gov2` folder.


<!--custom.prerequisites.start-->
<!--custom.prerequisites.end-->

### Get started

- [Hello IAM](hello/hello.go#L4) (`ListPolicies`)


### Single actions

Code excerpts that show you how to call individual service functions.

- [Attach a policy to a role](actions/roles.go#L132) (`AttachRolePolicy`)
- [Create a policy](actions/policies.go#L65) (`CreatePolicy`)
- [Create a role](actions/roles.go#L46) (`CreateRole`)
- [Create a service-linked role](actions/roles.go#L98) (`CreateServiceLinkedRole`)
- [Create a user](actions/users.go#L74) (`CreateUser`)
- [Create an access key](actions/users.go#L175) (`CreateAccessKey`)
- [Create an inline policy for a user](actions/users.go#L92) (`PutUserPolicy`)
- [Delete a policy](actions/policies.go#L118) (`DeletePolicy`)
- [Delete a role](actions/roles.go#L200) (`DeleteRole`)
- [Delete a service-linked role](actions/roles.go#L117) (`DeleteServiceLinkedRole`)
- [Delete a user](actions/users.go#L160) (`DeleteUser`)
- [Delete an access key](actions/users.go#L193) (`DeleteAccessKey`)
- [Delete an inline policy from a user](actions/users.go#L144) (`DeleteUserPolicy`)
- [Detach a policy from a role](actions/roles.go#L166) (`DetachRolePolicy`)
- [Get a policy](actions/policies.go#L100) (`GetPolicy`)
- [Get a role](actions/roles.go#L81) (`GetRole`)
- [Get a user](actions/users.go#L47) (`GetUser`)
- [Get the account password policy](actions/account.go#L26) (`GetAccountPasswordPolicy`)
- [List SAML providers](actions/account.go#L44) (`ListSAMLProviders`)
- [List a user's access keys](actions/users.go#L209) (`ListAccessKeys`)
- [List groups](actions/groups.go#L27) (`ListGroups`)
- [List inline policies for a role](actions/roles.go#L182) (`ListRolePolicies`)
- [List inline policies for a user](actions/users.go#L126) (`ListUserPolicies`)
- [List policies](actions/policies.go#L47) (`ListPolicies`)
- [List policies attached to a role](actions/roles.go#L148) (`ListAttachedRolePolicies`)
- [List roles](actions/roles.go#L28) (`ListRoles`)
- [List users](actions/users.go#L29) (`ListUsers`)

### Scenarios

Code examples that show you how to accomplish a specific task by calling multiple
functions within the same service.

- [Create a user and assume a role](scenarios/scenario_assume_role.go)


<!--custom.examples.start-->
<!--custom.examples.end-->

## Run the examples

### Instructions


<!--custom.instructions.start-->
<!--custom.instructions.end-->

#### Hello IAM

This example shows you how to get started using IAM.

```
go run ./hello
```

#### Run a scenario

All scenarios can be run with the `cmd` runner. To get a list of scenarios
and to get help for running a scenario, use the following command:

```
go run ./cmd -h
```

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
in the `gov2` folder.



<!--custom.tests.start-->
<!--custom.tests.end-->

## Additional resources

- [IAM User Guide](https://docs.aws.amazon.com/IAM/latest/UserGuide/introduction.html)
- [IAM API Reference](https://docs.aws.amazon.com/IAM/latest/APIReference/welcome.html)
- [SDK for Go V2 IAM reference](https://pkg.go.dev/github.com/aws/aws-sdk-go-v2/service/iam)

<!--custom.resources.start-->
<!--custom.resources.end-->

---

Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.

SPDX-License-Identifier: Apache-2.0