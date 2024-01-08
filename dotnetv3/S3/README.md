# Amazon S3 code examples for the SDK for .NET

## Overview

Shows how to use the AWS SDK for .NET to work with Amazon Simple Storage Service (Amazon S3).

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

For prerequisites, see the [README](../README.md#Prerequisites) in the `dotnetv3` folder.


<!--custom.prerequisites.start-->
<!--custom.prerequisites.end-->

### Single actions

Code excerpts that show you how to call individual service functions.

- [Add CORS rules to a bucket](s3CORSExample/S3CORS.cs#L104) (`PutBucketCors`)
- [Add a lifecycle configuration to a bucket](LifecycleExample/Lifecycle.cs#L145) (`PutBucketLifecycleConfiguration`)
- [Cancel multipart uploads](AbortMPUExample/AbortMPU.cs#L6) (`AbortMultipartUploads`)
- [Copy an object from one bucket to another](CopyObjectExample/CopyObject.cs#L11) (`CopyObject`)
- [Create a bucket](S3_Basics/S3Bucket.cs#L12) (`CreateBucket`)
- [Delete CORS rules from a bucket](s3CORSExample/S3CORS.cs#L147) (`DeleteBucketCors`)
- [Delete an empty bucket](S3_Basics/S3Bucket.cs#L266) (`DeleteBucket`)
- [Delete an object](non-versioned-examples/DeleteObjectExample/DeleteObject.cs#L6) (`DeleteObject`)
- [Delete multiple objects](S3_Basics/S3Bucket.cs#L221) (`DeleteObjects`)
- [Delete the lifecycle configuration of a bucket](LifecycleExample/Lifecycle.cs#L192) (`DeleteBucketLifecycle`)
- [Enable logging](ServerAccessLoggingExample/ServerAccessLogging.cs#L6) (`PutBucketLogging`)
- [Enable notifications](EnableNotificationsExample/EnableNotifications.cs#L6) (`PutBucketNotificationConfiguration`)
- [Enable transfer acceleration](TransferAccelerationExample/TransferAcceleration.cs#L6) (`PutBucketAccelerateConfiguration`)
- [Get CORS rules for a bucket](s3CORSExample/S3CORS.cs#L125) (`GetBucketCors`)
- [Get an object from a bucket](S3_Basics/S3Bucket.cs#L85) (`GetObject`)
- [Get the ACL of a bucket](BucketACLExample/BucketACL.cs#L75) (`GetBucketAcl`)
- [Get the lifecycle configuration of a bucket](LifecycleExample/Lifecycle.cs#L169) (`GetBucketLifecycleConfiguration`)
- [Get the website configuration for a bucket](WebsiteConfigExample/WebsiteConfig.cs#L72) (`GetBucketWebsite`)
- [List buckets](ListBucketsExample/ListBuckets.cs#L4) (`ListBuckets`)
- [List object versions in a bucket](versioned-examples/ListObjectVersionsExample/ListObjectVersions.cs#L6) (`ListObjectVersions`)
- [List objects in a bucket](S3_Basics/S3Bucket.cs#L171) (`ListObjectsV2`)
- [Restore an archived copy of an object](RestoreArchivedObjectExample/RestoreArchivedObject.cs#L6) (`RestoreObject`)
- [Set a new ACL for a bucket](BucketACLExample/BucketACL.cs#L37) (`PutBucketAcl`)
- [Set the website configuration for a bucket](WebsiteConfigExample/WebsiteConfig.cs#L57) (`PutBucketWebsite`)
- [Upload an object to a bucket](S3_Basics/S3Bucket.cs#L43) (`PutObject`)

### Scenarios

Code examples that show you how to accomplish a specific task by calling multiple
functions within the same service.

- [Create a presigned URL](GenPresignedURLExample/GenPresignedUrl.cs)
- [Get started with buckets and objects](S3_Basics/S3_Basics.cs)
- [Get started with encryption](SSEClientEncryptionExample/SSEClientEncryption.cs)
- [Get started with tags](ObjectTagExample/ObjectTag.cs)
- [Manage access control lists (ACLs)](ManageACLsExample/ManageACLs.cs)
- [Perform a multipart copy](MPUapiCopyObjExample/MPUapiCopyObj.cs)
- [Upload or download large files](scenarios/TransferUtilityBasics/TransferUtilityBasics/TransferBasics.cs)

### Cross-service examples

Sample applications that work across multiple AWS services.

- [Create a serverless application to manage photos](../cross-service/PhotoAssetManager)
- [Detect objects in images](../cross-service/PhotoAnalyzerApp)


<!--custom.examples.start-->
<!--custom.examples.end-->

## Run the examples

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


<!--custom.instructions.start-->
<!--custom.instructions.end-->



#### Create a presigned URL

This example shows you how to create a presigned URL for Amazon S3 and upload an object.


<!--custom.scenario_prereqs.s3_Scenario_PresignedUrl.start-->
<!--custom.scenario_prereqs.s3_Scenario_PresignedUrl.end-->


<!--custom.scenarios.s3_Scenario_PresignedUrl.start-->
<!--custom.scenarios.s3_Scenario_PresignedUrl.end-->

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

#### Get started with encryption

This example shows you how to get started with encryption for Amazon S3 objects.


<!--custom.scenario_prereqs.s3_Encryption.start-->
<!--custom.scenario_prereqs.s3_Encryption.end-->


<!--custom.scenarios.s3_Encryption.start-->
<!--custom.scenarios.s3_Encryption.end-->

#### Get started with tags

This example shows you how to get started with tags for Amazon S3 objects.


<!--custom.scenario_prereqs.s3_Scenario_Tagging.start-->
<!--custom.scenario_prereqs.s3_Scenario_Tagging.end-->


<!--custom.scenarios.s3_Scenario_Tagging.start-->
<!--custom.scenarios.s3_Scenario_Tagging.end-->

#### Manage access control lists (ACLs)

This example shows you how to manage access control lists (ACLs) for Amazon S3 buckets.


<!--custom.scenario_prereqs.s3_Scenario_ManageACLs.start-->
<!--custom.scenario_prereqs.s3_Scenario_ManageACLs.end-->


<!--custom.scenarios.s3_Scenario_ManageACLs.start-->
<!--custom.scenarios.s3_Scenario_ManageACLs.end-->

#### Perform a multipart copy

This example shows you how to perform a multipart copy of an Amazon S3 object.


<!--custom.scenario_prereqs.s3_MultipartCopy.start-->
<!--custom.scenario_prereqs.s3_MultipartCopy.end-->


<!--custom.scenarios.s3_MultipartCopy.start-->
<!--custom.scenarios.s3_MultipartCopy.end-->

#### Upload or download large files

This example shows you how to upload or download large files to and from Amazon S3.


<!--custom.scenario_prereqs.s3_Scenario_UsingLargeFiles.start-->
<!--custom.scenario_prereqs.s3_Scenario_UsingLargeFiles.end-->


<!--custom.scenarios.s3_Scenario_UsingLargeFiles.start-->
<!--custom.scenarios.s3_Scenario_UsingLargeFiles.end-->

### Tests

⚠ Running tests might result in charges to your AWS account.


To find instructions for running these tests, see the [README](../README.md#Tests)
in the `dotnetv3` folder.



<!--custom.tests.start-->
<!--custom.tests.end-->

## Additional resources

- [Amazon S3 User Guide](https://docs.aws.amazon.com/AmazonS3/latest/userguide/Welcome.html)
- [Amazon S3 API Reference](https://docs.aws.amazon.com/AmazonS3/latest/API/Welcome.html)
- [SDK for .NET Amazon S3 reference](https://docs.aws.amazon.com/sdkfornet/v3/apidocs/items/S3/NS3.html)

<!--custom.resources.start-->
<!--custom.resources.end-->

---

Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.

SPDX-License-Identifier: Apache-2.0