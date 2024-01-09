# HealthImaging code examples for the SDK for JavaScript (v3)

## Overview

Shows how to use the AWS SDK for JavaScript (v3) to work with AWS HealthImaging.

<!--custom.overview.start-->
<!--custom.overview.end-->

_HealthImaging is a HIPAA-eligible service that helps health care providers and their medical imaging ISV partners store, transform, and apply machine learning to medical images._

## ⚠ Important

* Running this code might result in charges to your AWS account. For more details, see [AWS Pricing](https://aws.amazon.com/pricing/) and [Free Tier](https://aws.amazon.com/free/).
* Running the tests might result in charges to your AWS account.
* We recommend that you grant your code least privilege. At most, grant only the minimum permissions required to perform the task. For more information, see [Grant least privilege](https://docs.aws.amazon.com/IAM/latest/UserGuide/best-practices.html#grant-least-privilege).
* This code is not tested in every AWS Region. For more information, see [AWS Regional Services](https://aws.amazon.com/about-aws/global-infrastructure/regional-product-services).

<!--custom.important.start-->
<!--custom.important.end-->

## Code examples

### Prerequisites

For prerequisites, see the [README](../../README.md#Prerequisites) in the `javascriptv3` folder.


<!--custom.prerequisites.start-->
<!--custom.prerequisites.end-->

### Get started

- [Hello HealthImaging](actions/create-datastore.js#L8) (`ListDatastores`)


### Single actions

Code excerpts that show you how to call individual service functions.

- [Add a tag to a resource](actions/tag-resource.js#L8) (`TagResource`)
- [Copy an image set](actions/copy-image-set.js#L8) (`CopyImageSet`)
- [Create a data store](actions/create-datastore.js#L8) (`CreateDatastore`)
- [Delete a data store](actions/delete-datastore.js#L8) (`DeleteDatastore`)
- [Delete an image set](actions/delete-image-set.js#L8) (`DeleteImageSet`)
- [Get an image frame](actions/get-image-frame.js#L9) (`GetImageFrame`)
- [Get data store properties](actions/get-datastore.js#L8) (`GetDatastore`)
- [Get image set properties](actions/get-image-set.js#L8) (`GetImageSet`)
- [Get import job properties](actions/get-dicom-import-job.js#L8) (`GetDICOMImportJob`)
- [Get metadata for an image set](actions/get-image-set-metadata.js#L8) (`GetImageSetMetadata`)
- [Import bulk data into a data store](actions/start-dicom-import-job.js#L8) (`StartDICOMImportJob`)
- [List data stores](actions/list-datastores.js#L8) (`ListDatastores`)
- [List image set versions](actions/list-image-set-versions.js#L8) (`ListImageSetVersions`)
- [List import jobs for a data store](actions/list-dicom-import-jobs.js#L8) (`ListDICOMImportJobs`)
- [List tags for a resource](actions/list-tags-for-resource.js#L8) (`ListTagsForResource`)
- [Remove a tag from a resource](actions/untag-resource.js#L8) (`UntagResource`)
- [Search image sets](actions/search-image-sets.js#L8) (`SearchImageSets`)
- [Update image set metadata](actions/update-image-set-metadata.js#L8) (`UpdateImageSetMetadata`)

### Scenarios

Code examples that show you how to accomplish a specific task by calling multiple
functions within the same service.

- [Tagging a data store](scenarios/tagging-datastores.js)
- [Tagging an image set](scenarios/tagging-imagesets.js)


<!--custom.examples.start-->
<!--custom.examples.end-->

## Run the examples

### Instructions

**Note**: All code examples are written in ECMAscript 6 (ES6). For guidelines on converting to CommonJS, see
[JavaScript ES6/CommonJS syntax](https://docs.aws.amazon.com/sdk-for-javascript/v3/developer-guide/sdk-examples-javascript-syntax.html).

**Run a single action**

```bash
node ./actions/<fileName>
```

**Run a scenario**
Most scenarios can be run with the following command:
```bash
node ./scenarios/<fileName>
```

<!--custom.instructions.start-->
<!--custom.instructions.end-->

#### Hello HealthImaging

This example shows you how to get started using HealthImaging.

```bash
node ./hello.js
```


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
in the `javascriptv3` folder.



<!--custom.tests.start-->
<!--custom.tests.end-->

## Additional resources

- [HealthImaging Developer Guide](https://docs.aws.amazon.com/healthimaging/latest/devguide/what-is.html)
- [HealthImaging API Reference](https://docs.aws.amazon.com/healthimaging/latest/APIReference/Welcome.html)
- [SDK for JavaScript (v3) HealthImaging reference](https://docs.aws.amazon.com/AWSJavaScriptSDK/v3/latest/client/medical-imaging)

<!--custom.resources.start-->
<!--custom.resources.end-->

---

Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.

SPDX-License-Identifier: Apache-2.0