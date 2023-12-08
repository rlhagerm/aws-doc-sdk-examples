// snippet-sourcedescription:[DeleteDataSource.java demonstrates how to delete an AWS AppSync data source.]
// snippet-keyword:[AWS SDK for Java v2]
// snippet-service:[AWS AppSync]

/*
   Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
   SPDX-License-Identifier: Apache-2.0
*/
package com.example.appsync;

//snippet-start:[appsync.java2.del_ds.main]
//snippet-start:[appsync.java2.del_ds.import]
import software.amazon.awssdk.regions.Region;
import software.amazon.awssdk.services.appsync.AppSyncClient;
import software.amazon.awssdk.services.appsync.model.AppSyncException;
import software.amazon.awssdk.services.appsync.model.DeleteDataSourceRequest;
//snippet-end:[appsync.java2.del_ds.import]

/**
 * Before running this Java V2 code example, set up your development environment, including your credentials.
 *
 * For more information, see the following documentation topic:
 *
 * https://docs.aws.amazon.com/sdk-for-java/latest/developer-guide/get-started.html
 */
public class DeleteDataSource {
    public static void main(String[] args) {
        final String usage = """

            Usage:    <apiId> <dsName>\s

            Where:
               apiId - the id of the API (You can get this value from the AWS Management Console).\s

               dsName - The name of the data source to delete.
            """;

        if (args.length != 2) {
            System.out.println(usage);
            System.exit(1);
        }

        String apiId = args[0];
        String dsName = args[1];
        AppSyncClient appSyncClient = AppSyncClient.builder()
                .region(Region.US_EAST_1)
                .build();
        deleteDS(appSyncClient, apiId, dsName) ;
    }

    public static void deleteDS( AppSyncClient appSyncClient, String apiId, String dsName) {
        try {
            DeleteDataSourceRequest request = DeleteDataSourceRequest.builder()
                .apiId(apiId)
                .name(dsName)
                .build();

            appSyncClient.deleteDataSource(request);
            System.out.println("The data source was deleted.");

        } catch (AppSyncException e) {
            System.out.println(e.getMessage());
            System.exit(1);
        }
    }
}
//snippet-end:[appsync.java2.del_ds.main]
