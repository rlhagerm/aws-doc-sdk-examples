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
 Here are the main methods from the C++ code translated to Python:

```python
import boto3
import json
from botocore.exceptions import ClientError

class LambdaWrapper:

    def __init__(self, lambda_client): 
        self.lambda_client = lambda_client

    def get_iam_role(self, iam_role_name):
        iam = boto3.resource('iam')
        try:
            role = iam.Role(iam_role_name)
            role.load()
            print(f"Got IAM role {role.name}")
            return role
        except ClientError as e:
            if e.response['Error']['Code'] == 'NoSuchEntity':
                print(f"IAM role {iam_role_name} does not exist.")
            else:
                raise

    def create_iam_role_for_lambda(self, iam_role_name):
        iam = boto3.resource('iam')
        trust_policy = {
            "Version": "2012-10-17",
            "Statement": [
                {
                    "Effect": "Allow",
                    "Principal": {"Service": "lambda.amazonaws.com"},
                    "Action": "sts:AssumeRole"
                }
            ]
        }
        
        try:
            role = iam.create_role(
                RoleName=iam_role_name,
                AssumeRolePolicyDocument=json.dumps(trust_policy)
            )
            print(f"Created role {role.name}")
            
            policy_arn = "arn:aws:iam::aws:policy/service-role/AWSLambdaBasicExecutionRole" 
            role.attach_policy(PolicyArn=policy_arn)
            
            print(f"Attached basic execution policy to role {role.name}")
            return role
        except ClientError as e:
            if e.response['Error']['Code'] == 'EntityAlreadyExists':
                print(f"Role {iam_role_name} already exists, using it")
                role = iam.Role(iam_role_name)
            else:
                raise

    def create_function(self, function_name, handler_name, role, zip_file):
        try:
            self.lambda_client.create_function(
                FunctionName=function_name,
                Runtime='python3.8',
                Role=role.arn, 
                Handler=handler_name,
                Code={'ZipFile': zip_file},
                Description='Created by example'
            )
            print(f"Created function {function_name}")
        except ClientError as e:
            print(f"Couldn't create function {function_name}")
            raise

    def invoke_function(self, function_name, payload):
        try:
            response = self.lambda_client.invoke(
                FunctionName=function_name,
                Payload=json.dumps(payload)
            )
            print(f"Invoked function {function_name}")
            return response
        except ClientError as e:
            print(f"Couldn't invoke function {function_name}")
            raise

    # Other methods

```

The key differences from C++:
- Uses boto3 library instead of C++ SDK
- Uses Python exception handling instead of error codes
-snake_case method naming convention instead of camelCase
- Resource objects instead of passing ARNs 
- Python data types instead of C++ ones
- Print statements for logging instead of logger objects

But overall very similar in structure and logic. The boto3 Python library makes the transition fairly straightforward.