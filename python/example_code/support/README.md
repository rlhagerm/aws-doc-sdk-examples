# Support code examples for the SDK for Python

## Overview

Shows how to use the AWS SDK for Python (Boto3) to work with AWS Support.

<!--custom.overview.start-->
<!--custom.overview.end-->

_Support provides support for users of Amazon Web Services._

## ⚠ Important

* Running this code might result in charges to your AWS account. For more details, see [AWS Pricing](https://aws.amazon.com/pricing/) and [Free Tier](https://aws.amazon.com/free/).
* Running the tests might result in charges to your AWS account.
* We recommend that you grant your code least privilege. At most, grant only the minimum permissions required to perform the task. For more information, see [Grant least privilege](https://docs.aws.amazon.com/IAM/latest/UserGuide/best-practices.html#grant-least-privilege).
* This code is not tested in every AWS Region. For more information, see [AWS Regional Services](https://aws.amazon.com/about-aws/global-infrastructure/regional-product-services).

<!--custom.important.start-->
You must have a Business, Enterprise On-Ramp, or Enterprise Support plan in order to run these examples.
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

- [Hello Support](hello.py#L4) (`DescribeServices`)


### Single actions

Code excerpts that show you how to call individual service functions.

- [Add a communication to a case](support_wrapper.py#L179) (`AddCommunicationToCase`)
- [Add an attachment to a set](support_wrapper.py#L19) (`AddAttachmentsToSet`)
- [Create a case](support_wrapper.py#L103) (`CreateCase`)
- [Describe an attachment](support_wrapper.py#L242) (`DescribeAttachment`)
- [Describe cases](support_wrapper.py#L304) (`DescribeCases`)
- [Describe communications](support_wrapper.py#L210) (`DescribeCommunications`)
- [Describe services](support_wrapper.py#L39) (`DescribeServices`)
- [Describe severity levels](support_wrapper.py#L71) (`DescribeSeverityLevels`)
- [Resolve case](support_wrapper.py#L274) (`ResolveCase`)

### Scenarios

Code examples that show you how to accomplish a specific task by calling multiple
functions within the same service.

- [Get started with cases](get_started_support_cases.py)


<!--custom.examples.start-->
<!--custom.examples.end-->

## Run the examples

### Instructions


<!--custom.instructions.start-->
<!--custom.instructions.end-->

#### Hello Support

This example shows you how to get started using Support.

```
python hello.py
```


#### Get started with cases

This example shows you how to do the following:

- Get and display available services and severity levels for cases.
- Create a support case using a selected service, category, and severity level.
- Get and display a list of open cases for the current day.
- Add an attachment set and a communication to the new case.
- Describe the new attachment and communication for the case.
- Resolve the case.
- Get and display a list of resolved cases for the current day.

<!--custom.scenario_prereqs.support_Scenario_GetStartedSupportCases.start-->
<!--custom.scenario_prereqs.support_Scenario_GetStartedSupportCases.end-->

Start the example by running the following at a command prompt:

```
python get_started_support_cases.py
```


<!--custom.scenarios.support_Scenario_GetStartedSupportCases.start-->
<!--custom.scenarios.support_Scenario_GetStartedSupportCases.end-->

### Tests

⚠ Running tests might result in charges to your AWS account.


To find instructions for running these tests, see the [README](../../README.md#Tests)
in the `python` folder.



<!--custom.tests.start-->
<!--custom.tests.end-->

## Additional resources

- [Support User Guide](https://docs.aws.amazon.com/awssupport/latest/user/getting-started.html)
- [Support API Reference](https://docs.aws.amazon.com/awssupport/latest/APIReference/welcome.html)
- [SDK for Python Support reference](https://boto3.amazonaws.com/v1/documentation/api/latest/reference/services/support.html)

<!--custom.resources.start-->
<!--custom.resources.end-->

---

Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.

SPDX-License-Identifier: Apache-2.0