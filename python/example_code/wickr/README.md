# AWS Wickr code examples for the SDK for Python

## Overview

Shows how to use the AWS SDK for Python (Boto3) with AWS Wickr to create secure communication networks.

*AWS Wickr is an end-to-end encrypted service that helps organizations and government agencies communicate securely through messaging, voice and video calling, file sharing, and more.*

## ⚠ Important

* Running this code might result in charges to your AWS account. For more details, see [AWS Pricing](https://aws.amazon.com/pricing/) and [Free Tier](https://aws.amazon.com/free/).
* Running the tests might result in charges to your AWS account.
* We recommend that you grant your code least privilege. At most, grant only the minimum permissions required to perform the task. For more information, see [Grant least privilege](https://docs.aws.amazon.com/IAM/latest/UserGuide/best-practices.html#grant-least-privilege).
* This code is not tested in every AWS Region. For more information, see [AWS Regional Services](https://aws.amazon.com/about-aws/global-infrastructure/regional-product-services).

## Code examples

### Scenarios

Code examples that show you how to accomplish a specific task by calling multiple functions within the same service.

- [Create and manage secure communication networks](scenario_wickr_basics.py)

## Prerequisites

- You must have an AWS account, and have your default credentials and AWS Region configured as described in the [AWS Tools and SDKs Shared Configuration and Credentials Reference Guide](https://docs.aws.amazon.com/credref/latest/refdocs/creds-config-files.html).
- Python 3.7 or later
- Boto3 1.26.0 or later
- PyTest 5.3.5 or later (to run unit tests)

## Cautions

- As an AWS best practice, grant this code least privilege, or only the permissions required to perform a task. For more information, see [Grant Least Privilege](https://docs.aws.amazon.com/IAM/latest/UserGuide/best-practices.html#grant-least-privilege) in the *AWS Identity and Access Management User Guide*.
- This code has not been tested in all AWS Regions. Some AWS services are available only in specific Regions. For more information, see the [AWS Regional Services List](https://aws.amazon.com/about-aws/global-infrastructure/regional-product-services/?p=ngi&loc=4) on the AWS website.
- Running this code might result in charges to your AWS account.

## Running the code

### Prerequisites

Before running the examples, ensure you have:

1. **AWS Credentials**: Configure your AWS credentials using one of these methods:
   - AWS credentials file
   - Environment variables (`AWS_ACCESS_KEY_ID`, `AWS_SECRET_ACCESS_KEY`)
   - IAM roles (for EC2 instances)
   - AWS CLI (`aws configure`)

2. **Required permissions**: Your AWS credentials must have permissions to:
   - Create, list, and delete Wickr networks
   - Create and manage Wickr users
   - Create and manage Wickr rooms
   - Send messages and upload files
   - Access audit logs

3. **Python dependencies**:
   ```bash
   pip install boto3
   ```

### Running the scenario

The scenario demonstrates the complete workflow of creating and managing a secure Wickr communication network:

```bash
python scenario_wickr_basics.py
```

This interactive scenario will guide you through:

1. **Network Setup**: Creating or using an existing Wickr network
2. **User Management**: Creating users with different roles (Admin/User)
3. **Room Creation**: Setting up secure messaging rooms
4. **Security Features**: Configuring message expiration and burn-on-read
5. **File Sharing**: Demonstrating secure file upload capabilities
6. **Audit & Compliance**: Reviewing audit logs and security settings
7. **Cleanup**: Option to delete all created resources

### Example usage

```python
import boto3
from wickr_wrapper import WickrWrapper

# Create a Wickr client and wrapper
wickr_client = boto3.client('wickr')
wickr_wrapper = WickrWrapper(wickr_client)

# Create a network
response = wickr_wrapper.create_network(
    "MySecureNetwork", 
    "Secure communication network"
)
network_id = response['NetworkId']

# Create a user
user = wickr_wrapper.create_user(
    network_id,
    "alice.smith",
    "alice@example.com",
    "Admin"
)

# Create a room
room = wickr_wrapper.create_room(
    network_id,
    "Project Alpha",
    "Secure project communications"
)

# Send a message
message = wickr_wrapper.send_message(
    room['RoomId'],
    "Welcome to the secure room!"
)
```

## Additional information

- [AWS Wickr Administration Guide](https://docs.aws.amazon.com/wickr/latest/adminguide/)
- [AWS Wickr User Guide](https://docs.aws.amazon.com/wickr/latest/userguide/)
- [AWS SDK for Python (Boto3) Documentation](https://boto3.amazonaws.com/v1/documentation/api/latest/index.html)

---

Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.

SPDX-License-Identifier: Apache-2.0
