//snippet-sourcedescription:[ListTemplates.java demonstrates how to list Amazon QuickSight templates.]
//snippet-keyword:[AWS SDK for Java v2]
//snippet-service:[Amazon QuickSight]

/*
   Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
   SPDX-License-Identifier: Apache-2.0
*/

package com.example.quicksight;

// snippet-start:[quicksight.java2.list_templates.main]
// snippet-start:[quicksight.java2.list_templates.import]
import software.amazon.awssdk.regions.Region;
import software.amazon.awssdk.services.quicksight.QuickSightClient;
import software.amazon.awssdk.services.quicksight.model.ListTemplatesRequest;
import software.amazon.awssdk.services.quicksight.model.ListTemplatesResponse;
import software.amazon.awssdk.services.quicksight.model.TemplateSummary;
import software.amazon.awssdk.services.quicksight.model.QuickSightException;
import java.util.List;
// snippet-end:[quicksight.java2.list_templates.import]

/**
 * Before running this Java V2 code example, set up your development environment, including your credentials.
 *
 * For more information, see the following documentation topic:
 *
 * https://docs.aws.amazon.com/sdk-for-java/latest/developer-guide/get-started.html
 */
public class ListTemplates {
    public static void main(String[] args) {
        final String usage = """

            Usage:    <account>

            Where:
               account - The ID of the AWS account.

            """;

        if (args.length != 1) {
            System.out.println(usage);
            System.exit(1);
        }

        String account = args[0];
        QuickSightClient qsClient = QuickSightClient.builder()
            .region(Region.US_EAST_1)
            .build();

        listAllTemplates(qsClient, account);
        qsClient.close();
    }

    public static void listAllTemplates(QuickSightClient qsClient, String account) {
        try {
            ListTemplatesRequest templateRequest = ListTemplatesRequest.builder()
                .awsAccountId(account)
                .maxResults(20)
                .build();

            ListTemplatesResponse res = qsClient.listTemplates(templateRequest);
            List<TemplateSummary> templateSummaries = res.templateSummaryList();
            for (TemplateSummary template : templateSummaries) {
                System.out.println("Template ARN: " + template.arn());
                System.out.println("Template Id: " + template.templateId());
                System.out.println("Template Name: " + template.name());
            }

        } catch (QuickSightException e) {
            System.err.println(e.awsErrorDetails().errorMessage());
            System.exit(1);
        }
    }
}
// snippet-end:[quicksight.java2.list_templates.main]

