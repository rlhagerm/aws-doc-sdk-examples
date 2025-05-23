# Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
# SPDX-License-Identifier: Apache-2.0


import logging
import random
import sys
import datetime

import boto3
from botocore.exceptions import ClientError

from controltower_wrapper import ControlTowerWrapper

# Add relative path to include demo_tools in this code example without need for setup.
sys.path.append("../..")
import demo_tools.question as q  # noqa

logger = logging.getLogger(__name__)


class ControlTowerScenario:
    stack_name = ""

    def __init__(self, controltower_wrapper, cloudformation_resource):
        """
        :param controltower_wrapper: An instance of the ControlTowerWrapper class.
        :param cloudformation_resource: A Boto3 CloudFormation resource.
        """
        self.controltower_wrapper = controltower_wrapper
        self.cf_resource = cloudformation_resource
        self.stack = None
        self.ou_id = None

    def run_scenario(self):
        print("-" * 88)
        print(
            "\t\tWelcome to the AWS ControlTower with ControlCatalog example scenario."
        )
        print("-" * 88)

        print("This demo will walk you through setting up an AWS Control Tower landing zone,")
        print("managing baselines, and working with controls.")

        try:
            # Set up organization and get Sandbox OU ID
            sandbox_ou_id = self.setup_organization()

            # Store the OU ID for use in the CloudFormation template
            self.ou_id = sandbox_ou_id

            # Deploy the stack.
            cf_stack = self.deploy_stack(self.ou_id)

            # Landing Zone Setup
            account_id = boto3.client("sts").get_caller_identity()["Account"]
            print("\nSetting up Landing Zone")
            manifest = self.create_landing_zone_manifest(account_id)
            lz_response = self.controltower_wrapper.create_landing_zone(manifest)
            print(f"Landing Zone ARN: {lz_response['landingZoneArn']}")
            print(f"Operation ID: {lz_response['operationId']}")

            # Wait for Landing Zone setup to complete
            print("\nWaiting for Landing Zone setup to complete...")
            while True:
                status = self.controltower_wrapper.get_landing_zone_operation(
                    lz_response['operationId'])
                print(f"Status: {status}")
                if status in ['SUCCEEDED', 'FAILED']:
                    break
                datetime.time.sleep(30)

            if status == 'SUCCEEDED':
                # Enable Baseline
                print("\nEnabling Identity Center Baseline")
                baseline_arn = self.controltower_wrapper.enable_baseline(
                    lz_response['landingZoneArn'],
                    'IDENTITYCENTER'
                )
                print(f"Enabled baseline ARN: {baseline_arn}")

                # List and Enable Controls
                print("\nManaging Controls")
                controls = self.controltower_wrapper.list_controls()
                print("\nAvailable Controls:")
                for i, control in enumerate(controls[:5], 1):
                    print(f"{i}. {control['controlName']}")

                if controls:
                    # Enable first control as an example.
                    control_arn = controls[0]['controlArn']
                    target_ou = self.get_organization_unit_arn()  # You'll need to implement this

                    print(f"\nEnabling control: {controls[0]['controlName']}")
                    operation_id = self.controltower_wrapper.enable_control(
                        control_arn, target_ou)

                    # Wait for control operation to complete.
                    while True:
                        status = self.controltower_wrapper.get_control_operation(operation_id)
                        print(f"Control operation status: {status}")
                        if status in ['SUCCEEDED', 'FAILED']:
                            break
                        datetime.time.sleep(30)

                    if status == 'SUCCEEDED':
                        # Disable the control
                        print("\nDisabling the control...")
                        operation_id = self.controltower_wrapper.disable_control(
                            control_arn, target_ou)
                        print(f"Disable operation ID: {operation_id}")

            print("\t\tThis concludes the scenario.")
            if q.ask(
                    f"\t\tClean up resources created by the scenario? (y/n) ",
                    q.is_yesno,
            ):
                self.destroy_stack(cf_stack)
                print("\t\tRemoved resources created by the scenario.")
            print("\t\tThanks for watching!")
            print("-" * 88)
        except Exception:
            logging.exception("Something went wrong with the demo!")
            self.destroy_stack(cf_stack)

    def setup_organization(self):
        """
        Checks if the current account is part of an organization and creates one if needed.
        Also ensures a Sandbox OU exists and returns its ID.

        :return: The ID of the Sandbox OU
        """
        print("\nChecking organization status...")
        org_client = boto3.client('organizations')

        try:
            # Check if account is part of an organization
            org_response = org_client.describe_organization()
            org_id = org_response['Organization']['Id']
            print(f"Account is part of organization: {org_id}")

        except ClientError as error:
            if error.response['Error']['Code'] == 'AWSOrganizationsNotInUseException':
                print("No organization found. Creating a new organization...")
                try:
                    create_response = org_client.create_organization(
                        FeatureSet='ALL'
                    )
                    org_id = create_response['Organization']['Id']
                    print(f"Created new organization: {org_id}")

                    # Wait for organization to be available
                    waiter = org_client.get_waiter('organization_active')
                    waiter.wait(
                        Organization=org_id,
                        WaiterConfig={'Delay': 5, 'MaxAttempts': 12}
                    )

                except ClientError as create_error:
                    logger.error(
                        "Couldn't create organization. Here's why: %s: %s",
                        create_error.response["Error"]["Code"],
                        create_error.response["Error"]["Message"]
                    )
                    raise
            else:
                logger.error(
                    "Couldn't describe organization. Here's why: %s: %s",
                    error.response["Error"]["Code"],
                    error.response["Error"]["Message"]
                )
                raise

        # Look for Sandbox OU
        sandbox_ou_id = None
        paginator = org_client.get_paginator('list_organizational_units_for_parent')

        try:
            # Get root ID first
            roots = org_client.list_roots()['Roots']
            if not roots:
                raise ValueError("No root found in organization")
            root_id = roots[0]['Id']

            # Search for existing Sandbox OU
            print("Checking for Sandbox OU...")
            for page in paginator.paginate(ParentId=root_id):
                for ou in page['OrganizationalUnits']:
                    if ou['Name'] == 'Sandbox':
                        sandbox_ou_id = ou['Id']
                        print(f"Found existing Sandbox OU: {sandbox_ou_id}")
                        break
                if sandbox_ou_id:
                    break

            # Create Sandbox OU if it doesn't exist
            if not sandbox_ou_id:
                print("Creating Sandbox OU...")
                create_ou_response = org_client.create_organizational_unit(
                    ParentId=root_id,
                    Name='Sandbox'
                )
                sandbox_ou_id = create_ou_response['OrganizationalUnit']['Id']
                print(f"Created new Sandbox OU: {sandbox_ou_id}")

                # Wait for OU to be available
                waiter = org_client.get_waiter('organizational_unit_active')
                waiter.wait(
                    OrganizationalUnitId=sandbox_ou_id,
                    WaiterConfig={'Delay': 5, 'MaxAttempts': 12}
                )

        except ClientError as error:
            logger.error(
                "Couldn't set up Sandbox OU. Here's why: %s: %s",
                error.response["Error"]["Code"],
                error.response["Error"]["Message"]
            )
            raise

        return sandbox_ou_id

    def deploy_stack(self, sandbox_ou_id: str):
        """
        Deploys prerequisite resources used by the scenario. The resources are
        defined in the associated `setup.yaml` AWS CloudFormation script and are deployed
        as a CloudFormation stack, so they can be easily managed and destroyed.
        """

        print("Let's deploy the stack for resource creation.")
        stack_name = q.ask("Enter a name for the stack: ", q.non_empty)

        with open(
            "../../../scenarios/basics/controltower/resources/cfn_template.yaml"
        ) as setup_file:
            setup_template = setup_file.read()
        print(f"Creating {stack_name}.")
        stack = self.cf_resource.create_stack(
            StackName=stack_name,
            TemplateBody=setup_template,
            Capabilities=["CAPABILITY_NAMED_IAM"],
            Parameters=[
                {
                    "ParameterKey": "ParentOrganizationId",
                    "ParameterValue": sandbox_ou_id,
                },
            ],
        )
        print("Waiting for stack to deploy. This typically takes a minute or two.")
        waiter = self.cf_resource.meta.client.get_waiter("stack_create_complete")
        waiter.wait(StackName=stack.name)
        stack.load()
        print(f"Stack status: {stack.stack_status}")

        '''
        outputs_dictionary = {
            output["OutputKey"]: output["OutputValue"] for output in stack.outputs
        }
        self.log_account_id = outputs_dictionary["LogAccountId"]
        self.security_account_id = outputs_dictionary["SecurityAccountId"]
        '''
        return stack

    def create_landing_zone_manifest(self, account_id: str):
        """
        Creates a landing zone manifest based on the CloudFormation outputs.
        """

        # Create and return manifest structure
        return {
            "governedRegions": ["us-east-1"],
            "organizationStructure": {
                "security": {
                    "name": "SecurityExample"
                },
                "sandbox": {
                    "name": "SandboxExample"
                }
            },
            "securityRoles": {
                "accountId": "565846806325"
            },
            "accessManagement": {
                "enabled": True
            },
            "centralizedLogging": {
                "accountId": "852843827772",
                "configurations": {
                    "loggingBucket": {
                        "retentionDays": 60
                    },
                    "accessLoggingBucket": {
                        "retentionDays": 60
                    }
                },
                "enabled": True
            }
        }

    def destroy_stack(self, stack):
        """
        Destroys the resources managed by the CloudFormation stack, and the CloudFormation
        stack itself.

        :param stack: The CloudFormation stack that manages the example resources.
        """

        print(f"Cleaning up resources and {stack.name}.")

        print(f"Deleting {stack.name}.")
        stack.delete()
        print("Waiting for stack removal. This may take a few minutes.")
        waiter = self.cf_resource.meta.client.get_waiter("stack_delete_complete")
        waiter.wait(StackName=stack.name)
        print("Stack delete complete.")


if __name__ == "__main__":
    try:
        cf = boto3.resource("cloudformation")
        control_tower_wrapper = ControlTowerWrapper.from_client()

        scenario = ControlTowerScenario(control_tower_wrapper, cf)
        scenario.run_scenario()
    except Exception:
        logging.exception("Something went wrong with the scenario.")
