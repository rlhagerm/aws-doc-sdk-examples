# AWS Control Tower Basics Scenario - Technical specification

This document contains the technical specifications for _AWS Control Tower Basics Scenario_,
a basics scenario that showcases AWS services and SDKs. It is primarily intended for the AWS code
examples team to use while developing this example in additional languages.

This document explains the following:

- Architecture and features of the example scenario.
- Metadata information for the scenario.
- Sample reference output.

For an introduction, see the [README.md](README.md).

---

### Table of contents

- [Resources and User Input](#resources-and-user-input)
- [Hello](#hello)
- [Scenario](#scenario)
- [Errors](#errors)
- [Metadata](#metadata)

## Resources and User Input

- The resources in this example are either created in the code, or using CloudFormation stack to set up the following resources:
  - An organization with 2 shared accounts 
  - IAM roles as specified in the tutorial

### Hello
The Hello example is a separate runnable example.

- Set up the service client.
- List available Baselines by name.

Example
```
Hello, AWS Control Tower! Let's list available baselines:

7 baseline(s) retrieved.
        AuditBaseline
        LogArchiveBaseline
        IdentityCenterBaseline
        BackupCentralVaultBaseline
        BackupAdminBaseline
        BackupBaseline

```
## Scenario

#### Setup
Deploy the Cloud Formation stack.

The CloudFormation stack creation should return the account IDs.
The Stack creation should prompt the user for logging and security account emails, and a name for the stack.

Example:
```
TODO
```

#### Landing Zone

Based on https://docs.aws.amazon.com/controltower/latest/userguide/walkthrough-api-setup.html

- Use the account IDs to generate a Landing Zone manifest file.
- Set up the Landing Zone with the manifest file, and capture the ARN and operation identifier.
- Poll for the status of the Landing Zone, until the operation is complete.

Example
```
TODO

```

#### Baseline

Based on https://docs.aws.amazon.com/controltower/latest/userguide/walkthrough-baseline-steps.html

- Update Landing Zone settings to enable the IdentityCenter Baseline (this will mean OUs must be re-registered)
- Use the ARNs of the OU to get the enabled baseline ARN
- Reset the enabled baseline for the OU


Example
```
TODO

```

#### Controls

Based on https://docs.aws.amazon.com/controltower/latest/controlreference/control-api-examples-short.html

- List Controls in Control Catalog
- Enable a Control
- Get the operational status of the Control
- Disable the Control

Example
```
TODO

```

- Cleanup
  - Clean up resources
  - Delete the Landing Zone
  - Delete the CloudFormation stack

Example:

```
TODO

```

---

## Errors
The following errors are handled in the Control Tower wrapper class:

| action                   | Error                     | Handling                                                                    |
|--------------------------|---------------------------|-----------------------------------------------------------------------------|
| `ListBaselines`          | AccessDeniedException     | Notify the user of insufficient permissions and exit.                       |
| `EnableBaseline`         | ValidationException       | Handle case where baseline is already enabled and return None.              |
| `ListControls`           | AccessDeniedException     | Notify the user of insufficient permissions and exit.                       |
| `EnableControl`          | ValidationException       | Handle case where control is already enabled and return None.               |
| `GetControlOperation`    | ResourceNotFound          | Notify the user that the control operation was not found.                   |
| `DisableControl`         | ResourceNotFound          | Notify the user that the control was not found.                             | 
| `ListLandingZones`       | AccessDeniedException     | Notify the user of insufficient permissions and exit.                       |


---

## Metadata

| action / scenario               | metadata file              | metadata key                         |
|---------------------------------|----------------------------|--------------------------------------|
| `ListBaselines`                 | controltower_metadata.yaml | controltower_Hello                   |
| `ListBaselines`                 | controltower_metadata.yaml | controltower_ListBaselines           |
| `EnableBaseline`                | controltower_metadata.yaml | controltower_EnableBaseline          |
| `EnableControl`                 | controltower_metadata.yaml | controltower_EnableControl           |
| `GetControlOperation`           | controltower_metadata.yaml | controltower_GetControlOperation     |
| `DisableControl`                | controltower_metadata.yaml | controltower_DisableControl          |
| `ListLandingZones`              | controltower_metadata.yaml | controltower_ListLandingZones        |
| `Control Tower Basics Scenario` | controltower_metadata.yaml | controltower_Scenario                |

