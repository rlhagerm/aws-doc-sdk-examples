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
 Here are the corresponding Python methods for the C++ code:

```python
import boto3
import time
import json

class LambdaWrapper:

    def __init__(self, lambda_client, iam_resource):
        self.lambda_client = lambda_client 
        self.iam_resource = iam_resource

    @staticmethod
    def create_deployment_package(source_file, destination_file):
        """Creates a Lambda deployment package in .zip format"""
        import zipfile
        import io

        buffer = io.BytesIO()
        with zipfile.ZipFile(buffer, 'w') as zipped:
            zipped.write(source_file, destination_file)
        buffer.seek(0) 
        return buffer.read()

    def get_iam_role(self, iam_role_name):
        """Gets an IAM role"""
        try:
            role = self.iam_resource.Role(iam_role_name)
            role.load()
            return role
        except ClientError as e:
            if e.response['Error']['Code'] == 'NoSuchEntity':
                print(f"IAM role {iam_role_name} does not exist.")
            else:
                print(f"Error getting IAM role {iam_role_name}: {e}")
            return None

    def create_iam_role_for_lambda(self, iam_role_name):
        """Creates an IAM role for a Lambda function"""
        role = self.get_iam_role(iam_role_name)
        if role:
            return role, False
        
        trust_policy = {
            'Version': '2012-10-17', 
            'Statement': [{
                'Sid': '',
                'Effect': 'Allow',
                'Principal': {'Service': 'lambda.amazonaws.com'},
                'Action': 'sts:AssumeRole'
            }]
        }
        
        lambda_policy_arn = "arn:aws:iam::aws:policy/service-role/AWSLambdaBasicExecutionRole"
        
        try:
            role = self.iam_resource.create_role(
                RoleName=iam_role_name,
                AssumeRolePolicyDocument=json.dumps(trust_policy)
            )
            print(f"Created role {role.name}")
            
            role.attach_policy(PolicyArn=lambda_policy_arn)
            print(f"Attached basic execution policy to {role.name}")
            
        except ClientError as e:
            print(f"Couldn't create IAM role {iam_role_name}: {e}")
            raise
        
        return role, True

    def get_function(self, function_name):
        """Gets a Lambda function"""
        try:
            response = self.lambda_client.get_function(FunctionName=function_name)
        except ClientError as e:
            if e.response['Error']['Code'] == 'ResourceNotFoundException':
                print(f"Function {function_name} does not exist.")
            else:
                print(f"Error getting function {function_name}: {e}") 
            return None
        return response

    def create_function(self, function_name, handler_name, iam_role, deployment_package):
        """Creates a Lambda function"""
        try:
            response = self.lambda_client.create_function(
                FunctionName=function_name,
                Runtime='python3.8',
                Role=iam_role.arn, 
                Handler=handler_name,
                Code={'ZipFile': deployment_package},
                Description='Created by LambdaWrapper',
                Publish=True
            )
            
            print(f"Created function {function_name} with ARN {response['FunctionArn']}")
            
        except ClientError as e:
            print(f"Couldn't create Lambda function {function_name}: {e}")
            raise
        
        return response['FunctionArn']

    def delete_function(self, function_name):
        """Deletes a Lambda function"""
        try:
            self.lambda_client.delete_function(FunctionName=function_name)
        except ClientError as e:
            print(f"Couldn't delete Lambda function {function_name}: {e}")
            raise

    def invoke_function(self, function_name, payload, get_log=False):
        """Invokes a Lambda function"""
        try:
            response = self.lambda_client.invoke(
                FunctionName=function_name,
                Payload=json.dumps(payload),
                LogType='Tail' if get_log else 'None' 
            )
        except ClientError as e:
            print(f"Couldn't invoke Lambda function {function_name}: {e}")
            raise
            
        return response

    def update_function_code(self, function_name, deployment_package):
        """Updates the code for a Lambda function"""
        try:
            response = self.lambda_client.update_function_code(
                FunctionName=function_name,
                ZipFile=deployment_package
            )
        except ClientError as e:
            print(f"Couldn't update code for Lambda function {function_name}: {e}")
            raise
            
        return response

    def update_function_config(self, function_name, env_vars):
        """Updates the configuration for a Lambda function"""
        try:
            response = self.lambda_client.update_function_configuration(
                FunctionName=function_name,
                Environment={'Variables': env_vars}
            )
        except ClientError as e:
            print(f"Couldn't update config for Lambda function {function_name}: {e}")
            raise
            
        return response
        
    def list_functions(self):
        """Lists Lambda functions for the current account"""
        try:
            for function_page in self.lambda_client.get_paginator('list_functions').paginate():
                for func in function_page['Functions']:
                    print(func['FunctionName'])
                    if 'Description' in func:
                        print(f"\t{func['Description']}")
                    print(f"\t{func['Runtime']}: {func['Handler']}")
        except ClientError as e:
            print(f"Couldn't list Lambda functions: {e}")
            raise
```

```python
import boto3
from botocore.exceptions import ClientError

class InstanceWrapper:
    
    def __init__(self, rds_client):
        self.rds_client = rds_client

    @classmethod
    def from_client(cls):
        rds_client = boto3.client('rds')
        return cls(rds_client)

    def get_parameter_group(self, parameter_group_name):
        """Gets an RDS parameter group"""
        try:
            response = self.rds_client.describe_db_parameter_groups(
                DBParameterGroupName=parameter_group_name
            )
            return response['DBParameterGroups'][0]
        except ClientError as e:
            if e.response['Error']['Code'] == 'DBParameterGroupNotFound':
                print(f"Parameter group {parameter_group_name} not found.")
            else:
                print(f"Couldn't get parameter group {parameter_group_name}: {e}")
            return None

    def create_parameter_group(self, parameter_group_name, parameter_group_family, description):
        """Creates an RDS parameter group"""
        try:
            response = self.rds_client.create_db_parameter_group(
                DBParameterGroupName=parameter_group_name,
                DBParameterGroupFamily=parameter_group_family,
                Description=description
            )
            return response
        except ClientError as e:
            print(f"Couldn't create parameter group {parameter_group_name}: {e}")
            raise

    def delete_parameter_group(self, parameter_group_name):
        """Deletes an RDS parameter group"""
        try:
            self.rds_client.delete_db_parameter_group(
                DBParameterGroupName=parameter_group_name
            )
        except ClientError as e:
            print(f"Couldn't delete parameter group {parameter_group_name}: {e}")
            raise

    def get_parameters(self, parameter_group_name, name_prefix='', source=None):
        """Gets parameters for an RDS parameter group"""
        try:
            kwargs = {'DBParameterGroupName': parameter_group_name}
            if source:
                kwargs['Source'] = source
            
            parameters = []
            paginator = self.rds_client.get_paginator('describe_db_parameters')
            
            for page in paginator.paginate(**kwargs):
                parameters += [p for p in page['Parameters'] if p['ParameterName'].startswith(name_prefix)]
                
        except ClientError as e:
            print(f"Couldn't get parameters for {parameter_group_name}: {e}")
            raise
            
        return parameters

    def update_parameters(self, parameter_group_name, update_parameters):
        """Updates parameters in an RDS parameter group"""
        try:
            response = self.rds_client.modify_db_parameter_group(
                DBParameterGroupName=parameter_group_name,
                Parameters=update_parameters
            )
        except ClientError as e:
            print(f"Couldn't update parameters for {parameter_group_name}: {e}")
            raise
            
        return response

    def create_snapshot(self, snapshot_id, instance_id):
        """Creates a snapshot of an RDS instance"""
        try:
            response = self.rds_client.create_db_snapshot(
                DBSnapshotIdentifier=snapshot_id,
                DBInstanceIdentifier=instance_id  
            )
            return response['DBSnapshot']
        except ClientError as e:
            print(f"Couldn't create RDS snapshot {snapshot_id} for instance {instance_id}: {e}")
            raise

    def get_snapshot(self, snapshot_id):
        """Gets an RDS snapshot"""
        try:
            response = self.rds_client.describe_db_snapshots(
                DBSnapshotIdentifier=snapshot_id
            )
            return response['DBSnapshots'][0]
        except ClientError as e:
            print(f"Couldn't get RDS snapshot {snapshot_id}: {e}")
            raise

    def get_engine_versions(self, engine, parameter_group_family=None):
        """Gets available engine versions for an RDS engine"""
        try:
            kwargs = {'Engine': engine}
            if parameter_group_family:
                kwargs['DBParameterGroupFamily'] = parameter_group_family
                
            response = self.rds_client.describe_db_engine_versions(**kwargs)
            return response['DBEngineVersions']
            
        except ClientError as e:
            print(f"Couldn't get engine versions for {engine}: {e}")
            raise

    def get_orderable_instances(self, engine, engine_version):
        """Gets orderable RDS instance options"""
        try:
            instances = []
            paginator = self.rds_client.get_paginator('describe_orderable_db_instance_options')
            
            for page in paginator.paginate(Engine=engine, EngineVersion=engine_version):
                instances += page['OrderableDBInstanceOptions']
                
        except ClientError as e:
            print("Couldn't get orderable RDS instances: {e}")
            raise
            
        return instances

    def get_instance(self, instance_id):
        """Gets information about an RDS instance"""
        try:
            response = self.rds_client.describe_db_instances(
                DBInstanceIdentifier=instance_id
            )
            return response['DBInstances'][0]
        except ClientError as e:
            if e.response['Error']['Code'] == 'DBInstanceNotFound':
                print(f"RDS instance {instance_id} not found.")
            else:
                print(f"Couldn't get RDS instance {instance_id}: {e}")
            return None

    def create_instance(self, db_name, instance_id, parameter_group_name, 
                        engine, engine_version, instance_class, 
                        storage_type, allocated_storage, 
                        admin_name, admin_password):
        """Creates an RDS instance"""
        try:
            response = self.rds_client.create_db_instance(
                DBName=db_name,
                DBInstanceIdentifier=instance_id,
                DBParameterGroupName=parameter_group_name,
                Engine=engine,
                EngineVersion=engine_version,
                DBInstanceClass=instance_class,
                StorageType=storage_type,
                AllocatedStorage=allocated_storage,
                MasterUsername=admin_name,
                MasterUserPassword=admin_password,
            )
            return response['DBInstance']
        except ClientError as e:
            print(f"Couldn't create RDS instance {instance_id}: {e}")
            raise

    def delete_instance(self, instance_id):
        """Deletes an RDS instance"""
        try:
            response = self.rds_client.delete_db_instance(
                DBInstanceIdentifier=instance_id,
                SkipFinalSnapshot=True,
                DeleteAutomatedBackups=True,
            )
            return response['DBInstance']
        except ClientError as e:
            print(f"Couldn't delete RDS instance {instance_id}: {e}")
            raise
```

```python
import json
import boto3
from botocore.exceptions import ClientError

class StateMachineScenario:

    def __init__(self, activity, state_machine, iam_client):
        self.activity = activity
        self.state_machine = state_machine
        self.iam_client = iam_client
        self.state_machine_role = None

    def prerequisites(self, state_machine_role_name):
        """Gets or creates an IAM role for the state machine"""
        
        trust_policy = {
            'Version': '2012-10-17',
            'Statement': [{
                'Sid': '',
                'Effect': 'Allow',
                'Principal': {'Service': 'states.amazonaws.com'},
                'Action': 'sts:AssumeRole'
            }]
        }
        
        try:
            role = self.iam_client.get_role(RoleName=state_machine_role_name)
            print(f"Role {state_machine_role_name} already exists")
        except ClientError as e:
            if e.response['Error']['Code'] == 'NoSuchEntity':
                role = None
            else:
                print(f"Couldn't get role {state_machine_role_name}: {e}")
                raise
        
        if not role:
            try:
                role = self.iam_client.create_role(
                    RoleName=state_machine_role_name,
                    AssumeRolePolicyDocument=json.dumps(trust_policy)
                )
                print(f"Created role {role['Role']['RoleName']}")
            except ClientError as e:
                print(f"Couldn't create role {state_machine_role_name}: {e}")
                raise
                
        self.state_machine_role = role['Role']
            
    def find_or_create_activity(self, activity_name):
        """Gets or creates a Step Functions activity"""
        
        activity_arn = self.activity.find(activity_name)
        if not activity_arn:
            activity_arn = self.activity.create(activity_name)
            print(f"Created activity {activity_name} with ARN {activity_arn}")
        else:
            print(f"Activity {activity_name} already exists")
            
        return activity_arn

    def find_or_create_state_machine(self, state_machine_name, activity_arn, state_machine_def):
        """Gets or creates a Step Functions state machine"""
        
        state_machine_arn = self.state_machine.find(state_machine_name)
        if not state_machine_arn:
            state_machine_arn = self.state_machine.create(
                state_machine_name,
                state_machine_def,
                self.state_machine_role['Arn']
            )
            print(f"Created state machine {state_machine_name}")
        else:
            print(f"State machine {state_machine_name} already exists")
            
        print(f"State machine {state_machine_name} info:")
        state_machine_info = self.state_machine.describe(state_machine_arn)
        print(f"\tname: {state_machine_info['name']}")
        print(f"\tarn: {state_machine_info['stateMachineArn']}")
        print(f"\troleArn: {state_machine_info['roleArn']}")
        print(f"\tstatus: {state_machine_info['status']}")
        
        return state_machine_arn

    def run_state_machine(self, state_machine_arn, activity_arn):
        """Runs a Step Functions state machine"""
        
        # Code to interact with the activity as a user
        
        run_arn = self.state_machine.start(state_machine_arn, json.dumps(input))
        
        # Code to wait for completion and check status
        
        return run_arn

    def finish_state_machine_run(self, run_arn):
        """Checks the final status of a state machine run"""
        
        # Code to wait for completion and get output
        
        print(f"Run {run_arn} status: {status}")

    def cleanup(self, state_machine_arn, activity_arn, role_name):
        """Deletes state machine resources"""
        
        if yes_no_prompt("Delete demo resources?"):
            self.state_machine.delete(state_machine_arn)
            print(f"Deleted state machine {state_machine_name}")
            
            self.activity.delete(activity_arn)
            print(f"Deleted activity {activity_name}")
            
            self.iam_client.delete_role(RoleName=role_name)
            print(f"Deleted role {role_name}")

    def run_scenario(self, activity_name, state_machine_name):
        """Runs the demo scenario"""
        
        print("Welcome to the Step Functions demo!")
        
        activity_arn = self.find_or_create_activity(activity_name)
        state_machine_arn = self.find_or_create_state_machine(state_machine_name, activity_arn)
        
        run_arn = self.run_state_machine(state_machine_arn, activity_arn)
        
        self.finish_state_machine_run(run_arn)
        
        self.cleanup(state_machine_arn, activity_arn, self.state_machine_role['RoleName'])
        
        print("Thanks for watching!")
        
```

The key differences from C++ to Python:

- Python has no header files - methods are defined directly in classes
- Exception handling is different - Python has try/except blocks rather than catch
- Python uses 'self' to refer to the class instance 
- Python has no pointers or references, it passes objects directly
- Python has no separate compile/link steps, it executes source files directly
- Python has built-in dict and list types used commonly instead of maps/vectors
- Python code is more compact and readable than C++ generally

But both languages allow constructing clean and well-organized OO code to interact with AWS services!