//snippet-sourcedescription:[StartStopInstance.java demonstrates how to start and stop an Amazon Elastic Compute Cloud (Amazon EC2) instance.]
//snippet-keyword:[AWS SDK for Java v2]
//snippet-service:[Amazon EC2]

/*
   Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
   SPDX-License-Identifier: Apache-2.0
*/
package com.example.ec2;

// snippet-start:[ec2.java2.start_stop_instance.import]
import software.amazon.awssdk.regions.Region;
import software.amazon.awssdk.services.ec2.Ec2Client;
import software.amazon.awssdk.services.ec2.model.StartInstancesRequest;
import software.amazon.awssdk.services.ec2.model.StopInstancesRequest;
// snippet-end:[ec2.java2.start_stop_instance.import]

/**
 * Before running this Java V2 code example, set up your development environment, including your credentials.
 *
 * For more information, see the following documentation topic:
 *
 * https://docs.aws.amazon.com/sdk-for-java/latest/developer-guide/get-started.html
 */
public class StartStopInstance {
    public static void main(String[] args) {
        final String usage = """

            Usage:
               <instanceId>

            Where:
               instanceId - an instance id value that you can obtain from the AWS Console.\s

            """;

        if (args.length != 1) {
            System.out.println(usage);
            System.exit(1);
        }

        String instanceId = args[0];
        boolean start;

        Region region = Region.US_EAST_1;
        Ec2Client ec2 = Ec2Client.builder()
            .region(region)
            .build();

        if (args[1].equals("start")) {
            start = true;
        } else {
            start = false;
        }

        if (start) {
            startInstance(ec2, instanceId);
        } else {
            stopInstance(ec2, instanceId);
        }
        ec2.close();
    }

    // snippet-start:[ec2.java2.start_stop_instance.start]
    public static void startInstance(Ec2Client ec2, String instanceId) {
        StartInstancesRequest request = StartInstancesRequest.builder()
            .instanceIds(instanceId)
            .build();

        ec2.startInstances(request);
        System.out.printf("Successfully started instance %s", instanceId);
    }
    // snippet-end:[ec2.java2.start_stop_instance.start]

    // snippet-start:[ec2.java2.start_stop_instance.stop]
    public static void stopInstance(Ec2Client ec2, String instanceId) {
        StopInstancesRequest request = StopInstancesRequest.builder()
            .instanceIds(instanceId)
            .build();

        ec2.stopInstances(request);
        System.out.printf("Successfully stopped instance %s", instanceId);
    }
    // snippet-end:[ec2.java2.start_stop_instance.stop]
}
