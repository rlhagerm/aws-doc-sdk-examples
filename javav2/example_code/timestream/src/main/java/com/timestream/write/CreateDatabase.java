//snippet-sourcedescription:[CreateDatabase.java demonstrates how to create a database.]
//snippet-keyword:[AWS SDK for Java v2]
//snippet-keyword:[Amazon Timestream]

/*
   Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
   SPDX-License-Identifier: Apache-2.0
*/

package com.timestream.write;

//snippet-start:[timestream.java2.create_db.main]
//snippet-start:[timestream.java2.create_db.import]
import software.amazon.awssdk.regions.Region;
import software.amazon.awssdk.services.timestreamwrite.TimestreamWriteClient;
import software.amazon.awssdk.services.timestreamwrite.model.CreateDatabaseRequest;
import software.amazon.awssdk.services.timestreamwrite.model.TimestreamWriteException;
//snippet-end:[timestream.java2.create_db.import]

/**
 * Before running this SDK for Java (v2) code example, set up your development environment, including your credentials.
 *
 * For more information, see the following documentation:
 *
 * https://docs.aws.amazon.com/sdk-for-java/latest/developer-guide/get-started.html
 */

public class CreateDatabase {
    public static void main(String[] args) {
        final String usage = """

            Usage:    <dbName>\s

            Where:
               dbName - The name of the database.
            """;

        if (args.length != 1) {
            System.out.println(usage);
            System.exit(1);
        }

        String dbName = args[0];
        TimestreamWriteClient timestreamWriteClient = TimestreamWriteClient.builder()
            .region(Region.US_EAST_1)
            .build();

        createNewDatabase(timestreamWriteClient, dbName);
        timestreamWriteClient.close();
    }

    public static void createNewDatabase(TimestreamWriteClient timestreamWriteClient, String dbName) {
        try {
            System.out.println("Creating database");
            CreateDatabaseRequest request = CreateDatabaseRequest.builder()
                .databaseName(dbName)
                .build();

            timestreamWriteClient.createDatabase(request);
            System.out.println("Database " + dbName + " created successfully.");

        } catch (TimestreamWriteException e) {
            System.err.println(e.awsErrorDetails().errorMessage());
            System.exit(1);
        }
    }
}
//snippet-end:[timestream.java2.create_db.main]
