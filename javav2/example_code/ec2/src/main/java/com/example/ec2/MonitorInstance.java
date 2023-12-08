//snippet-sourcedescription:[MonitorInstance.java demonstrates how to toggle detailed monitoring for an Amazon Elastic Compute Cloud (Amazon EC2) instance.]
//snippet-keyword:[AWS SDK for Java v2]
//snippet-service:[Amazon EC2]

/*
   Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
   SPDX-License-Identifier: Apache-2.0
*/

package com.example.ec2;

// snippet-start:[ec2.java2.monitor_instance.main]
// snippet-start:[ec2.java2.monitor_instance.import]
import software.amazon.awssdk.regions.Region;
import software.amazon.awssdk.services.ec2.Ec2Client;
import software.amazon.awssdk.services.ec2.model.MonitorInstancesRequest;
import software.amazon.awssdk.services.ec2.model.UnmonitorInstancesRequest;
// snippet-end:[ec2.java2.monitor_instance.import]

/**
 * Before running this Java V2 code example, set up your development environment, including your credentials.
 *
 * For more information, see the following documentation topic:
 *
 * https://docs.aws.amazon.com/sdk-for-java/latest/developer-guide/get-started.html
 */
public class MonitorInstance {

    public static void main(String[] args) {
        final String usage = """

            Usage:
               <instanceId> <monitor>

            Where:
               instanceId - An instance id value that you can obtain from the AWS Management Console.\s
               monitor - A monitoring status (true|false)""";

        if (args.length != 2) {
            System.out.println(usage);
            System.exit(1);
        }

        String instanceId = args[0];
        boolean monitor = Boolean.parseBoolean(args[1]);
        Region region = Region.US_EAST_1;
        Ec2Client ec2 = Ec2Client.builder()
            .region(region)
            .build();

        if (monitor) {
            monitorInstance(ec2, instanceId);
        } else {
            unmonitorInstance(ec2, instanceId);
        }
        ec2.close();
    }

    public static void monitorInstance(Ec2Client ec2, String instanceId) {
        MonitorInstancesRequest request = MonitorInstancesRequest.builder()
            .instanceIds(instanceId)
            .build();

        ec2.monitorInstances(request);
        System.out.printf("Successfully enabled monitoring for instance %s", instanceId);
    }
    // snippet-end:[ec2.java2.monitor_instance.main]

    // snippet-start:[ec2.java2.monitor_instance.stop]
    public static void unmonitorInstance(Ec2Client ec2, String instanceId) {
        UnmonitorInstancesRequest request = UnmonitorInstancesRequest.builder()
            .instanceIds(instanceId)
            .build();

        ec2.unmonitorInstances(request);
        System.out.printf("Successfully disabled monitoring for instance %s", instanceId);
    }
    // snippet-end:[ec2.java2.monitor_instance.stop]
}
