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
 Here are the corresponding methods in Python:

```python
import io
import zipfile
from botocore.exceptions import ClientError
import logging

logger = logging.getLogger(__name__)

class LambdaWrapper:

    @staticmethod
    def create_deployment_package(source_file, destination_file):
        """
        Creates a Lambda deployment package in .zip format in an in-memory buffer. 
        This buffer can be passed directly to Lambda when creating the function.
        
        :param source_file: The name of the file that contains the Lambda handler
                            function.
        :param destination_file: The name to give the file when it's deployed to Lambda.
        :return: The deployment package as bytes.
        """
        buffer = io.BytesIO()
        with zipfile.ZipFile(buffer, "w") as zipped:
            zipped.write(source_file, destination_file)
        buffer.seek(0)
        return buffer.read()

    def get_iam_role(self, iam_role_name):
        """
        Gets an AWS IAM role by name.
        
        :param iam_role_name: The name of the role to retrieve.
        :return: The IAM role if found, else None.
        """
        try:
            role = self.iam_resource.Role(iam_role_name)
            role.load()
            logger.info("Got IAM role %s", role.name)
            return role
        except ClientError as error:
            if error.response["Error"]["Code"] == "NoSuchEntity":
                logger.info("IAM role %s does not exist.", iam_role_name)
            else:
                logger.error(
                    "Couldn't get IAM role %s. %s: %s", 
                    iam_role_name,
                    error.response["Error"]["Code"], 
                    error.response["Error"]["Message"]
                )
            return None

    def create_iam_role_for_lambda(self, iam_role_name):
        """
        Creates an IAM role for a Lambda function if it doesn't exist.
        
        :param iam_role_name: The name of the role to create.
        :return: The role and a value indicating if it was created.
        """
        role = self.get_iam_role(iam_role_name)
        if role is not None:
            return role, False
        
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
        
        basic_policy_arn = "arn:aws:iam::aws:policy/service-role/AWSLambdaBasicExecutionRole"
        
        try:
            role = self.iam_resource.create_role(
                RoleName=iam_role_name,
                AssumeRolePolicyDocument=json.dumps(trust_policy)
            )
            logger.info("Created role %s.", role.name)
            
            role.attach_policy(PolicyArn=basic_policy_arn)
            logger.info("Attached basic policy to role %s.", role.name)
            
        except ClientError as error:
            logger.error(
                "Couldn't create role %s or attach policy %s. %s: %s",
                iam_role_name,
                basic_policy_arn,
                error.response["Error"]["Code"], 
                error.response["Error"]["Message"]
            )
            raise
        
        return role, True

    def get_function(self, function_name):
        """
        Gets a Lambda function by name.
        
        :param function_name: The name of the function.
        :return: The function data if found, else None. 
        """
        try:
            response = self.lambda_client.get_function(FunctionName=function_name)
            return response
        except ClientError as error:
            if error.response["Error"]["Code"] == "ResourceNotFoundException":
                logger.info("Function %s does not exist.", function_name)
            else:
                logger.error(
                    "Couldn't get function %s. %s: %s",
                    function_name,
                    error.response["Error"]["Code"], 
                    error.response["Error"]["Message"]
                )
            return None

    # Other methods omitted for brevity
```

The Python code follows a similar style and structure to the C++ code, with some key differences:

- Uses built-in Python data types like bytes and dictionaries instead of C++ classes 
- Leverages Python's exception handling instead of return codes
- Lambda deployment package creation is simpler with Python's zipfile module
- Uses boto3 instead of the C++ SDK for AWS services
- Omits features not applicable to Python like move semantics

Let me know if you would like me to provide any of the other methods from the C++ code sample translated to Python!