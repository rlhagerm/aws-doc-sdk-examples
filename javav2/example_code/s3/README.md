# Amazon S3 code examples for the SDK for Java 2.x

## Overview

Shows how to use the AWS SDK for Java 2.x to work with Amazon Simple Storage Service (Amazon S3).

<!--custom.overview.start-->
<!--custom.overview.end-->

_Amazon S3 is storage for the internet. You can use Amazon S3 to store and retrieve any amount of data at any time, from anywhere on the web._

## ⚠ Important

* Running this code might result in charges to your AWS account. For more details, see [AWS Pricing](https://aws.amazon.com/pricing/) and [Free Tier](https://aws.amazon.com/free/).
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

- [Hello Amazon S3](src/main/java/com/example/s3/HelloS3.java#L10) (`ListBuckets`)


### Single actions

Code excerpts that show you how to call individual service functions.

- [Add CORS rules to a bucket](src/main/java/com/example/s3/S3Cors.java#L12) (`PutBucketCors`)
- [Add a lifecycle configuration to a bucket](src/main/java/com/example/s3/LifecycleConfiguration.java#L11) (`PutBucketLifecycleConfiguration`)
- [Add a policy to a bucket](src/main/java/com/example/s3/SetBucketPolicy.java#L12) (`PutBucketPolicy`)
- [Copy an object from one bucket to another](src/main/java/com/example/s3/CopyObject.java#L11) (`CopyObject`)
- [Create a bucket](src/main/java/com/example/s3/CreateBucket.java#L12) (`CreateBucket`)
- [Delete a policy from a bucket](src/main/java/com/example/s3/DeleteBucketPolicy.java#L11) (`DeleteBucketPolicy`)
- [Delete an empty bucket](src/main/java/com/example/s3/S3BucketOps.java#L83) (`DeleteBucket`)
- [Delete multiple objects](src/main/java/com/example/s3/DeleteMultiObjects.java#L12) (`DeleteObjects`)
- [Delete the website configuration from a bucket](src/main/java/com/example/s3/DeleteWebsiteConfiguration.java#L12) (`DeleteBucketWebsite`)
- [Determine the existence and content type of an object](src/main/java/com/example/s3/GetObjectContentType.java#L13) (`HeadObject`)
- [Download objects to a local directory](src/main/java/com/example/s3/transfermanager/DownloadToDirectory.java#L11) (`DownloadDirectory`)
- [Enable notifications](src/main/java/com/example/s3/SetBucketEventBridgeNotification.java#L11) (`PutBucketNotificationConfiguration`)
- [Get an object from a bucket](src/main/java/com/example/s3/GetObjectData.java#L13) (`GetObject`)
- [Get the ACL of a bucket](src/main/java/com/example/s3/GetAcl.java#L10) (`GetBucketAcl`)
- [Get the policy for a bucket](src/main/java/com/example/s3/GetBucketPolicy.java#L12) (`GetBucketPolicy`)
- [List in-progress multipart uploads](src/main/java/com/example/s3/ListMultipartUploads.java#L12) (`ListMultipartUploads`)
- [List objects in a bucket](src/main/java/com/example/s3/ListObjects.java#L10) (`ListObjectsV2`)
- [Restore an archived copy of an object](src/main/java/com/example/s3/RestoreObject.java#L13) (`RestoreObject`)
- [Set a new ACL for a bucket](src/main/java/com/example/s3/SetAcl.java#L11) (`PutBucketAcl`)
- [Set the website configuration for a bucket](src/main/java/com/example/s3/SetWebsiteConfiguration.java#L11) (`PutBucketWebsite`)
- [Upload an object to a bucket](src/main/java/com/example/s3/PutObject.java#L11) (`PutObject`)
- [Upload directory to a bucket](src/main/java/com/example/s3/transfermanager/UploadADirectory.java#L11) (`UploadDirectory`)

### Scenarios

Code examples that show you how to accomplish a specific task by calling multiple
functions within the same service.

- [Get started with buckets and objects](src/main/java/com/example/s3/S3Scenario.java)
- [Parse URIs](src/main/java/com/example/s3/ParseUri.java)
- [Perform a multipart upload](src/main/java/com/example/s3/PerformMultiPartUpload.java)
- [Upload or download large files](src/main/java/com/example/s3/transfermanager/DownloadToDirectory.java)
- [Upload stream of unknown size](src/main/java/com/example/s3/async/PutObjectFromStreamAsync.java)
- [Use checksums](src/main/java/com/example/s3/BasicOpsWithChecksums.java)


<!--custom.examples.start-->
<!--custom.examples.end-->

## Run the examples

### Instructions


<!--custom.instructions.start-->
<!--custom.instructions.end-->

#### Hello Amazon S3

This example shows you how to get started using Amazon S3.



#### Get started with buckets and objects

This example shows you how to do the following:

- Create a bucket and upload a file to it.
- Download an object from a bucket.
- Copy an object to a subfolder in a bucket.
- List the objects in a bucket.
- Delete the bucket objects and the bucket.

<!--custom.scenario_prereqs.s3_Scenario_GettingStarted.start-->
<!--custom.scenario_prereqs.s3_Scenario_GettingStarted.end-->


<!--custom.scenarios.s3_Scenario_GettingStarted.start-->
<!--custom.scenarios.s3_Scenario_GettingStarted.end-->

#### Parse URIs

This example shows you how to parse Amazon S3 URIs to extract important components like the bucket name and object key.


<!--custom.scenario_prereqs.s3_Scenario_URIParsing.start-->
<!--custom.scenario_prereqs.s3_Scenario_URIParsing.end-->


<!--custom.scenarios.s3_Scenario_URIParsing.start-->
<!--custom.scenarios.s3_Scenario_URIParsing.end-->

#### Perform a multipart upload

This example shows you how to perform a multipart upload to an Amazon S3 object.


<!--custom.scenario_prereqs.s3_Scenario_MultipartUpload.start-->
<!--custom.scenario_prereqs.s3_Scenario_MultipartUpload.end-->


<!--custom.scenarios.s3_Scenario_MultipartUpload.start-->
<!--custom.scenarios.s3_Scenario_MultipartUpload.end-->

#### Upload or download large files

This example shows you how to upload or download large files to and from Amazon S3.


<!--custom.scenario_prereqs.s3_Scenario_UsingLargeFiles.start-->
<!--custom.scenario_prereqs.s3_Scenario_UsingLargeFiles.end-->


<!--custom.scenarios.s3_Scenario_UsingLargeFiles.start-->
<!--custom.scenarios.s3_Scenario_UsingLargeFiles.end-->

#### Upload stream of unknown size

This example shows you how to upload a stream of unknown size to an Amazon S3 object.


<!--custom.scenario_prereqs.s3_Scenario_UploadStream.start-->
<!--custom.scenario_prereqs.s3_Scenario_UploadStream.end-->


<!--custom.scenarios.s3_Scenario_UploadStream.start-->
<!--custom.scenarios.s3_Scenario_UploadStream.end-->

#### Use checksums

This example shows you how to use checksums to work with an Amazon S3 object.


<!--custom.scenario_prereqs.s3_Scenario_UseChecksums.start-->
<!--custom.scenario_prereqs.s3_Scenario_UseChecksums.end-->


<!--custom.scenarios.s3_Scenario_UseChecksums.start-->
<!--custom.scenarios.s3_Scenario_UseChecksums.end-->

### Tests

⚠ Running tests might result in charges to your AWS account.


To find instructions for running these tests, see the [README](../../README.md#Tests)
in the `javav2` folder.



<!--custom.tests.start-->
<!--custom.tests.end-->

## Additional resources

- [Amazon S3 User Guide](https://docs.aws.amazon.com/AmazonS3/latest/userguide/Welcome.html)
- [Amazon S3 API Reference](https://docs.aws.amazon.com/AmazonS3/latest/API/Welcome.html)
- [SDK for Java 2.x Amazon S3 reference](https://sdk.amazonaws.com/java/api/latest/software/amazon/awssdk/services/s3/package-summary.html)

<!--custom.resources.start-->
<!--custom.resources.end-->

---

Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.

SPDX-License-Identifier: Apache-2.0