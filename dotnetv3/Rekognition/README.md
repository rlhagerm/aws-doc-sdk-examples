# Amazon Rekognition code examples for the SDK for .NET

## Overview

Shows how to use the AWS SDK for .NET to work with Amazon Rekognition.

<!--custom.overview.start-->
<!--custom.overview.end-->

_Amazon Rekognition makes it easy to add image and video analysis to your applications._

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

- [Compare faces in an image against a reference image](CompareFacesExample/CompareFaces.cs#L6) (`CompareFaces`)
- [Create a collection](CreateCollectionExample/CreateCollection.cs#L6) (`CreateCollection`)
- [Delete a collection](DeleteCollectionExample/DeleteCollection.cs#L6) (`DeleteCollection`)
- [Delete faces from a collection](DeleteFacesExample/DeleteFaces.cs#L6) (`DeleteFaces`)
- [Describe a collection](DescribeCollectionExample/DescribeCollection.cs#L6) (`DescribeCollection`)
- [Detect faces in an image](DetectFacesExample/DetectFaces.cs#L6) (`DetectFaces`)
- [Detect labels in an image](DetectLabelsExample/DetectLabels.cs#L6) (`DetectLabels`)
- [Detect moderation labels in an image](DetectModerationLabelsExample/DetectModerationLabels.cs#L6) (`DetectModerationLabels`)
- [Detect text in an image](DetectTextExample/DetectText.cs#L6) (`DetectText`)
- [Get information about celebrities](CelebrityInfoExample/CelebrityInfo.cs#L6) (`GetCelebrityInfo`)
- [Index faces to a collection](AddFacesExample/AddFaces.cs#L6) (`IndexFaces`)
- [List collections](ListCollectionsExample/ListCollections.cs#L6) (`ListCollections`)
- [List faces in a collection](ListFacesExample/ListFaces.cs#L6) (`ListFaces`)
- [Recognize celebrities in an image](CelebritiesInImageExample/CelebritiesInImage.cs#L6) (`RecognizeCelebrities`)
- [Search for faces in a collection](SearchFacesMatchingIdExample/SearchFacesMatchingId.cs#L6) (`SearchFaces`)
- [Search for faces in a collection compared to a reference image](SearchFacesMatchingImageExample/SearchFacesMatchingImage.cs#L6) (`SearchFacesByImage`)

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



### Tests

⚠ Running tests might result in charges to your AWS account.


To find instructions for running these tests, see the [README](../README.md#Tests)
in the `dotnetv3` folder.



<!--custom.tests.start-->
<!--custom.tests.end-->

## Additional resources

- [Amazon Rekognition Developer Guide](https://docs.aws.amazon.com/rekognition/latest/dg/what-is.html)
- [Amazon Rekognition API Reference](https://docs.aws.amazon.com/rekognition/latest/APIReference/Welcome.html)
- [SDK for .NET Amazon Rekognition reference](https://docs.aws.amazon.com/sdkfornet/v3/apidocs/items/Rekognition/NRekognition.html)

<!--custom.resources.start-->
<!--custom.resources.end-->

---

Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.

SPDX-License-Identifier: Apache-2.0