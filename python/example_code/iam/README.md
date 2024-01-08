# IAM code examples for the SDK for Python

## Overview

Shows how to use the AWS SDK for Python (Boto3) to work with AWS Identity and Access Management (IAM).

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

For prerequisites, see the [README](../../README.md#Prerequisites) in the `python` folder.

Install the packages required by these examples by running the following in a virtual environment:

```
python -m pip install -r requirements.txt
```

<!--custom.prerequisites.start-->
<!--custom.prerequisites.end-->

### Single actions

Code excerpts that show you how to call individual service functions.

- [Attach a policy to a role](policy_wrapper.py#L221) (`AttachRolePolicy`)
- [Attach a policy to a user](user_wrapper.py#L107) (`AttachUserPolicy`)
- [Create a policy](policy_wrapper.py#L25) (`CreatePolicy`)
- [Create a policy version](policy_wrapper.py#L79) (`CreatePolicyVersion`)
- [Create a role](role_wrapper.py#L23) (`CreateRole`)
- [Create a service-linked role](service_linked_roles.py#L23) (`CreateServiceLinkedRole`)
- [Create a user](user_wrapper.py#L25) (`CreateUser`)
- [Create an access key](access_key_wrapper.py#L21) (`CreateAccessKey`)
- [Create an alias for an account](account_wrapper.py#L23) (`CreateAccountAlias`)
- [Create an instance profile](../../cross_service/resilient_service/auto_scaler.py#L86) (`CreateInstanceProfile`)
- [Delete a policy](policy_wrapper.py#L61) (`DeletePolicy`)
- [Delete a role](role_wrapper.py#L102) (`DeleteRole`)
- [Delete a user](user_wrapper.py#L46) (`DeleteUser`)
- [Delete an access key](access_key_wrapper.py#L47) (`DeleteAccessKey`)
- [Delete an account alias](account_wrapper.py#L44) (`DeleteAccountAlias`)
- [Delete an instance profile](../../cross_service/resilient_service/auto_scaler.py#L259) (`DeleteInstanceProfile`)
- [Detach a policy from a role](policy_wrapper.py#L240) (`DetachRolePolicy`)
- [Detach a policy from a user](user_wrapper.py#L126) (`DetachUserPolicy`)
- [Generate a credential report](account_wrapper.py#L131) (`GenerateCredentialReport`)
- [Get a credential report](account_wrapper.py#L155) (`GetCredentialReport`)
- [Get a detailed authorization report for your account](account_wrapper.py#L86) (`GetAccountAuthorizationDetails`)
- [Get a policy](policy_wrapper.py#L139) (`GetPolicy`)
- [Get a policy version](policy_wrapper.py#L140) (`GetPolicyVersion`)
- [Get a role](role_wrapper.py#L59) (`GetRole`)
- [Get a summary of account usage](account_wrapper.py#L111) (`GetAccountSummary`)
- [Get data about the last use of an access key](access_key_wrapper.py#L68) (`GetAccessKeyLastUsed`)
- [Get the account password policy](account_wrapper.py#L175) (`GetAccountPasswordPolicy`)
- [List SAML providers](account_wrapper.py#L213) (`ListSAMLProviders`)
- [List a user's access keys](access_key_wrapper.py#L97) (`ListAccessKeys`)
- [List account aliases](account_wrapper.py#L62) (`ListAccountAliases`)
- [List groups](group_wrapper.py#L21) (`ListGroups`)
- [List inline policies for a role](role_wrapper.py#L139) (`ListRolePolicies`)
- [List policies](policy_wrapper.py#L117) (`ListPolicies`)
- [List policies attached to a role](role_wrapper.py#L158) (`ListAttachedRolePolicies`)
- [List roles](role_wrapper.py#L81) (`ListRoles`)
- [List users](user_wrapper.py#L65) (`ListUsers`)
- [Update a user](user_wrapper.py#L85) (`UpdateUser`)
- [Update an access key](access_key_wrapper.py#L118) (`UpdateAccessKey`)

### Scenarios

Code examples that show you how to accomplish a specific task by calling multiple
functions within the same service.

- [Build and manage a resilient service](../../cross_service/resilient_service/runner.py)
- [Create a user and assume a role](scenario_create_user_assume_role.py)
- [Create read-only and read-write users](user_wrapper.py)
- [Manage access keys](access_key_wrapper.py)
- [Manage policies](policy_wrapper.py)
- [Manage roles](role_wrapper.py)
- [Manage your account](account_wrapper.py)
- [Roll back a policy version](policy_wrapper.py)


<!--custom.examples.start-->
<!--custom.examples.end-->

## Run the examples

### Instructions


<!--custom.instructions.start-->
<!--custom.instructions.end-->



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

#### Create a user and assume a role

This example shows you how to create a user and assume a role. 

- Create a user with no permissions.
- Create a role that grants permission to list Amazon S3 buckets for the account.
- Add a policy to let the user assume the role.
- Assume the role and list S3 buckets using temporary credentials, then clean up resources.

<!--custom.scenario_prereqs.iam_Scenario_CreateUserAssumeRole.start-->
<!--custom.scenario_prereqs.iam_Scenario_CreateUserAssumeRole.end-->

Start the example by running the following at a command prompt:

```
python scenario_create_user_assume_role.py
```


<!--custom.scenarios.iam_Scenario_CreateUserAssumeRole.start-->
<!--custom.scenarios.iam_Scenario_CreateUserAssumeRole.end-->

#### Create read-only and read-write users

This example shows you how to create users and attach policies to them. 

- Create two IAM users.
- Attach a policy for one user to get and put objects in an Amazon S3 bucket.
- Attach a policy for the second user to get objects from the bucket.
- Get different permissions to the bucket based on user credentials.

<!--custom.scenario_prereqs.iam_Scenario_UserPolicies.start-->
<!--custom.scenario_prereqs.iam_Scenario_UserPolicies.end-->

Start the example by running the following at a command prompt:

```
python user_wrapper.py
```


<!--custom.scenarios.iam_Scenario_UserPolicies.start-->
<!--custom.scenarios.iam_Scenario_UserPolicies.end-->

#### Manage access keys

This example shows you how to manage access keys. 

- Create and list access keys.
- Find out when and how an access key was last used.
- Update and delete access keys.

<!--custom.scenario_prereqs.iam_Scenario_ManageAccessKeys.start-->
<!--custom.scenario_prereqs.iam_Scenario_ManageAccessKeys.end-->

Start the example by running the following at a command prompt:

```
python access_key_wrapper.py
```


<!--custom.scenarios.iam_Scenario_ManageAccessKeys.start-->
<!--custom.scenarios.iam_Scenario_ManageAccessKeys.end-->

#### Manage policies

This example shows you how to do the following:

- Create and list policies.
- Create and get policy versions.
- Roll back a policy to a previous version.
- Delete policies.

<!--custom.scenario_prereqs.iam_Scenario_PolicyManagement.start-->
<!--custom.scenario_prereqs.iam_Scenario_PolicyManagement.end-->

Start the example by running the following at a command prompt:

```
python policy_wrapper.py
```


<!--custom.scenarios.iam_Scenario_PolicyManagement.start-->
<!--custom.scenarios.iam_Scenario_PolicyManagement.end-->

#### Manage roles

This example shows you how to do the following:

- Create an IAM role.
- Attach and detach policies for a role.
- Delete a role.

<!--custom.scenario_prereqs.iam_Scenario_RoleManagement.start-->
<!--custom.scenario_prereqs.iam_Scenario_RoleManagement.end-->

Start the example by running the following at a command prompt:

```
python role_wrapper.py
```


<!--custom.scenarios.iam_Scenario_RoleManagement.start-->
<!--custom.scenarios.iam_Scenario_RoleManagement.end-->

#### Manage your account

This example shows you how to do the following:

- Get and update the account alias.
- Generate a report of users and credentials.
- Get a summary of account usage.
- Get details for all users, groups, roles, and policies in your account, including their relationships to each other.

<!--custom.scenario_prereqs.iam_Scenario_AccountManagement.start-->
<!--custom.scenario_prereqs.iam_Scenario_AccountManagement.end-->

Start the example by running the following at a command prompt:

```
python account_wrapper.py
```


<!--custom.scenarios.iam_Scenario_AccountManagement.start-->
<!--custom.scenarios.iam_Scenario_AccountManagement.end-->

#### Roll back a policy version

This example shows you how to do the following:

- Get the list of policy versions in order by date.
- Find the default policy version.
- Make the previous policy version the default.
- Delete the old default version.

<!--custom.scenario_prereqs.iam_Scenario_RollbackPolicyVersion.start-->
<!--custom.scenario_prereqs.iam_Scenario_RollbackPolicyVersion.end-->

Start the example by running the following at a command prompt:

```
python policy_wrapper.py
```


<!--custom.scenarios.iam_Scenario_RollbackPolicyVersion.start-->
<!--custom.scenarios.iam_Scenario_RollbackPolicyVersion.end-->

### Tests

⚠ Running tests might result in charges to your AWS account.


To find instructions for running these tests, see the [README](../../README.md#Tests)
in the `python` folder.



<!--custom.tests.start-->
<!--custom.tests.end-->

## Additional resources

- [IAM User Guide](https://docs.aws.amazon.com/IAM/latest/UserGuide/introduction.html)
- [IAM API Reference](https://docs.aws.amazon.com/IAM/latest/APIReference/welcome.html)
- [SDK for Python IAM reference](https://boto3.amazonaws.com/v1/documentation/api/latest/reference/services/iam.html)

<!--custom.resources.start-->
<!--custom.resources.end-->

---

Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.

SPDX-License-Identifier: Apache-2.0