# IAM code examples for the SDK for Java 2.x

## Overview

Shows how to use the AWS SDK for Java 2.x to work with AWS Identity and Access Management (IAM).

<!--custom.overview.start-->
<!--custom.overview.end-->

_IAM is a web service for securely controlling access to AWS services. With IAM, you can centrally manage permissions in your AWS account._

## ⚠ Important

* Running this code might result in charges to your AWS account. For more details, see [AWS Pricing](https://aws.amazon.com/pricing/?aws-products-pricing.sort-by=item.additionalFields.productNameLowercase&aws-products-pricing.sort-order=asc&awsf.Free%20Tier%20Type=*all&awsf.tech-category=*all) and [Free Tier](https://aws.amazon.com/free/?all-free-tier.sort-by=item.additionalFields.SortRank&all-free-tier.sort-order=asc&awsf.Free%20Tier%20Types=*all&awsf.Free%20Tier%20Categories=*all).
* Running the tests might result in charges to your AWS account.
* We recommend that you grant your code least privilege. At most, grant only the minimum permissions required to perform the task. For more information, see [Grant least privilege](https://docs.aws.amazon.com/IAM/latest/UserGuide/best-practices.html#grant-least-privilege).
* This code is not tested in every AWS Region. For more information, see [AWS Regional Services](https://aws.amazon.com/about-aws/global-infrastructure/regional-product-services).

<!--custom.important.start-->
<!--custom.important.end-->

## Code examples

### Prerequisites

For prerequisites, see the [README](../../README.md#Prerequisites) in the `javav2` folder.


<!--custom.prerequisites.start-->
<!--custom.prerequisites.end-->

### Get started

- [Hello IAM](src/main/java/com/example/iam/HelloIAM.java#L11) (`ListPolicies`)


### Single actions

Code excerpts that show you how to call individual service functions.

- [Attach a policy to a role](src/main/java/com/example/iam/AttachRolePolicy.java#L11) (`AttachRolePolicy`)
- [Create a policy](src/main/java/com/example/iam/CreatePolicy.java#L11) (`CreatePolicy`)
- [Create a role](src/main/java/com/example/iam/CreateRole.java#L12) (`CreateRole`)
- [Create a user](src/main/java/com/example/iam/CreateUser.java#L11) (`CreateUser`)
- [Create an access key](src/main/java/com/example/iam/CreateAccessKey.java#L11) (`CreateAccessKey`)
- [Create an alias for an account](src/main/java/com/example/iam/CreateAccountAlias.java#L11) (`CreateAccountAlias`)
- [Delete a policy](src/main/java/com/example/iam/DeletePolicy.java#L11) (`DeletePolicy`)
- [Delete a user](src/main/java/com/example/iam/DeleteUser.java#L11) (`DeleteUser`)
- [Delete an access key](src/main/java/com/example/iam/DeleteAccessKey.java#L11) (`DeleteAccessKey`)
- [Delete an account alias](src/main/java/com/example/iam/DeleteAccountAlias.java#L11) (`DeleteAccountAlias`)
- [Detach a policy from a role](src/main/java/com/example/iam/DetachRolePolicy.java#L10) (`DetachRolePolicy`)
- [List a user's access keys](src/main/java/com/example/iam/ListAccessKeys.java#L10) (`ListAccessKeys`)
- [List account aliases](src/main/java/com/example/iam/ListAccountAliases.java#L11) (`ListAccountAliases`)
- [List users](src/main/java/com/example/iam/ListUsers.java#L11) (`ListUsers`)
- [Update a user](src/main/java/com/example/iam/UpdateUser.java#L11) (`UpdateUser`)
- [Update an access key](src/main/java/com/example/iam/UpdateAccessKey.java#L11) (`UpdateAccessKey`)

### Scenarios

Code examples that show you how to accomplish a specific task by calling multiple
functions within the same service.

- [Build and manage a resilient service](../../usecases/resilient_service/src/main/java/com/example/resilient/Main.java)
- [Create a user and assume a role](src/main/java/com/example/iam/IAMScenario.java)
- [Work with the IAM Policy Builder API](src/main/java/com/example/iam/IamPolicyBuilderExamples.java)


<!--custom.examples.start-->
<!--custom.examples.end-->

## Run the examples

### Instructions


<!--custom.instructions.start-->
<!--custom.instructions.end-->

#### Hello IAM

This example shows you how to get started using IAM.



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

#### Work with the IAM Policy Builder API

This example shows you how to do the following:

- Create IAM policies by using the object-oriented API.
- Use the IAM Policy Builder API with the IAM service.

<!--custom.scenario_prereqs.iam_Scenario_IamPolicyBuilder.start-->
<!--custom.scenario_prereqs.iam_Scenario_IamPolicyBuilder.end-->


<!--custom.scenarios.iam_Scenario_IamPolicyBuilder.start-->
<!--custom.scenarios.iam_Scenario_IamPolicyBuilder.end-->

### Tests

⚠ Running tests might result in charges to your AWS account.


To find instructions for running these tests, see the [README](../../README.md#Tests)
in the `javav2` folder.



<!--custom.tests.start-->
<!--custom.tests.end-->

## Additional resources

- [IAM User Guide](https://docs.aws.amazon.com/IAM/latest/UserGuide/introduction.html)
- [IAM API Reference](https://docs.aws.amazon.com/IAM/latest/APIReference/welcome.html)
- [SDK for Java 2.x IAM reference](https://sdk.amazonaws.com/java/api/latest/software/amazon/awssdk/services/iam/package-summary.html)

<!--custom.resources.start-->
<!--custom.resources.end-->

---

Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.

SPDX-License-Identifier: Apache-2.0