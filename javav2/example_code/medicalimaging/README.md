# HealthImaging code examples for the SDK for Java 2.x

## Overview

Shows how to use the AWS SDK for Java 2.x to work with AWS HealthImaging.

<!--custom.overview.start-->
<!--custom.overview.end-->

_HealthImaging is a HIPAA-eligible service that helps health care providers and their medical imaging ISV partners store, transform, and apply machine learning to medical images._

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

- [Add a tag to a resource](src/main/java/com/example/medicalimaging/TagResource.java#L60) (`TagResource`)
- [Copy an image set](src/main/java/com/example/medicalimaging/CopyImageSet.java#L70) (`CopyImageSet`)
- [Create a data store](src/main/java/com/example/medicalimaging/CreateDatastore.java#L57) (`CreateDatastore`)
- [Delete a data store](src/main/java/com/example/medicalimaging/DeleteDatastore.java#L55) (`DeleteDatastore`)
- [Delete an image set](src/main/java/com/example/medicalimaging/DeleteImageSet.java#L58) (`DeleteImageSet`)
- [Get an image frame](src/main/java/com/example/medicalimaging/GetImageFrame.java#L67) (`GetImageFrame`)
- [Get data store properties](src/main/java/com/example/medicalimaging/GetDatastore.java#L60) (`GetDatastore`)
- [Get image set properties](src/main/java/com/example/medicalimaging/GetImageSet.java#L68) (`GetImageSet`)
- [Get import job properties](src/main/java/com/example/medicalimaging/GetDicomImportJob.java#L84) (`GetDICOMImportJob`)
- [Get metadata for an image set](src/main/java/com/example/medicalimaging/GetImageSet.java#L68) (`GetImageSetMetadata`)
- [Import bulk data into a data store](src/main/java/com/example/medicalimaging/StartDicomImportJob.java#L67) (`StartDICOMImportJob`)
- [List data stores](src/main/java/com/example/medicalimaging/ListDatastores.java#L51) (`ListDatastores`)
- [List image set versions](src/main/java/com/example/medicalimaging/ListImageSetVersions.java#L66) (`ListImageSetVersions`)
- [List import jobs for a data store](src/main/java/com/example/medicalimaging/ListDicomImportJobs.java#L63) (`ListDICOMImportJobs`)
- [List tags for a resource](src/main/java/com/example/medicalimaging/ListTagsForResource.java#L61) (`ListTagsForResource`)
- [Remove a tag from a resource](src/main/java/com/example/medicalimaging/UntagResource.java#L60) (`UntagResource`)
- [Search image sets](src/main/java/com/example/medicalimaging/SearchImageSets.java#L128) (`SearchImageSets`)
- [Update image set metadata](src/main/java/com/example/medicalimaging/UpdateImageSetMetadata.java#L78) (`UpdateImageSetMetadata`)

### Scenarios

Code examples that show you how to accomplish a specific task by calling multiple
functions within the same service.

- [Tagging a data store](src/main/java/com/example/medicalimaging/TaggingDatastores.java)
- [Tagging an image set](src/main/java/com/example/medicalimaging/TaggingImageSets.java)


<!--custom.examples.start-->
<!--custom.examples.end-->

## Run the examples

### Instructions


<!--custom.instructions.start-->
<!--custom.instructions.end-->



#### Tagging a data store

This example shows you how to tag a HealthImaging data store.


<!--custom.scenario_prereqs.medical-imaging_tagging_datastores.start-->
<!--custom.scenario_prereqs.medical-imaging_tagging_datastores.end-->


<!--custom.scenarios.medical-imaging_tagging_datastores.start-->
<!--custom.scenarios.medical-imaging_tagging_datastores.end-->

#### Tagging an image set

This example shows you how to tag a HealthImaging image set.


<!--custom.scenario_prereqs.medical-imaging_tagging_imagesets.start-->
<!--custom.scenario_prereqs.medical-imaging_tagging_imagesets.end-->


<!--custom.scenarios.medical-imaging_tagging_imagesets.start-->
<!--custom.scenarios.medical-imaging_tagging_imagesets.end-->

### Tests

⚠ Running tests might result in charges to your AWS account.


To find instructions for running these tests, see the [README](../../README.md#Tests)
in the `javav2` folder.



<!--custom.tests.start-->
<!--custom.tests.end-->

## Additional resources

- [HealthImaging Developer Guide](https://docs.aws.amazon.com/healthimaging/latest/devguide/what-is.html)
- [HealthImaging API Reference](https://docs.aws.amazon.com/healthimaging/latest/APIReference/Welcome.html)
- [SDK for Java 2.x HealthImaging reference](https://sdk.amazonaws.com/java/api/latest/software/amazon/awssdk/services/medical-imaging/package-summary.html)

<!--custom.resources.start-->
<!--custom.resources.end-->

---

Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.

SPDX-License-Identifier: Apache-2.0