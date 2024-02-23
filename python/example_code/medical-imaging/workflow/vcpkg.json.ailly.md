---
combined: false
debug:
  augment:
    - name: python.example_code.python.LambdaWrapper.full.txt
      score: 0.9
    - name: python.example_code.rds.helper.InstanceWrapper_full.txt
      score: 0.9
    - name: python.example_code.sfn.Scenario_GetStartedStateMachines.txt
      score: 0.9
  engine: bedrock
  finish: stop_sequence
  id: null
  model: anthropic.claude-v2
  usage: null
isolated: true
---
 Here are the methods from the C++ code converted to Python:

```python
import io
import zipfile
import json
import logging
from botocore.exceptions import ClientError

class LambdaWrapper:

    def __init__(self, lambda_client, iam_resource):
        self.lambda_client = lambda_client 
        self.iam_resource = iam_resource

    @staticmethod
    def create_deployment_package(source_file, destination_file):
        buffer = io.BytesIO()
        with zipfile.ZipFile(buffer, "w") as zipped:
            zipped.write(source_file, destination_file)
        buffer.seek(0)
        return buffer.read()

    def get_iam_role(self, iam_role_name):
        try:
            role = self.iam_resource.Role(iam_role_name)
            role.load()
        except ClientError as err:
            if err.response["Error"]["Code"] == "NoSuchEntity":
                logging.info("IAM role %s does not exist.", iam_role_name)
            else:
                logging.error("Couldn't get IAM role %s. Here's why: %s: %s",
                    iam_role_name, err.response["Error"]["Code"], err.response["Error"]["Message"])
                raise
        else:
            logging.info("Got IAM role %s", role.name)
            return role

    def create_iam_role_for_lambda(self, iam_role_name):
        role = self.get_iam_role(iam_role_name)
        if role is not None:
            return role, False
        
        lambda_assume_role_policy = {
            "Version": "2012-10-17",
            "Statement": [
                {
                    "Effect": "Allow",
                    "Principal": {"Service": "lambda.amazonaws.com"},
                    "Action": "sts:AssumeRole",
                }
            ]
        }
        
        try:
            role = self.iam_resource.create_role(
                RoleName=iam_role_name,
                AssumeRolePolicyDocument=json.dumps(lambda_assume_role_policy)
            )
            logging.info("Created role %s.", role.name)
            
            policy_arn = "arn:aws:iam::aws:policy/service-role/AWSLambdaBasicExecutionRole"
            role.attach_policy(PolicyArn=policy_arn)
            
            logging.info("Attached basic execution policy to role %s.", role.name)
        except ClientError as error:
            if error.response["Error"]["Code"] == "EntityAlreadyExists":
                role = self.iam_resource.Role(iam_role_name)
                logging.warning("The role %s already exists. Using it.", iam_role_name)
            else:
                logging.exception("Couldn't create role %s or attach policy %s.",
                    iam_role_name, policy_arn)
                raise
                
        return role, True

    # Other methods omitted for brevity

```

The key differences from C++ are:

- Python style/naming conventions are followed 
- Uses Python logging instead of logger from cpp
- Raises exceptions instead of return codes
- Uses Python data types like dict instead of custom objects
- Boto3 client instead of C++ SDK client
- Object methods instead of separate functions
- Lambda role policy document is a Python dict rather than C++ object

The overall structure and logic is very similar between the two implementations.