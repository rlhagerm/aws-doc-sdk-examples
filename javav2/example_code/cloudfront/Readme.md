# CloudFront code examples for the SDK for Java 2.x

## Overview

Shows how to use the AWS SDK for Java 2.x to work with Amazon CloudFront.

<!--custom.overview.start-->
<!--custom.overview.end-->

_CloudFront speeds up distribution of your static and dynamic web content, such as .html, .css, .php, image, and media files._

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

### Single actions

Code excerpts that show you how to call individual service functions.

- [Create a distribution](src/main/java/com/example/cloudfront/CreateDistribution.java#L11) (`CreateDistribution`)
- [Create a function](src/main/java/com/example/cloudfront/CreateFunction.java#L12) (`CreateFunction`)
- [Create a key group](src/main/java/com/example/cloudfront/CreateKeyGroup.java#L11) (`CreateKeyGroup`)
- [Delete a distribution](src/main/java/com/example/cloudfront/DeleteDistribution.java#L11) (`DeleteDistribution`)
- [Delete signing resources](src/main/java/com/example/cloudfront/DeleteSigningResources.java#L11) (`DeleteKeyGroup`)
- [Update a distribution](src/main/java/com/example/cloudfront/ModifyDistribution.java#L12) (`UpdateDistribution`)
- [Upload a public key](src/main/java/com/example/cloudfront/CreatePublicKey.java#L11) (`CreatePublicKey`)

### Scenarios

Code examples that show you how to accomplish a specific task by calling multiple
functions within the same service.

- [Sign URLs and cookies](src/main/java/com/example/cloudfront/CreateCannedPolicyRequest.java)


<!--custom.examples.start-->
<!--custom.examples.end-->

## Run the examples

### Instructions


<!--custom.instructions.start-->
<!--custom.instructions.end-->



#### Sign URLs and cookies

This example shows you how to create signed URLs and cookies that allow access to restricted resources.


<!--custom.scenario_prereqs.cloudfront_CloudFrontUtilities.start-->
<!--custom.scenario_prereqs.cloudfront_CloudFrontUtilities.end-->


<!--custom.scenarios.cloudfront_CloudFrontUtilities.start-->
<!--custom.scenarios.cloudfront_CloudFrontUtilities.end-->

### Tests

⚠ Running tests might result in charges to your AWS account.


To find instructions for running these tests, see the [README](../../README.md#Tests)
in the `javav2` folder.



<!--custom.tests.start-->
<!--custom.tests.end-->

## Additional resources

- [CloudFront Developer Guide](https://docs.aws.amazon.com/AmazonCloudFront/latest/DeveloperGuide/Introduction.html)
- [CloudFront API Reference](https://docs.aws.amazon.com/cloudfront/latest/APIReference/Welcome.html)
- [SDK for Java 2.x CloudFront reference](https://sdk.amazonaws.com/java/api/latest/software/amazon/awssdk/services/cloudfront/package-summary.html)

<!--custom.resources.start-->
<!--custom.resources.end-->

---

Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.

SPDX-License-Identifier: Apache-2.0