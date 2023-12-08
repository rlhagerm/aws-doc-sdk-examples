//snippet-sourcedescription:[DeleteApplication.java demonstrates how to delete an application.]
//snippet-keyword:[AWS SDK for Java v2]
//snippet-keyword:[AWS CodeDeploy]

/*
   Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
   SPDX-License-Identifier: Apache-2.0
*/

package com.example.deploy;

// snippet-start:[codedeploy.java2.delete_app.main]
// snippet-start:[codedeploy.java2.delete_app.import]
import software.amazon.awssdk.regions.Region;
import software.amazon.awssdk.services.codedeploy.CodeDeployClient;
import software.amazon.awssdk.services.codedeploy.model.CodeDeployException;
import software.amazon.awssdk.services.codedeploy.model.DeleteApplicationRequest;
// snippet-end:[codedeploy.java2.delete_app.import]

/**
 * Before running this Java V2 code example, set up your development environment, including your credentials.
 *
 * For more information, see the following documentation topic:
 *
 * https://docs.aws.amazon.com/sdk-for-java/latest/developer-guide/get-started.html
 */

public class DeleteApplication {
    public static void main(String[] args) {

        final String usage = """

            Usage:
                <appName>\s

            Where:
                appName -  The name of the application.\s
            """;

        if (args.length != 1) {
            System.out.println(usage);
            System.exit(1);
         }

        String appName = args[0];
        Region region = Region.US_EAST_1;
        CodeDeployClient deployClient = CodeDeployClient.builder()
            .region(region)
            .build();

        delApplication(deployClient, appName);
        deployClient.close();
    }

    public static void delApplication(CodeDeployClient deployClient, String appName) {

        try {
            DeleteApplicationRequest deleteApplicationRequest = DeleteApplicationRequest.builder()
                .applicationName(appName)
                .build();

            deployClient.deleteApplication(deleteApplicationRequest);
            System.out.println(appName +" was deleted!");

        } catch (CodeDeployException e) {
            System.err.println(e.awsErrorDetails().errorMessage());
            System.exit(1);
        }
    }
    // snippet-end:[codedeploy.java2.delete_app.main]
}
