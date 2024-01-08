# AWS SDK for Kotlin code examples

## Purpose

These examples demonstrate how to perform AWS service operations using the beta version of the AWS SDK for Kotlin.

## Prerequisites

You must have an AWS account, and have configured your default credentials and AWS Region as described in [AWS SDK for Kotlin
Developer Guide](https://docs.aws.amazon.com/sdk-for-kotlin/latest/developer-guide).

## Running the code

To run these examples, you can setup your development environment to use Gradle to configure and build AWS SDK for Kotlin projects. For more information,
see [Get started with the AWS SDK for Kotlin](https://docs.aws.amazon.com/sdk-for-kotlin/latest/developer-guide/setup.html).

See the individual readme files in each service directory for information about specific code examples for that service.

## Tests

You can run the JUnit tests from an IDE, such as IntelliJ, or from the command line. As each test runs, you can view messages that inform you if the various tests succeed or fail. For example, the following message informs you that Test 3 passed.

    Test 3 passed

Before running the JUnit tests, you must define values in the **config.properties** file located in the **resources** folder. This file contains values that are required to run the JUnit tests. If you do not define all values, the JUnit tests fail.

## Usecases

In the **usecases** folder, find step-by-step development tutorials that use multiple AWS services and the AWS SDK for Kotlin. By following these tutorials, you will gain a deeper understanding of how to create applications that use the AWS SDK for Kotlin. These tutorials include:

- [Create a React and Spring REST application that handles Amazon SQS messages](https://github.com/awsdocs/aws-doc-sdk-examples/tree/main/kotlin/usecases/creating_message_application) - Discusses how to develop a Spring REST API that sends and retrieves messages by using the AWS SDK for Kotlin and Amazon Simple Queue Service (Amazon SQS). This application also detects the language code of the posted message by using Amazon Comprehend. The Spring REST API is used by a React application that displays the data.

- [Create a React and Spring REST application that queries Amazon DynamoDB data](https://github.com/awsdocs/aws-doc-sdk-examples/tree/main/kotlin/usecases/itemtracker_dynamodb) - Discusses how to develop a Spring REST API that queries Amazon DynamoDB data. The Spring REST API uses the AWS SDK for Kotlin to invoke AWS services and is used by a React application that displays the data.

- [Create a React and Spring REST application that queries Amazon Redshift data](https://github.com/scmacdon/aws-doc-sdk-examples/tree/main/kotlin/usecases/creating_redshift_application) - Discusses how to develop a Spring REST API that queries Amazon Redshift data. The Spring REST API uses the AWS SDK for Kotlin to invoke AWS services and is used by a React application that displays the data.

- [Create a React and Spring REST application that queries Amazon Aurora data](https://github.com/scmacdon/aws-doc-sdk-examples/tree/main/kotlin/usecases/serverless_rds) - Discusses how to develop a Spring REST API that queries Amazon Aurora data. The Spring REST API uses the AWS SDK for Kotlin to invoke AWS services and is used by a React application that displays the data.

- [Create an example photo analyzer application using the AWS SDK for Kotlin](https://github.com/scmacdon/aws-doc-sdk-examples/tree/main/kotlin/usecases/creating_photo_analyzer_app) - Discusses using the AWS SDK for Kotlin and various AWS services, such as the Amazon Rekognition service, to analyze images. This application analyzes many images and generates a report that breaks down each image into a series of labels.

- [Create AWS serverless workflows using the AWS SDK for Kotlin](https://github.com/scmacdon/aws-doc-sdk-examples/tree/main/kotlin/usecases/creating_workflows_stepfunctions) - Discusses using the AWS SDK for Kotlin and AWS Step Functions to create a workflow that invokes AWS services. Each workflow step is implemented by using an AWS Lambda function.

- [Create a Spring Boot Application that has publish-subscribe functionality](https://github.com/scmacdon/aws-doc-sdk-examples/tree/main/kotlin/usecases/subpub_app) - Discusses how to create a web application that has subscription and publish functionality. In this tutorial, the Spring Framework along with AWS SDK for Kotlin is used to create the application.

- [Create a publish-subscribe Android application that translates messages using the AWS SDK for Kotlin](https://github.com/scmacdon/aws-doc-sdk-examples/tree/main/kotlin/usecases/subpub_app_android) - Discusses how to create an Android application that has subscription and publish functionality.

### Notes

- We recommend that you grant this code least privilege,
  or at most the minimum permissions required to perform the task.
  For more information, see
  [Grant Least Privilege](https://docs.aws.amazon.com/IAM/latest/UserGuide/best-practices.html#grant-least-privilege)
  in the AWS Identity and Access Management User Guide.
- This code has not been tested in all AWS Regions.
  Some AWS services are available only in specific
  [Regions](https://aws.amazon.com/about-aws/global-infrastructure/regional-product-services).
- Running this code might result in charges to your AWS account.

## Docker image (Beta)

This example code will soon be available in a container image
hosted on [Amazon Elastic Container Registry (ECR)](https://docs.aws.amazon.com/AmazonECR/latest/userguide/what-is-ecr.html). This image will be pre-loaded
with all Kotlin examples with dependencies pre-resolved, allowing you to explore
these examples in an isolated environment.

⚠️ As of January 2023, the [SDK for Kotlin image](https://gallery.ecr.aws/b4v4v1s0/kotlin) is available but still
undergoing active development. Refer to
[this GitHub issue](https://github.com/awsdocs/aws-doc-sdk-examples/issues/4130)
for more information.

---

Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
SPDX-License-Identifier: Apache-2.0
