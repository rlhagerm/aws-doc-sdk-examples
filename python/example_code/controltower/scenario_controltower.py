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


class ControlTowerScenario:
    log_account_id = ""
    security_account_id = ""
    stack_name = ""

    def __init__(self, medical_imaging_wrapper, s3_client, cf_resource):
        self.medical_imaging_wrapper = medical_imaging_wrapper
        self.s3_client = s3_client
        self.cf_resource = cf_resource

    def run_scenario(self):
        print("-" * 88)
        print(
            "\t\tWelcome to the AWS ControlTower with ControlCatalog example scenario."
        )
        print("-" * 88)

        print(
            """\
        This scenario will...

        These resources are created using the provided AWS CloudFormation stack
        which will be deployed now.
        """
        )
        cf_stack = self.deploy()

        print("\t\tThis concludes this scenario.")
        if q.ask(
                f"\t\tClean up resources created by the scenario? (y/n) ",
                q.is_yesno,
        ):
            self.destroy(cf_stack)
            print("\t\tRemoved resources created by the scenario.")
        print("\t\tThanks for watching!")
        print("-" * 88)

    def deploy(self):
        """
        Deploys prerequisite resources used by the scenario. The resources are
        defined in the associated `setup.yaml` AWS CloudFormation script and are deployed
        as a CloudFormation stack, so they can be easily managed and destroyed.
        """

        print("\t\tLet's deploy the stack for resource creation.")
        stack_name = q.ask("\t\tEnter a name for the stack: ", q.non_empty)

        logging_account_email = q.ask(
            "\t\tEnter an email address for the logging account: ", q.non_empty
        )

        logging_account_name = q.ask(
            "\t\tEnter a name for the logging account: ", q.non_empty
        )

        security_account_email = q.ask(
            "\t\tEnter an email address for the security account: ", q.non_empty
        )

        security_account_name = q.ask(
            "\t\tEnter a name for the security account: ", q.non_empty
        )

        account_id = boto3.client("sts").get_caller_identity()["Account"]

        with open(
            "../../../scenarios/basics/controltower/resources/cfn_template.yaml"
        ) as setup_file:
            setup_template = setup_file.read()
        print(f"\t\tCreating {stack_name}.")
        stack = self.cf_resource.create_stack(
            StackName=stack_name,
            TemplateBody=setup_template,
            Capabilities=["CAPABILITY_NAMED_IAM"],
            Parameters=[
                {
                    "ParameterKey": "LoggingAccountEmail",
                    "ParameterValue": logging_account_email,
                },
                {
                    "ParameterKey": "LoggingAccountName",
                    "ParameterValue": logging_account_name,
                },
                {
                    "ParameterKey": "SecurityAccountEmail",
                    "ParameterValue": security_account_email,
                },
                {
                    "ParameterKey": "SecurityAccountName",
                    "ParameterValue": security_account_name,
                },
            ],
        )
        print("\t\tWaiting for stack to deploy. This typically takes a minute or two.")
        waiter = self.cf_resource.meta.client.get_waiter("stack_create_complete")
        waiter.wait(StackName=stack.name)
        stack.load()
        print(f"\t\tStack status: {stack.stack_status}")

        outputs_dictionary = {
            output["OutputKey"]: output["OutputValue"] for output in stack.outputs
        }
        self.log_account_id = outputs_dictionary["LogAccountId"]
        self.security_account_id = outputs_dictionary["SecurityAccountId"]

        return stack

    def destroy(self, stack):
        """
        Destroys the resources managed by the CloudFormation stack, and the CloudFormation
        stack itself.

        :param stack: The CloudFormation stack that manages the example resources.
        """

        print(f"\t\tCleaning up resources and {stack.name}.")

        print(f"\t\tDeleting {stack.name}.")
        stack.delete()
        print("\t\tWaiting for stack removal. This may take a few minutes.")
        waiter = self.cf_resource.meta.client.get_waiter("stack_delete_complete")
        waiter.wait(StackName=stack.name)
        print("\t\tStack delete complete.")


if __name__ == "__main__":
    try:
        control_tower = boto3.client("controltower")
        cf = boto3.resource("cloudformation")
        control_tower_wrapper = ControlTowerWrapper.from_client()

        scenario = ControlTowerScenario(control_tower_wrapper, control_tower, cf)
        scenario.run_scenario()
    except Exception:
        logging.exception("Something went wrong with the scenario.")
