# Lambda code examples for the SDK for C++

## Overview

Shows how to use the AWS SDK for C++ to work with AWS Lambda.

<!--custom.overview.start-->
<!--custom.overview.end-->

_Lambda allows you to run code without provisioning or managing servers._

## ⚠ Important

* Running this code might result in charges to your AWS account. For more details, see [AWS Pricing](https://aws.amazon.com/pricing/?aws-products-pricing.sort-by=item.additionalFields.productNameLowercase&aws-products-pricing.sort-order=asc&awsf.Free%20Tier%20Type=*all&awsf.tech-category=*all) and [Free Tier](https://aws.amazon.com/free/?all-free-tier.sort-by=item.additionalFields.SortRank&all-free-tier.sort-order=asc&awsf.Free%20Tier%20Types=*all&awsf.Free%20Tier%20Categories=*all).
* Running the tests might result in charges to your AWS account.
* We recommend that you grant your code least privilege. At most, grant only the minimum permissions required to perform the task. For more information, see [Grant least privilege](https://docs.aws.amazon.com/IAM/latest/UserGuide/best-practices.html#grant-least-privilege).
* This code is not tested in every AWS Region. For more information, see [AWS Regional Services](https://aws.amazon.com/about-aws/global-infrastructure/regional-product-services).

<!--custom.important.start-->
<!--custom.important.end-->

## Code examples

### Prerequisites



Before using the code examples, first complete the installation and setup steps
for [Getting started](https://docs.aws.amazon.com/sdk-for-cpp/v1/developer-guide/getting-started.html) in the AWS SDK for
C++ Developer Guide.
This section covers how to get and build the SDK, and how to build your own code by using the SDK with a
sample Hello World-style application.

Next, for information on code example structures and how to build and run the examples, see [Getting started with the AWS SDK for C++ code examples](https://docs.aws.amazon.com/sdk-for-cpp/v1/developer-guide/getting-started-code-examples.html).


<!--custom.prerequisites.start-->
<!--custom.prerequisites.end-->

### Get started

- [Hello Lambda](hello_lambda/CMakeLists.txt#L4) (`ListFunctions`)


### Single actions

Code excerpts that show you how to call individual service functions.

- [Create a function](get_started_with_functions_scenario.cpp#L607) (`CreateFunction`)
- [Delete a function](get_started_with_functions_scenario.cpp#L607) (`DeleteFunction`)
- [Get a function](get_started_with_functions_scenario.cpp#L607) (`GetFunction`)
- [Invoke a function](get_started_with_functions_scenario.cpp#L607) (`Invoke`)
- [List functions](get_started_with_functions_scenario.cpp#L607) (`ListFunctions`)
- [Update function code](get_started_with_functions_scenario.cpp#L607) (`UpdateFunctionCode`)
- [Update function configuration](get_started_with_functions_scenario.cpp#L607) (`UpdateFunctionConfiguration`)

### Scenarios

Code examples that show you how to accomplish a specific task by calling multiple
functions within the same service.

- [Get started with functions](get_started_with_functions_scenario.cpp)

### Cross-service examples

Sample applications that work across multiple AWS services.

- [Create a serverless application to manage photos](../../example_code/cross-service/photo_asset_manager)


<!--custom.examples.start-->
<!--custom.examples.end-->

## Run the examples

### Instructions

An executable is built for each source file. These executables are located in the build folder and have
"run_" prepended to the source file name, minus the suffix. See the "main" function in the source file for further instructions.

For example, to run the action in the source file "my_action.cpp", execute the following command from within the build folder. The command
will display any required arguments.

```
./run_my_action
```

<!--custom.instructions.start-->
<!--custom.instructions.end-->

#### Hello Lambda

This example shows you how to get started using Lambda.



#### Get started with functions

This example shows you how to do the following:

- Create an IAM role and Lambda function, then upload handler code.
- Invoke the function with a single parameter and get results.
- Update the function code and configure with an environment variable.
- Invoke the function with new parameters and get results. Display the returned execution log.
- List the functions for your account, then clean up resources.

<!--custom.scenario_prereqs.lambda_Scenario_GettingStartedFunctions.start-->
<!--custom.scenario_prereqs.lambda_Scenario_GettingStartedFunctions.end-->


<!--custom.scenarios.lambda_Scenario_GettingStartedFunctions.start-->
<!--custom.scenarios.lambda_Scenario_GettingStartedFunctions.end-->

### Tests

⚠ Running tests might result in charges to your AWS account.



```sh
   cd <BUILD_DIR>
   cmake <path-to-root-of-this-source-code> -DBUILD_TESTS=ON
   make
   ctest
```


<!--custom.tests.start-->
<!--custom.tests.end-->

## Additional resources

- [Lambda Developer Guide](https://docs.aws.amazon.com/lambda/latest/dg/welcome.html)
- [Lambda API Reference](https://docs.aws.amazon.com/lambda/latest/dg/API_Reference.html)
- [SDK for C++ Lambda reference](https://sdk.amazonaws.com/cpp/api/LATEST/aws-cpp-sdk-lambda/html/annotated.html)

<!--custom.resources.start-->
<!--custom.resources.end-->

---

Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.

SPDX-License-Identifier: Apache-2.0