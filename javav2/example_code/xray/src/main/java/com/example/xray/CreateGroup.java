//snippet-sourcedescription:[CreateGroup.java demonstrates how to create an AWS XRay group with a filter expression.]
//snippet-keyword:[SDK for Java 2.0]
//snippet-service:[AWS X-Ray Service]

/*
   Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
   SPDX-License-Identifier: Apache-2.0
*/

package com.example.xray;

// snippet-start:[xray.java2_create_group.main]
// snippet-start:[xray.java2_create_group.import]
import software.amazon.awssdk.regions.Region;
import software.amazon.awssdk.services.xray.XRayClient;
import software.amazon.awssdk.services.xray.model.CreateGroupRequest;
import software.amazon.awssdk.services.xray.model.CreateGroupResponse;
import software.amazon.awssdk.services.xray.model.XRayException;
// snippet-end:[xray.java2_create_group.import]

/**
 * Before running this Java V2 code example, set up your development environment, including your credentials.
 *
 * For more information, see the following documentation topic:
 *
 * https://docs.aws.amazon.com/sdk-for-java/latest/developer-guide/get-started.html
 */
public class CreateGroup {
    public static void main(String[] args) {
        final String usage = """

            Usage:    <groupName>

            Where:
               groupName - The name of the group to create\s

            """;

        if (args.length != 1) {
            System.out.println(usage);
            System.exit(1);
        }

        String groupName = args[0];
        Region region = Region.US_EAST_1;
        XRayClient xRayClient = XRayClient.builder()
            .region(region)
            .build();

        createNewGroup(xRayClient, groupName);
    }

    public static void createNewGroup(XRayClient xRayClient, String groupName) {
        try {
            CreateGroupRequest groupRequest = CreateGroupRequest.builder()
                .filterExpression("fault = true AND http.url CONTAINS \"example/game\" AND responsetime >= 5")
                .groupName(groupName)
                .build();

            CreateGroupResponse groupResponse = xRayClient.createGroup(groupRequest);
            System.out.println("The Group ARN is " + groupResponse.group().groupARN());

        } catch (XRayException e) {
            System.err.println(e.getMessage());
            System.exit(1);
        }
    }
}
// snippet-end:[xray.java2_create_group.main]
