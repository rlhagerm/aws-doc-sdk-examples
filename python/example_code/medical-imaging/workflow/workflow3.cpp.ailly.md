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
import boto3
import logging
from botocore.exceptions import ClientError

logger = logging.getLogger(__name__)

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
            logger.info("Got IAM role %s", role.name)
            return role
        except ClientError as e:
            if e.response['Error']['Code'] == 'NoSuchEntity':
                logger.info("IAM role %s does not exist.", iam_role_name)
            else:
                logger.error("Couldn't get IAM role %s. Here's why: %s: %s",
                             iam_role_name, e.response['Error']['Code'], e.response['Error']['Message'])
                raise

    def create_iam_role_for_lambda(self, iam_role_name):
        """Creates an IAM role for a Lambda function"""
        trust_policy = {
            'Version': '2012-10-17',
            'Statement': [{
                'Sid': '',
                'Effect': 'Allow',
                'Principal': {'Service': 'lambda.amazonaws.com'},
                'Action': 'sts:AssumeRole'
            }]
        }
        policy_arn = "arn:aws:iam::aws:policy/service-role/AWSLambdaBasicExecutionRole"
        
        try:
            role = self.iam_resource.create_role(RoleName=iam_role_name,
                                                AssumeRolePolicyDocument=json.dumps(trust_policy))
            logger.info("Created role %s.", role.name)

            role.attach_policy(PolicyArn=policy_arn)
            logger.info("Attached basic execution policy to role %s.", role.name)
        
        except ClientError as e:
            if e.response['Error']['Code'] == 'EntityAlreadyExists':
                role = self.iam_resource.Role(iam_role_name) 
                logger.warning("The role %s already exists. Using it.", iam_role_name)
            else:
                logger.exception("Couldn't create role %s or attach policy %s.", iam_role_name, policy_arn)
                raise
        
        return role

    def get_function(self, function_name):
        """Gets a Lambda function"""
        try:
            response = self.lambda_client.get_function(FunctionName=function_name)
        except ClientError as e:
            if e.response['Error']['Code'] == 'ResourceNotFoundException':
                logger.info("Function %s does not exist.", function_name)
            else:
                logger.error("Couldn't get function %s. Here's why: %s: %s", 
                             function_name, e.response['Error']['Code'], e.response['Error']['Message'])
                raise
        return response

    def create_function(self, function_name, handler_name, iam_role, deployment_package):
        """Creates a Lambda function"""
        try:
            response = self.lambda_client.create_function(
                FunctionName = function_name,
                Description = "AWS Lambda doc example",
                Runtime = "python3.8",
                Role = iam_role.arn, 
                Handler = handler_name,
                Code = {'ZipFile': deployment_package},
                Publish = True
            )
            
            function_arn = response['FunctionArn']
            logger.info("Created function '%s' with ARN '%s'", function_name, function_arn)
            
            waiter = self.lambda_client.get_waiter('function_active_v2')
            waiter.wait(FunctionName=function_name)
            
        except ClientError:
            logger.error("Couldn't create function %s.", function_name)
            raise
        else:
            return function_arn

    def delete_function(self, function_name):
        """Deletes a Lambda function"""
        try:
            self.lambda_client.delete_function(FunctionName=function_name)
        except ClientError:
            logger.exception("Couldn't delete function %s.", function_name)
            raise

    def invoke_function(self, function_name, function_params, get_log=False):
        """Invokes a Lambda function"""
        try:
            response = self.lambda_client.invoke(
                FunctionName = function_name,
                Payload = json.dumps(function_params),
                LogType = 'Tail' if get_log else 'None'
            )
            logger.info("Invoked function %s.", function_name)
        except ClientError:
            logger.exception("Couldn't invoke function %s.", function_name)
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
            logger.error("Couldn't update function %s. Here's why: %s: %s",
                         function_name, e.response['Error']['Code'], e.response['Error']['Message'])
            raise
        else:
            return response

    def update_function_configuration(self, function_name, env_vars):
        """Updates the environment variables for a Lambda function"""
        try:
            response = self.lambda_client.update_function_configuration(
                FunctionName=function_name,
                Environment={'Variables': env_vars}
            )
        except ClientError as e:
            logger.error("Couldn't update function configuration %s. Here's why: %s: %s",
                         function_name, e.response['Error']['Code'], e.response['Error']['Message'])
            raise
        else:
            return response

    def list_functions(self):
        """Lists the Lambda functions for the current account""" 
        try:
            func_paginator = self.lambda_client.get_paginator('list_functions')
            for func_page in func_paginator.paginate():
                for func in func_page['Functions']:
                    print(func['FunctionName'])
                    desc = func.get('Description')
                    if desc:
                        print(f"\t{desc}")
                    print(f"\t{func['Runtime']}: {func['Handler']}")
        except ClientError as e:
            logger.error("Couldn't list functions. Here's why: %s: %s", 
                         e.response['Error']['Code'], e.response['Error']['Message'])
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
        try:
            response = self.rds_client.describe_db_parameter_groups(
                DBParameterGroupName=parameter_group_name
            )
            parameter_group = response['DBParameterGroups'][0]
        except ClientError as e:
            if e.response['Error']['Code'] == 'DBParameterGroupNotFound':
                logger.info("Parameter group %s does not exist.", parameter_group_name)
            else:
                logger.error("Couldn't get parameter group %s. Here's why: %s: %s",
                             parameter_group_name, e.response['Error']['Code'], e.response['Error']['Message'])
                raise
        else:
            return parameter_group

    def create_parameter_group(self, parameter_group_name, parameter_group_family, description):
        try:
            response = self.rds_client.create_db_parameter_group(
                DBParameterGroupName=parameter_group_name,
                DBParameterGroupFamily=parameter_group_family, 
                Description=description
            )
        except ClientError as e:
            logger.error("Couldn't create parameter group %s. Here's why: %s: %s",
                         parameter_group_name, e.response['Error']['Code'], e.response['Error']['Message'])
            raise
        else:
            return response

    def delete_parameter_group(self, parameter_group_name):
        try:
            self.rds_client.delete_db_parameter_group(
                DBParameterGroupName=parameter_group_name
            )
        except ClientError as e:
            logger.error("Couldn't delete parameter group %s. Here's why: %s: %s",
                         parameter_group_name, e.response['Error']['Code'], e.response['Error']['Message'])
            raise

    def get_parameters(self, parameter_group_name, name_prefix="", source=None):
        kwargs = {'DBParameterGroupName': parameter_group_name}
        if source is not None:
            kwargs['Source'] = source
            
        parameters = []
        paginator = self.rds_client.get_paginator('describe_db_parameters')
        
        for page in paginator.paginate(**kwargs):
            parameters += [p for p in page['Parameters'] if p['ParameterName'].startswith(name_prefix)]
            
        return parameters

    def update_parameters(self, parameter_group_name, update_parameters):
        try:
            response = self.rds_client.modify_db_parameter_group(
                DBParameterGroupName=parameter_group_name,
                Parameters=update_parameters
            )
        except ClientError as e:
            logger.error("Couldn't update parameters in %s. Here's why: %s: %s",
                         parameter_group_name, e.response['Error']['Code'], e.response['Error']['Message'])
            raise
        else:
            return response

    def create_snapshot(self, snapshot_id, instance_id):
        try:
            response = self.rds_client.create_db_snapshot(
                DBSnapshotIdentifier=snapshot_id,
                DBInstanceIdentifier=instance_id
            )
            snapshot = response['DBSnapshot']
        except ClientError as e:
            logger.error("Couldn't create snapshot of %s. Here's why: %s: %s",
                         instance_id, e.response['Error']['Code'], e.response['Error']['Message'])
            raise
        else:
            return snapshot

    def get_snapshot(self, snapshot_id):
        try:
            response = self.rds_client.describe_db_snapshots(
                DBSnapshotIdentifier=snapshot_id
            )
            snapshot = response['DBSnapshots'][0]
        except ClientError as e:
            logger.error("Couldn't get snapshot %s. Here's why: %s: %s",
                         snapshot_id, e.response['Error']['Code'], e.response['Error']['Message'])
            raise
        else:
            return snapshot

    def get_engine_versions(self, engine, parameter_group_family=None):
        kwargs = {'Engine': engine}
        if parameter_group_family is not None:
            kwargs['DBParameterGroupFamily'] = parameter_group_family
            
        try:
            response = self.rds_client.describe_db_engine_versions(**kwargs)
            versions = response['DBEngineVersions']
        except ClientError as e:
            logger.error("Couldn't get engine versions for %s. Here's why: %s: %s",
                         engine, e.response['Error']['Code'], e.response['Error']['Message'])
            raise
        else:
            return versions

    def get_orderable_instances(self, db_engine, db_engine_version):
        try:
            inst_opts = []
            paginator = self.rds_client.get_paginator('describe_orderable_db_instance_options')
            
            for page in paginator.paginate(Engine=db_engine, 
                                           EngineVersion=db_engine_version):
                inst_opts += page['OrderableDBInstanceOptions']
                
        except ClientError as e:
            logger.error("Couldn't get orderable DB instances. Here's why: %s: %s", 
                         e.response['Error']['Code'], e.response['Error']['Message'])
            raise
        else:
            return inst_opts

    def get_db_instance(self, instance_id):
        try:
            response = self.rds_client.describe_db_instances(
                DBInstanceIdentifier=instance_id
            )
            db_inst = response['DBInstances'][0]
        except ClientError as e:
            if e.response['Error']['Code'] == 'DBInstanceNotFound':
                logger.info("Instance %s does not exist.", instance_id)
            else:
                logger.error("Couldn't get DB instance %s. Here's why: %s: %s",
                             instance_id, e.response['Error']['Code'], e.response['Error']['Message'])
                raise
        else:
            return db_inst

    def create_db_instance(self, db_name, instance_id, parameter_group_name, 
                           db_engine, db_engine_version, instance_class, 
                           storage_type, allocated_storage, admin_name, admin_password):
        
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
            db_inst = response['DBInstance']
        except ClientError as e:
            logger.error("Couldn't create DB instance %s. Here's why: %s: %s",
                         instance_id, e.response['Error']['Code'], e.response['Error']['Message'])
            raise
        else:
            return db_inst

    def delete_db_instance(self, instance_id):
        try:
            response = self.rds_client.delete_db_instance(
                DBInstanceIdentifier=instance_id,
                SkipFinalSnapshot=True,
                DeleteAutomatedBackups=True
            )
            db_inst = response['DBInstance']
        except ClientError as e:
            logger.error("Couldn't delete DB instance %s. Here's why: %s: %s",
                         instance_id, e.response['Error']['Code'], e.response['Error']['Message'])
            raise
        else:
            return db_inst
```

```python
import json
import logging
import boto3
from botocore.exceptions import ClientError
import questionary as q

logger = logging.getLogger(__name__)

class StateMachineScenario:

    def __init__(self, activity, state_machine, iam_client):
        self.activity = activity
        self.state_machine = state_machine
        self.iam_client = iam_client
        self.state_machine_role = None

    def prerequisites(self, state_machine_role_name):
        trust_policy = {
            "Version": "2012-10-17",
            "Statement": [
                {
                    "Sid": "",
                    "Effect": "Allow",
                    "Principal": {"Service": "states.amazonaws.com"},
                    "Action": "sts:AssumeRole"
                }
            ]
        }
        
        try:
            role = self.iam_client.get_role(RoleName=state_machine_role_name)
            print(f"Prerequisite IAM role {state_machine_role_name} already exists.")
        except ClientError as e:
            if e.response['Error']['Code'] == 'NoSuchEntity':
                role = None
            else:
                logger.error("Couldn't get prerequisite IAM role %s. Here's why: %s: %s", 
                             state_machine_role_name, e.response['Error']['Code'], e.response['Error']['Message'])
                raise
        
        if role is None:
            try:
                role = self.iam_client.create_role(RoleName=state_machine_role_name,
                                                  AssumeRolePolicyDocument=json.dumps(trust_policy))
                logger.info("Created role %s.", role.name)
            except ClientError as e:
                logger.error("Couldn't create prerequisite IAM role %s. Here's why: %s: %s",
                             state_machine_role_name, e.response['Error']['Code'], e.response['Error']['Message'])
                raise
        
        self.state_machine_role = role

    def find_or_create_activity(self, activity_name):
        print("First, let's set up an activity and state machine.")
        
        activity_arn = self.activity.find(activity_name)
        if activity_arn is None:
            activity_arn = self.activity.create(activity_name)
            print(f"Activity {activity_name} created. Its Amazon Resource Name (ARN) is {activity_arn}.")
        else:
            print(f"Activity {activity_name} already exists.")
            
        return activity_arn

    def find_or_create_state_machine(self, state_machine_name, activity_arn, state_machine_file):
        state_machine_arn = self.state_machine.find(state_machine_name)
        if state_machine_arn is None:
            with open(state_machine_file) as f:
                state_machine_def = f.read().replace("{{DOC_EXAMPLE_ACTIVITY_ARN}}", activity_arn)
                
            state_machine_arn = self.state_machine.create(state_machine_name, 
                                                         state_machine_def,
                                                         self.state_machine_role['Arn'])
            print(f"State machine {state_machine_name} created.")
        else:
            print(f"State machine {state_machine_name} already exists.")
            
        print("-"*88)
        print(f"Here's some information about state machine {state_machine_name}:")
        state_machine_info = self.state_machine.describe(state_machine_arn)
        for field in ['name', 'status', 'stateMachineArn', 'roleArn']:
            print(f"\t{field}: {state_machine_info[field]}")
            
        return state_machine_arn

    def run_state_machine(self, state_machine_arn, activity_arn):
        print(f"Let's run the state machine. It's a simplistic, non-AI chat simulator we'll call ChatSFN.")
        
        user_name = q.ask("What should ChatSFN call you? ", q.non_empty)
        run_input = {'name': user_name}
        
        print("Starting state machine...")
        run_arn = self.state_machine.start(state_machine_arn, json.dumps(run_input))
        
        action = None
        while action != 'done':
            activity_task = self.activity.get_task(activity_arn)
            task_input = json.loads(activity_task['input'])
            
            print(f"ChatSFN: {task_input['message']}")
            action = task_input['actions'][q.choose('What now? ', task_input['actions'])]
            
            task_response = {'action': action}
            self.activity.send_task_success(activity_task['taskToken'], 
                                            json.dumps(task_response))
            
        return run_arn

    def finish_state_machine_run(self, run_arn):
        print(f"Let's get the final output from the state machine:")
        
        status = 'RUNNING'
        while status == 'RUNNING':
            run_output = self.state_machine.describe_run(run_arn)
            status = run_output['status']
            
            if status == 'RUNNING':
                print("The state machine is still running, let's wait for it to finish.")
                time.sleep(1)
            elif status == 'SUCCEEDED':
                print(f"ChatSFN: {json.loads(run_output['output'])['message']}")
            else:
                print(f"Run status: {status}.")

    def cleanup(self, state_machine_name, state_machine_arn, activity_name, activity_arn, state_machine_role_name):
        if q.ask("Do you want to delete the state machine, activity, and role created for this example? (y/n) ", 
                 q.is_yesno):
            self.state_machine.delete(state_machine_arn)
            print(f"Deleted state machine {state_machine_name}.")
            
            self.activity.delete(activity_arn)
            print(f"Deleted activity {activity_name}.")
            
            self.iam_client.delete_role(RoleName=state_machine_role_name)
            print(f"Deleted role {state_machine_role_name}.")

    def run_scenario(self, activity_name, state_machine_name):
        print("-"*88)
        print("Welcome to the AWS Step Functions state machines demo.")
        print("-"*88)
        
        activity_arn = self.find_or_create_activity(activity_name)
        state_machine_arn = self.find_or_create_state_machine(state_machine_name,
                                                             activity_arn,
                                                             "../../../resources/sample_files/chat_sfn_state_machine.json")
                                                             
        print("-"*88)
        run_arn = self.run_state_machine(state_machine_arn, activity_arn)
        print("-"*88)
        self.finish_state_machine_run(run_arn)
        print("-"*88)
        self.cleanup(state_machine_name, state_machine_arn, activity_name, 
                     activity_arn, self.state_machine_role['RoleName'])
                     
        print("-"*88)
        print("\nThanks for watching!")
        print("-"*88)


if __name__ == '__main__':
    logging.basicConfig(level=logging.INFO, format="%(levelname)s: %(message)s")
    try:
        stepfunctions_client = boto3.client('stepfunctions')
        iam_client = boto3.client('iam')
        scenario = StateMachineScenario(Activity(stepfunctions_client), 
                                        StateMachine(stepfunctions_client),
                                        iam_client)
        scenario.prerequisites("doc-example-state-machine-chat")
        scenario.run_scenario("doc-example-activity", "doc-example-state-machine")
    except Exception:
        logging.exception("Something went wrong with the demo.")
```

The key differences:

- Python uses classes and methods instead of standalone functions 
- Uses built-in Python libraries like json, logging, zipfile instead of separate SDK classes
- boto3 client instead of SDK client classes
- Python exception handling instead of outcomes
- Python print() instead of cout
- Logging instead of cerr/cout
- f-strings for string formatting
- list comprehensions and generator expressions instead of explicit loops
- Python built-in open() instead of streams
- Python threads instead of C++ threads/semaphores

The overall structure is very similar, with the main logic extracted out into classes. The boto3 client provides a Pythonic interface to the AWS services.