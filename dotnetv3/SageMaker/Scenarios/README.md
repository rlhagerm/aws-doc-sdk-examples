# Create and execute a SageMaker geospatial pipeline using an AWS SDK

## Overview

This scenario demonstrates how to work with Amazon SageMaker (SageMaker) pipelines and geospatial jobs.


## ⚠ Important

* Running this code might result in charges to your AWS account.
* Running the tests might result in charges to your AWS account.
* We recommend that you grant your code least privilege. At most, grant only the minimum permissions required to perform the task. For more information, see [Grant least privilege](https://docs.aws.amazon.com/IAM/latest/UserGuide/best-practices.html#grant-least-privilege).
* This code is not tested in every AWS Region. For more information, see [AWS Regional Services](https://aws.amazon.com/about-aws/global-infrastructure/regional-product-services).

## Scenario

### Prerequisites

To view pipelines in SageMaker 
For general prerequisites, see the [README](../README.md#Prerequisites) in the `dotnetv3` folder.

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

#### Get started with geospatial jobs and pipelines

This example shows you how to do the following:

* Set up resources for a pipeline.
* Set up a pipeline that executes a geospatial job.
* Start a pipeline execution.
* Monitor the status of the execution.
* View the output of the pipeline.
* Clean up resources.

## Additional resources

* [SageMaker Developer Guide](https://docs.aws.amazon.com/sagemaker/latest/dg/whatis.html)
* [SageMaker API Reference](https://docs.aws.amazon.com/sagemaker/latest/APIReference/Welcome.html)
* [SDK for .NET SageMaker reference](https://docs.aws.amazon.com/sdkfornet/v3/apidocs/items/SageMaker/NSageMaker.html)

---

Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.

SPDX-License-Identifier: Apache-2.0