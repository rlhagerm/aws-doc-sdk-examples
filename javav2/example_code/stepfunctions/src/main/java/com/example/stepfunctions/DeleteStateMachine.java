//snippet-sourcedescription:[DeleteStateMachine.java demonstrates how to delete a state machine for AWS Step Functions.]
//snippet-keyword:[AWS SDK for Java v2]
//snippet-service:[AWS Step Functions]
/*
   Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
   SPDX-License-Identifier: Apache-2.0
*/

package com.example.stepfunctions;

// snippet-start:[stepfunctions.java2.delete_machine.import]
import software.amazon.awssdk.auth.credentials.ProfileCredentialsProvider;
import software.amazon.awssdk.regions.Region;
import software.amazon.awssdk.services.sfn.SfnClient;
import software.amazon.awssdk.services.sfn.model.SfnException;
import software.amazon.awssdk.services.sfn.model.DeleteStateMachineRequest;
// snippet-end:[stepfunctions.java2.delete_machine.import]

/**
 * Before running this Java V2 code example, set up your development environment, including your credentials.
 *
 * For more information, see the following documentation topic:
 *
 * https://docs.aws.amazon.com/sdk-for-java/latest/developer-guide/get-started.html
 */
public class DeleteStateMachine {

    public static void main(String[] args) {

        final String usage = "\n" +
            "Usage:\n" +
            "    <stateMachineArn>\n\n" +
            "Where:\n" +
            "    stateMachineArn - The ARN of the state machine to delete.\n";

        if (args.length != 1) {
            System.out.println(usage);
            System.exit(1);
        }

        String stateMachineArn = args[0];
        Region region = Region.US_EAST_1;
        SfnClient sfnClient = SfnClient.builder()
            .region(region)
            .credentialsProvider(ProfileCredentialsProvider.create())
            .build();

        deleteMachine(sfnClient, stateMachineArn);
        sfnClient.close();
    }

    public static void deleteMachine(SfnClient sfnClient, String stateMachineArn) {
        try {
            DeleteStateMachineRequest deleteStateMachineRequest = DeleteStateMachineRequest.builder()
                .stateMachineArn(stateMachineArn)
                .build();

            sfnClient.deleteStateMachine(deleteStateMachineRequest);
            System.out.println(stateMachineArn +" was successfully deleted.");

        } catch (SfnException e) {
            System.err.println(e.awsErrorDetails().errorMessage());
            System.exit(1);
        }
    }
}

