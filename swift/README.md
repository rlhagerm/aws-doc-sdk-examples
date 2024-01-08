# AWS SDK for Swift code examples

## Overview

The code examples in this topic show you how to use the AWS SDK for Swift with AWS.

The AWS SDK for Swift provides a Swift API for AWS infrastructure services. Using the SDK, you can build applications on top of Amazon S3, Amazon EC2, Amazon DynamoDB, and more.

## Types of code examples

- **Single-service actions** - Code examples that show you how to call individual service functions.

- **Single-service scenarios** - Code examples that show you how to accomplish a specific task by calling multiple functions within the same service.

- **Cross-service examples** - Sample applications that work across multiple AWS services.

### Find code examples

Single-service actions and scenarios are organized by AWS service in the
[example_code](https://github.com/awsdocs/aws-doc-sdk-examples/tree/main/swift/example_code/)
directory. A README in each folder lists and describes how to run the examples.

There are currently no cross-service examples for the AWS SDK for Swift. To request a cross-service example, create an issue in the [AWS SDK Code Examples](https://github.com/awsdocs/aws-doc-sdk-examples/) repo.

## ⚠️ Important

- Running this code might result in charges to your AWS account.
- Running the tests might result in charges to your AWS account.
- We recommend that you grant your code least privilege. At most, grant only the minimum permissions required to perform the task. For more information, see [Grant least privilege](https://docs.aws.amazon.com/IAM/latest/UserGuide/best-practices.html#grant-least-privilege).
- This code is not tested in every AWS Region. For more information, see [AWS Regional Services](https://aws.amazon.com/about-aws/global-infrastructure/regional-product-services).

### Prerequisites

- You must have an AWS account, and have your default credentials and AWS Region configured as described in the [Getting started](https://docs.aws.amazon.com/sdk-for-swift/latest/developer-guide/getting-started.html) section of the _AWS SDK for Swift Developer Guide_.
- You must have the [Swift](https://www.swift.org/) compiler (version 5.4 or later) and tools installed. If you have the latest version of Xcode installed, you already have the Swift tools. For detailed instructions, see [Setting up](https://docs.aws.amazon.com/sdk-for-swift/latest/developer-guide/setting-up.html) in the _AWS SDK for Swift Developer Guide_.
- Some examples require newer versions of the Swift compiler. See the comment at the top of the example's `Package.swift` file to find the minimum Swift tools version required by the example.

To build any of these examples from a terminal window, navigate into its directory then use the command:

    $ swift build

To build one of these examples in Xcode, navigate to the example's directory
(such as the `FindOrCreateIdentityPool` directory, to build that example), then
type `xed .` to open the example directory in Xcode. You can then use standard
Xcode build and run commands.

## Tests

**Note**: Running the tests might result in charges to your AWS account.

To run the tests for an example, use the command `swift test` in the example's directory.

## Docker image (Beta)

This example code will soon be available in a container image
hosted on [Amazon Elastic Container Registry (ECR)](https://docs.aws.amazon.com/AmazonECR/latest/userguide/what-is-ecr.html). This image will be pre-loaded
with all Swift examples with dependencies pre-resolved, allowing you to explore
these examples in an isolated environment.

⚠️ As of January 2023, the [SDK for Swift image](https://gallery.ecr.aws/b4v4v1s0/swift) is available on ECR Public but is still
undergoing active development. Refer to
[this GitHub issue](https://github.com/awsdocs/aws-doc-sdk-examples/issues/4132)
for more information.

## Additional resources

- [AWS SDK for Swift Developer Guide](https://docs.aws.amazon.com/sdk-for-swift/latest/developer-guide) - Documentation for the AWS SDK for Swift
- [AWS SDK for Swift](https://github.com/awslabs/aws-sdk-swift) on GitHub - Contribute to the AWS SDK for Swift
- [The Swift Programming Language](https://docs.swift.org/swift-book) - The definitive reference and guide for Swift programmers

Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved. SPDX-License-Identifier: Apache-2.0
