# IAM code examples for the SDK for JavaScript (v3)

## Overview

Shows how to use the AWS SDK for JavaScript (v3) to work with AWS Identity and Access Management (IAM).

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

For prerequisites, see the [README](../../README.md#Prerequisites) in the `javascriptv3` folder.


<!--custom.prerequisites.start-->
<!--custom.prerequisites.end-->

### Get started

- [Hello IAM](hello.js#L8) (`ListPolicies`)


### Single actions

Code excerpts that show you how to call individual service functions.

- [Attach a policy to a role](actions/attach-role-policy.js#L8) (`AttachRolePolicy`)
- [Attach an inline policy to a role](actions/put-role-policy.js#L8) (`PutRolePolicy`)
- [Create a SAML provider](actions/create-saml-provider.js#L8) (`CreateSAMLProvider`)
- [Create a group](actions/create-group.js#L8) (`CreateGroup`)
- [Create a policy](actions/create-policy.js#L8) (`CreatePolicy`)
- [Create a role](actions/create-role.js#L8) (`CreateRole`)
- [Create a service-linked role](actions/create-service-linked-role.js#L8) (`CreateServiceLinkedRole`)
- [Create a user](actions/create-user.js#L8) (`CreateUser`)
- [Create an access key](actions/create-access-key.js#L8) (`CreateAccessKey`)
- [Create an alias for an account](actions/create-account-alias.js#L8) (`CreateAccountAlias`)
- [Create an instance profile](../cross-services/wkflw-resilient-service/steps-demo.js#L452) (`CreateInstanceProfile`)
- [Delete SAML provider](actions/delete-saml-provider.js#L8) (`DeleteSAMLProvider`)
- [Delete a group](actions/delete-group.js#L8) (`DeleteGroup`)
- [Delete a policy](actions/delete-policy.js#L8) (`DeletePolicy`)
- [Delete a role](actions/delete-role.js#L8) (`DeleteRole`)
- [Delete a role policy](actions/delete-role-policy.js#L8) (`DeleteRolePolicy`)
- [Delete a server certificate](actions/delete-server-certificate.js#L8) (`DeleteServerCertificate`)
- [Delete a service-linked role](actions/delete-service-linked-role.js#L8) (`DeleteServiceLinkedRole`)
- [Delete a user](actions/delete-user.js#L8) (`DeleteUser`)
- [Delete an access key](actions/delete-access-key.js#L8) (`DeleteAccessKey`)
- [Delete an account alias](actions/delete-account-alias.js#L8) (`DeleteAccountAlias`)
- [Delete an instance profile](../cross-services/wkflw-resilient-service/steps-destroy.js#L215) (`DeleteInstanceProfile`)
- [Detach a policy from a role](actions/detach-role-policy.js#L8) (`DetachRolePolicy`)
- [Get a policy](actions/get-policy.js#L8) (`GetPolicy`)
- [Get a role](actions/get-role.js#L8) (`GetRole`)
- [Get a server certificate](actions/get-server-certificate.js#L8) (`GetServerCertificate`)
- [Get a service-linked role's deletion status](actions/get-service-linked-role-deletion-status.js#L8) (`GetServiceLinkedRoleDeletionStatus`)
- [Get data about the last use of an access key](actions/get-access-key-last-used.js#L8) (`GetAccessKeyLastUsed`)
- [Get the account password policy](actions/get-account-password-policy.js#L8) (`GetAccountPasswordPolicy`)
- [List SAML providers](actions/list-saml-providers.js#L8) (`ListSAMLProviders`)
- [List a user's access keys](actions/list-access-keys.js#L8) (`ListAccessKeys`)
- [List account aliases](actions/list-account-aliases.js#L8) (`ListAccountAliases`)
- [List groups](actions/list-groups.js#L8) (`ListGroups`)
- [List inline policies for a role](actions/list-role-policies.js#L8) (`ListRolePolicies`)
- [List policies](actions/list-policies.js#L8) (`ListPolicies`)
- [List policies attached to a role](actions/list-attached-role-policies.js#L8) (`ListAttachedRolePolicies`)
- [List roles](actions/list-roles.js#L8) (`ListRoles`)
- [List server certificates](actions/list-server-certificates.js#L8) (`ListServerCertificates`)
- [List users](actions/list-users.js#L8) (`ListUsers`)
- [Update a server certificate](actions/update-server-certificate.js#L8) (`UpdateServerCertificate`)
- [Update a user](actions/update-user.js#L8) (`UpdateUser`)
- [Update an access key](actions/update-access-key.js#L8) (`UpdateAccessKey`)
- [Upload a server certificate](actions/upload-server-certificate.js#L8) (`UploadServerCertificate`)

### Scenarios

Code examples that show you how to accomplish a specific task by calling multiple
functions within the same service.

- [Build and manage a resilient service](javascriptv3/example_code/cross-services/wkflw-resilient-service/index.js)
- [Create a user and assume a role](scenarios/basic.js)


<!--custom.examples.start-->
<!--custom.examples.end-->

## Run the examples

### Instructions

**Note**: All code examples are written in ECMAscript 6 (ES6). For guidelines on converting to CommonJS, see
[JavaScript ES6/CommonJS syntax](https://docs.aws.amazon.com/sdk-for-javascript/v3/developer-guide/sdk-examples-javascript-syntax.html).

**Run a single action**

```bash
node ./actions/<fileName>
```

**Run a scenario**
Most scenarios can be run with the following command:
```bash
node ./scenarios/<fileName>
```

<!--custom.instructions.start-->
<!--custom.instructions.end-->

#### Hello IAM

This example shows you how to get started using IAM.

```bash
node ./hello.js
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


<!--custom.scenarios.cross_ResilientService.start-->
<!--custom.scenarios.cross_ResilientService.end-->

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


To find instructions for running these tests, see the [README](../../README.md#Tests)
in the `javascriptv3` folder.



<!--custom.tests.start-->
<!--custom.tests.end-->

## Additional resources

- [IAM User Guide](https://docs.aws.amazon.com/IAM/latest/UserGuide/introduction.html)
- [IAM API Reference](https://docs.aws.amazon.com/IAM/latest/APIReference/welcome.html)
- [SDK for JavaScript (v3) IAM reference](https://docs.aws.amazon.com/AWSJavaScriptSDK/v3/latest/client/iam)

<!--custom.resources.start-->
<!--custom.resources.end-->

---

Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.

SPDX-License-Identifier: Apache-2.0