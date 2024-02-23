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
 Here are the corresponding methods translated from C++ to Python for the LambdaWrapper class:

```python
import io
import zipfile
import json
import logging
import boto3
from botocore.exceptions import ClientError

class LambdaWrapper:

    def __init__(self, lambda_client, iam_resource):
        self.lambda_client = lambda_client
        self.iam_resource = iam_resource

    @staticmethod
    def create_deployment_package(source_file, destination_file):
        """Creates a Lambda deployment package in .zip format"""
        buffer = io.BytesIO()
        with zipfile.ZipFile(buffer, "w") as zipped:
            zipped.write(source_file, destination_file)
        buffer.seek(0)
        return buffer.read()

    def get_iam_role(self, iam_role_name):
        """Gets an IAM role"""
        try:
            role = self.iam_resource.Role(iam_role_name)
            role.load()
        except ClientError as e:
            if e.response["Error"]["Code"] == "NoSuchEntity":
                logging.info("IAM role %s does not exist.", iam_role_name)
            else:
                logging.error("Couldn't get IAM role %s. %s: %s", 
                    iam_role_name, e.response["Error"]["Code"], e.response["Error"]["Message"])
                raise 
        else:
            logging.info("Got IAM role %s", role.name)
            return role

    def create_iam_role_for_lambda(self, iam_role_name):
        """Creates an IAM role for Lambda"""
        role = self.get_iam_role(iam_role_name)
        if role:
            return role, False
        
        trust_policy = {
            "Version": "2012-10-17",
            "Statement": [{
                "Effect": "Allow",
                "Principal": {"Service": "lambda.amazonaws.com"},
                "Action": "sts:AssumeRole"
            }]
        }
        
        policy_arn = "arn:aws:iam::aws:policy/service-role/AWSLambdaBasicExecutionRole"   
        
        try:
            role = self.iam_resource.create_role(
                RoleName=iam_role_name,
                AssumeRolePolicyDocument=json.dumps(trust_policy)
            )
            logging.info("Created role %s.", role.name)
            
            role.attach_policy(PolicyArn=policy_arn)
            logging.info("Attached basic execution policy to role %s.", role.name)
        except ClientError as e:
            logging.error("Couldn't create role %s or attach policy %s. %s: %s",
                iam_role_name, policy_arn, e.response["Error"]["Code"], e.response["Error"]["Message"])
            raise
        else:
            return role, True

    def get_function(self, function_name):
        """Gets a Lambda function"""
        try:
            response = self.lambda_client.get_function(FunctionName=function_name)
        except ClientError as e:
            if e.response["Error"]["Code"] == "ResourceNotFoundException":
                logging.info("Function %s does not exist.", function_name)
            else:
                logging.error("Couldn't get function %s. %s: %s",
                    function_name, e.response["Error"]["Code"], e.response["Error"]["Message"])
                raise
        else:
            return response

    def create_function(self, function_name, handler_name, iam_role, deployment_package):
        """Creates a Lambda function"""
        try:
            response = self.lambda_client.create_function(
                FunctionName=function_name,
                Description="AWS Lambda doc example",
                Runtime="python3.8",
                Role=iam_role.arn,
                Handler=handler_name,
                Code={"ZipFile": deployment_package},
                Publish=True
            )
            logging.info("Created function '%s' with ARN '%s'", 
                function_name, response["FunctionArn"])
        except ClientError:
            logging.error("Couldn't create function %s.", function_name)
            raise
        else:
            return response["FunctionArn"]

    def delete_function(self, function_name):
        """Deletes a Lambda function"""
        try:
            self.lambda_client.delete_function(FunctionName=function_name)
        except ClientError:
            logging.exception("Couldn't delete function %s.", function_name)
            raise

    def invoke_function(self, function_name, function_params, get_log=False):
        """Invokes a Lambda function"""
        try:
            response = self.lambda_client.invoke(
                FunctionName=function_name,
                Payload=json.dumps(function_params),
                LogType="Tail" if get_log else "None")
            logging.info("Invoked function %s.", function_name)
        except ClientError:
            logging.exception("Couldn't invoke function %s.", function_name)
            raise
        else:
            return response

    def update_function_code(self, function_name, deployment_package):
        """Updates the code for a Lambda function"""
        try:
            response = self.lambda_client.update_function_code(
                FunctionName=function_name,
                ZipFile=deployment_package
            )
        except ClientError as e:
            logging.error("Couldn't update function %s. %s: %s",
                function_name, e.response["Error"]["Code"], e.response["Error"]["Message"])
            raise
        else:
            return response

    def update_function_configuration(self, function_name, env_vars):
        """Updates the environment variables for a Lambda function"""
        try:
            response = self.lambda_client.update_function_configuration(
                FunctionName=function_name,
                Environment={"Variables": env_vars}
            )
        except ClientError as e:
            logging.error("Couldn't update function configuration %s. %s: %s",
                function_name, e.response["Error"]["Code"], e.response["Error"]["Message"])
            raise
        else:
            return response

    def list_functions(self):
        """Lists Lambda functions"""
        try:
            func_paginator = self.lambda_client.get_paginator("list_functions")
            for page in func_paginator.paginate():
                for func in page["Functions"]:
                    print(func["FunctionName"])
                    desc = func.get("Description")
                    if desc:
                        print(f"\t{desc}")
                    print(f"\t{func['Runtime']}: {func['Handler']}")
        except ClientError as e:
            logging.error("Couldn't list functions. %s: %s", 
                e.response["Error"]["Code"], e.response["Error"]["Message"])
            raise
```

And here are the corresponding methods for the InstanceWrapper class:

```python
import logging
import boto3
from botocore.exceptions import ClientError

class InstanceWrapper:

    def __init__(self, rds_client):
        self.rds_client = rds_client

    @classmethod
    def from_client(cls):
        rds_client = boto3.client("rds")
        return cls(rds_client)

    def get_parameter_group(self, parameter_group_name):
        """Gets a DB parameter group"""
        try:
            response = self.rds_client.describe_db_parameter_groups(
                DBParameterGroupName=parameter_group_name
            )
            parameter_group = response["DBParameterGroups"][0]
        except ClientError as e:
            if e.response["Error"]["Code"] == "DBParameterGroupNotFound":
                logging.info("Parameter group %s does not exist.", parameter_group_name)
            else:
                logging.error("Couldn't get parameter group %s. %s: %s",
                    parameter_group_name, e.response["Error"]["Code"], e.response["Error"]["Message"])
                raise
        else:
            return parameter_group

    def create_parameter_group(self, parameter_group_name, parameter_group_family, description):
        """Creates a DB parameter group"""
        try:
            response = self.rds_client.create_db_parameter_group(
                DBParameterGroupName=parameter_group_name,
                DBParameterGroupFamily=parameter_group_family,
                Description=description
            )
        except ClientError as e:
            logging.error("Couldn't create parameter group %s. %s: %s",
                parameter_group_name, e.response["Error"]["Code"], e.response["Error"]["Message"])
            raise
        else:
            return response

    def delete_parameter_group(self, parameter_group_name):
        """Deletes a DB parameter group"""
        try:
            self.rds_client.delete_db_parameter_group(
                DBParameterGroupName=parameter_group_name
            )
        except ClientError as e:
            logging.error("Couldn't delete parameter group %s. %s: %s",
                parameter_group_name, e.response["Error"]["Code"], e.response["Error"]["Message"])
            raise

    def get_parameters(self, parameter_group_name, name_prefix="", source=None):
        """Gets parameters in a DB parameter group"""
        try:
            kwargs = {"DBParameterGroupName": parameter_group_name}
            if source:
                kwargs["Source"] = source
                
            parameters = []
            paginator = self.rds_client.get_paginator("describe_db_parameters")
            for page in paginator.paginate(**kwargs):
                parameters += [p for p in page["Parameters"] if p["ParameterName"].startswith(name_prefix)]
        except ClientError as e:
            logging.error("Couldn't get parameters for %s. %s: %s",
                parameter_group_name, e.response["Error"]["Code"], e.response["Error"]["Message"])
            raise
        else:
            return parameters

    def update_parameters(self, parameter_group_name, update_parameters):
        """Updates parameters in a DB parameter group"""
        try:
            response = self.rds_client.modify_db_parameter_group(
                DBParameterGroupName=parameter_group_name,
                Parameters=update_parameters
            )
        except ClientError as e:
            logging.error("Couldn't update parameters in %s. %s: %s",
                parameter_group_name, e.response["Error"]["Code"], e.response["Error"]["Message"])
            raise
        else:
            return response

    def create_snapshot(self, snapshot_id, instance_id):
        """Creates a snapshot of a DB instance"""
        try:
            response = self.rds_client.create_db_snapshot(
                DBSnapshotIdentifier=snapshot_id,
                DBInstanceIdentifier=instance_id
            )
            snapshot = response["DBSnapshot"]
        except ClientError as e:
            logging.error("Couldn't create snapshot of %s. %s: %s",
                instance_id, e.response["Error"]["Code"], e.response["Error"]["Message"])
            raise
        else:
            return snapshot

    def get_snapshot(self, snapshot_id):
        """Gets a DB snapshot"""
        try:
            response = self.rds_client.describe_db_snapshots(
                DBSnapshotIdentifier=snapshot_id
            )
            snapshot = response["DBSnapshots"][0]
        except ClientError as e:
            logging.error("Couldn't get snapshot %s. %s: %s", 
                snapshot_id, e.response["Error"]["Code"], e.response["Error"]["Message"])
            raise
        else:
            return snapshot

    def get_engine_versions(self, engine, parameter_group_family=None):
        """Gets available engine versions for an engine and parameter group""" 
        try:
            kwargs = {"Engine": engine}
            if parameter_group_family:
                kwargs["DBParameterGroupFamily"] = parameter_group_family

            response = self.rds_client.describe_db_engine_versions(**kwargs)
            versions = response["DBEngineVersions"]
        except ClientError as e:
            logging.error("Couldn't get engine versions for %s. %s: %s",
                engine, e.response["Error"]["Code"], e.response["Error"]["Message"])
            raise
        else:
            return versions

    def get_orderable_instances(self, db_engine, db_engine_version):
        """Gets orderable DB instances for an engine and engine version"""
        try:
            inst_opts = []
            paginator = self.rds_client.get_paginator("describe_orderable_db_instance_options")
            for page in paginator.paginate(Engine=db_engine, EngineVersion=db_engine_version):
                inst_opts += page["OrderableDBInstanceOptions"]
        except ClientError as e:
            logging.error("Couldn't get orderable DB instances. %s: %s", 
                e.response["Error"]["Code"], e.response["Error"]["Message"])
            raise
        else:
            return inst_opts

    def get_db_instance(self, instance_id):
        """Gets a DB instance"""
        try:
            response = self.rds_client.describe_db_instances(
                DBInstanceIdentifier=instance_id
            )
            db_inst = response["DBInstances"][0]
        except ClientError as e:
            if e.response["Error"]["Code"] == "DBInstanceNotFound":
                logging.info("Instance %s does not exist.", instance_id)
            else:
                logging.error("Couldn't get DB instance %s. %s: %s",
                    instance_id, e.response["Error"]["Code"], e.response["Error"]["Message"])
                raise
        else:
            return db_inst

    def create_db_instance(self, db_name, instance_id, parameter_group_name, db_engine, 
        db_engine_version, instance_class, storage_type, allocated_storage, admin_name, admin_password):
        """Creates a DB instance"""
        try:
            response = self.rds_client.create_db_instance(
                DBName=db_name,
                DBInstanceIdentifier=instance_id,
                DBParameterGroupName=parameter_group_name,
                Engine=db_engine,
                EngineVersion=db_engine_version,
                DBInstanceClass=instance_class,
                StorageType=storage_type,
                AllocatedStorage=allocated_storage,
                MasterUsername=admin_name,
                MasterUserPassword=admin_password,
            )
            db_inst = response["DBInstance"]
        except ClientError as e:
            logging.error("Couldn't create DB instance %s. %s: %s",
                instance_id, e.response["Error"]["Code"], e.response["Error"]["Message"])
            raise
        else:
            return db_inst

    def delete_db_instance(self, instance_id):
        """Deletes a DB instance"""
        try:
            response = self.rds_client.delete_db_instance(
                DBInstanceIdentifier=instance_id,
                SkipFinalSnapshot=True,
                DeleteAutomatedBackups=True
            )
            db_inst = response["DBInstance"]
        except ClientError as e:
            logging.error("Couldn't delete DB instance %s. %s: %s",
                instance_id, e.response["Error"]["Code"], e.response["Error"]["Message"])
            raise
        else:
            return db_inst
```

And finally, here are the corresponding methods for the StateMachineScenario class:

```python
import json
import logging
import boto3
from botocore.exceptions import ClientError

class StateMachineScenario:

    def __init__(self, activity, state_machine, iam_client):
        self.activity = activity 
        self.state_machine = state_machine
        self.iam_client = iam_client

    def prerequisites(self, state_machine_role_name):
        """Creates an IAM role for the state machine if needed"""
        trust_policy = {
            "Version": "2012-10-17", 
            "Statement": [{
                "Sid": "",
                "Effect": "Allow",
                "Principal": {"Service": "states.amazonaws.com"},
                "Action": "sts:AssumeRole"
            }]
        }
        
        try:
            role = self.iam_client.get_role(RoleName=state_machine_role_name)
        except ClientError as e:
            if e.response["Error"]["Code"] == "NoSuchEntity":
                role = None
            else:
                logging.error("Couldn't get role %s. %s: %s",
                    state_machine_role_name, e.response["Error"]["Code"], e.response["Error"]["Message"])
                raise
        
        if not role:
            try:
                role = self.iam_client.create_role(
                    RoleName=state_machine_role_name,
                    AssumeRolePolicyDocument=json.dumps(trust_policy)
                )
                logging.info("Created role %s.", role.name)
            except ClientError as e:
                logging.error("Couldn't create role %s. %s: %s",
                    state_machine_role_name, e.response["Error"]["Code"], e.response["Error"]["Message"])
                raise
        
        self.state_machine_role = role["Role"]

    def find_or_create_activity(self, activity_name):
        """Finds or creates a Step Functions activity"""
        activity_arn = self.activity.find(activity_name)
        if not activity_arn:
            activity_arn = self.activity.create(activity_name)
            print(f"Created activity {activity_name} with ARN {activity_arn}")
        else:
            print(f"Activity {activity_name} already exists")
        return activity_arn

    def find_or_create_state_machine(self, state_machine_name, activity_arn, state_machine_file):
        """Finds or creates a Step Functions state machine"""
        state_machine_arn = self.state_machine.find(state_machine_name)
        if not state_machine_arn:
            with open(state_machine_file) as f:
                state_machine_def = f.read().replace("{{ACTIVITY_ARN}}", activity_arn)
            state_machine_arn = self.state_machine.create(
                state_machine_name, state_machine_def, self.state_machine_role["Arn"])
            print(f"Created state machine {state_machine_name}")
        else:
            print(f"State machine {state_machine_name} already exists")
        
        print(f"State machine {state_machine_name} info:")
        state_machine_info = self.state_machine.describe(state_machine_arn)
        print(f"\tname: {state_machine_info['name']}")
        print(f"\tstatus: {state_machine_info['status']}")
        print(f"\tstateMachineArn: {state_machine_info['stateMachineArn']}")
        print(f"\troleArn: {state_machine_info['roleArn']}")
        
        return state_machine_arn

    def run_state_machine(self, state_machine_arn, activity_arn):
        """Runs the state machine"""
        user_name = input("What is your name? ")
        run_input = {"name": user_name}
        
        print("Starting state machine...")
        run_arn = self.state_machine.start(state_machine_arn, json.dumps(run_input))
        
        action = None
        while action != "done":
            activity_task = self.activity.get_task(activity_arn)
            task_input = json.loads(activity_task["input"])
            
            print(f"ChatBot: {task_input['message']}")
            action = input("What now? ")
            task_response = {"action": action}
            
            self.activity.send_task_success(
                activity_task["taskToken"], json.dumps(task_response)
            )

        return run_arn

    def finish_state_machine_run(self, run_arn):
        """Prints final output for a state machine run"""
        print("Getting final state machine output...")
        status = "RUNNING"
        while status == "RUNNING":
            run_output = self.state_machine.describe_run(run_arn)
            status = run_output["status"]
            if status == "SUCCEEDED":
                print(f"ChatBot: {json.loads(run_output['output'])['message']}")
            else:
                print(f"Run status: {status}")

    def cleanup(self, state_machine_name, state_machine_arn, activity_name, activity_arn, role_name):
        """Deletes resources created for the demo"""
        if input("Delete resources created (y/n)? ").lower().startswith("y"):
            self.state_machine.delete(state_machine_arn)
            print(f"Deleted state machine {state_machine_name}")
            self.activity.delete(activity_arn)
            print(f"Deleted activity {activity_name}") 
            self.iam_client.delete_role(RoleName=role_name)
            print(f"Deleted role {role_name}")
```