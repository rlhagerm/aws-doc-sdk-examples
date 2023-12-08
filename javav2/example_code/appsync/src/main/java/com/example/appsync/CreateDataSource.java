// snippet-sourcedescription:[CreateDataSource.java demonstrates how to create an AWS AppSync data source that uses Amazon DynamoDB.]
// snippet-keyword:[AWS SDK for Java v2]
// snippet-service:[AWS AppSync]

/*
   Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
   SPDX-License-Identifier: Apache-2.0
*/

package com.example.appsync;

//snippet-start:[appsync.java2.create_ds.main]
//snippet-start:[appsync.java2.create_ds.import]
import software.amazon.awssdk.regions.Region;
import software.amazon.awssdk.services.appsync.AppSyncClient;
import software.amazon.awssdk.services.appsync.model.DynamodbDataSourceConfig;
import software.amazon.awssdk.services.appsync.model.CreateDataSourceRequest;
import software.amazon.awssdk.services.appsync.model.DataSourceType;
import software.amazon.awssdk.services.appsync.model.CreateDataSourceResponse;
import software.amazon.awssdk.services.appsync.model.AppSyncException;
//snippet-end:[appsync.java2.create_ds.import]

/**
 * Before running this Java V2 code example, set up your development environment, including your credentials.
 *
 * For more information, see the following documentation topic:
 *
 * https://docs.aws.amazon.com/sdk-for-java/latest/developer-guide/get-started.html
 */
public class CreateDataSource {

    public static void main(String[] args) {
        final String USAGE = """

            Usage:    <apiId> <name> <dsRole> <tableName>

            Where:
               apiId - The id of the API (You can get this value from the AWS Management Console).\s
               name - The name of the data source.\s
               dsRole - The AWS Identity and Access Management (IAM) service role for the data source.\s
               tableName - The name of the Amazon DynamoDB table used as the data source.\s
            """;

        if (args.length != 4) {
             System.out.println(USAGE);
             System.exit(1);
         }

        String apiId = args[0];
        String name = args[1];
        String dsRole = args[2];
        String tableName = args[3];
        AppSyncClient appSyncClient = AppSyncClient.builder()
                .region(Region.US_EAST_1)
                .build();

        String dsARN = createDS(appSyncClient, name, String.valueOf(Region.US_EAST_1), dsRole, apiId, tableName);
        System.out.println("The ARN of the data source is "+dsARN);
    }

    public static String createDS(AppSyncClient appSyncClient, String name, String reg, String dsRole, String apiVal, String tableName) {
        try {
            DynamodbDataSourceConfig config = DynamodbDataSourceConfig.builder()
                    .awsRegion(reg)
                    .tableName(tableName)
                    .versioned(true)
                    .build();

            CreateDataSourceRequest request = CreateDataSourceRequest.builder()
                    .description("Created using the AWS SDK for Java")
                    .apiId(apiVal)
                    .name(name)
                    .serviceRoleArn(dsRole)
                    .dynamodbConfig(config)
                    .type(DataSourceType.AMAZON_DYNAMODB)
                    .build();

            CreateDataSourceResponse response = appSyncClient.createDataSource(request);
            return response.dataSource().dataSourceArn();

        } catch (AppSyncException e) {
            System.out.println(e.getMessage());
            System.exit(1);
        }
        return "";
    }
}
//snippet-end:[appsync.java2.create_ds.main]
