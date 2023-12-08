//snippet-sourcedescription:[DescribeAnalysis.java demonstrates how to obtain information about a dashboard.]
//snippet-keyword:[AWS SDK for Java v2]
//snippet-service:[Amazon QuickSight]

/*
   Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
   SPDX-License-Identifier: Apache-2.0
*/

package com.example.quicksight;

// snippet-start:[quicksight.java2.describe_dashboard.main]
// snippet-start:[quicksight.java2.describe_dashboard.import]
import software.amazon.awssdk.regions.Region;
import software.amazon.awssdk.services.quicksight.QuickSightClient;
import software.amazon.awssdk.services.quicksight.model.DescribeDashboardRequest;
import software.amazon.awssdk.services.quicksight.model.DescribeDashboardResponse;
import software.amazon.awssdk.services.quicksight.model.QuickSightException;
// snippet-end:[quicksight.java2.describe_dashboard.import]

/**
 * Before running this Java V2 code example, set up your development environment, including your credentials.
 *
 * For more information, see the following documentation topic:
 *
 * https://docs.aws.amazon.com/sdk-for-java/latest/developer-guide/get-started.html
 */
public class DescribeDashboard {
    public static void main(String[] args) {

        final String usage = """

            Usage:    <account> <dashboardId>

            Where:
              account - The ID of the AWS account.
              dashboardId - The ID of the Amazon QuickSight Dashboard to describe.
            """;

        if (args.length != 2) {
            System.out.println(usage);
            System.exit(1);
        }

        String account = args[0];
        String dashboardId = args[1];
        QuickSightClient qsClient = QuickSightClient.builder()
            .region(Region.US_EAST_1)
            .build();

        describeSpecificDashboard(qsClient, account, dashboardId);
        qsClient.close();
    }

    public static void describeSpecificDashboard(QuickSightClient qsClient, String account, String dashboardId) {
        try {
            DescribeDashboardRequest analysesRequest = DescribeDashboardRequest.builder()
                .awsAccountId(account)
                .dashboardId(dashboardId)
                .build();

            DescribeDashboardResponse res = qsClient.describeDashboard(analysesRequest);
            System.out.println("The display name is " + res.dashboard().name());

        } catch (QuickSightException e) {
            System.err.println(e.awsErrorDetails().errorMessage());
            System.exit(1);
        }
    }
}
// snippet-end:[quicksight.java2.describe_dashboard.main]

