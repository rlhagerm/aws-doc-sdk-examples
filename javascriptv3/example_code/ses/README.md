# Amazon SES code examples for the SDK for JavaScript (v3)

## Overview

Shows how to use the AWS SDK for JavaScript (v3) to work with Amazon Simple Email Service (Amazon SES).

<!--custom.overview.start-->
<!--custom.overview.end-->

_Amazon SES is a reliable, scalable, and cost-effective email service._

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

### Single actions

Code excerpts that show you how to call individual service functions.

- [Create a receipt filter](src/ses_createreceiptfilter.js#L15) (`CreateReceiptFilter`)
- [Create a receipt rule](src/ses_createreceiptrule.js#L16) (`CreateReceiptRule`)
- [Create a receipt rule set](src/ses_createreceiptruleset.js#L14) (`CreateReceiptRuleSet`)
- [Create an email template](src/ses_createtemplate.js#L15) (`CreateTemplate`)
- [Delete a receipt filter](src/ses_deletereceiptfilter.js#L15) (`DeleteReceiptFilter`)
- [Delete a receipt rule](src/ses_deletereceiptrule.js#L15) (`DeleteReceiptRule`)
- [Delete a rule set](src/ses_deletereceiptruleset.js#L15) (`DeleteReceiptRuleSet`)
- [Delete an email template](src/ses_deletetemplate.js#L14) (`DeleteTemplate`)
- [Delete an identity](src/ses_deleteidentity.js#L15) (`DeleteIdentity`)
- [Get an existing email template](src/ses_gettemplate.js#L14) (`GetTemplate`)
- [List email templates](src/ses_listtemplates.js#L14) (`ListTemplates`)
- [List identities](src/ses_listidentities.js#L14) (`ListIdentities`)
- [List receipt filters](src/ses_listreceiptfilters.js#L14) (`ListReceiptFilters`)
- [Send bulk templated email](src/ses_sendbulktemplatedemail.js#L15) (`SendBulkTemplatedEmail`)
- [Send email](src/ses_sendemail.js#L15) (`SendEmail`)
- [Send raw email](src/send-with-attachments.js#L8) (`SendRawEmail`)
- [Send templated email](src/ses_sendtemplatedemail.js#L15) (`SendTemplatedEmail`)
- [Update an email template](src/ses_updatetemplate.js#L14) (`UpdateTemplate`)
- [Verify a domain identity](src/ses_verifydomainidentity.js#L14) (`VerifyDomainIdentity`)
- [Verify an email identity](src/ses_verifyemailidentity.js#L15) (`VerifyEmailIdentity`)

### Cross-service examples

Sample applications that work across multiple AWS services.

- [Use Step Functions to invoke Lambda functions](../../example_code/cross-services/lambda-step-functions)


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



### Tests

⚠ Running tests might result in charges to your AWS account.


To find instructions for running these tests, see the [README](../../README.md#Tests)
in the `javascriptv3` folder.



<!--custom.tests.start-->
<!--custom.tests.end-->

## Additional resources

- [Amazon SES Developer Guide](https://docs.aws.amazon.com/ses/latest/dg/Welcome.html)
- [Amazon SES API Reference](https://docs.aws.amazon.com/ses/latest/APIReference/Welcome.html)
- [SDK for JavaScript (v3) Amazon SES reference](https://docs.aws.amazon.com/AWSJavaScriptSDK/v3/latest/client/ses)

<!--custom.resources.start-->
<!--custom.resources.end-->

---

Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.

SPDX-License-Identifier: Apache-2.0