//snippet-sourcedescription:[WriteData.java demonstrates how to write data into a table.]
//snippet-keyword:[AWS SDK for Java v2]
//snippet-keyword:[Amazon Timestream]

/*
   Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
   SPDX-License-Identifier: Apache-2.0
*/

package com.timestream.write;

//snippet-start:[timestream.java2.write_table.main]
//snippet-start:[timestream.java2.write_table.import]
import software.amazon.awssdk.regions.Region;
import software.amazon.awssdk.services.timestreamwrite.TimestreamWriteClient;
import software.amazon.awssdk.services.timestreamwrite.model.Dimension;
import software.amazon.awssdk.services.timestreamwrite.model.MeasureValueType;
import software.amazon.awssdk.services.timestreamwrite.model.Record;
import software.amazon.awssdk.services.timestreamwrite.model.RejectedRecord;
import software.amazon.awssdk.services.timestreamwrite.model.RejectedRecordsException;
import software.amazon.awssdk.services.timestreamwrite.model.WriteRecordsRequest;
import software.amazon.awssdk.services.timestreamwrite.model.WriteRecordsResponse;
import java.util.ArrayList;
import java.util.List;
//snippet-end:[timestream.java2.write_table.import]

/**
 * Before running this SDK for Java (v2) code example, set up your development environment, including your credentials.
 *
 * For more information, see the following documentation:
 *
 * https://docs.aws.amazon.com/sdk-for-java/latest/developer-guide/get-started.html
 */
public class WriteData {
    public static void main(String[] args){
        final String usage = """

            Usage:    <dbName> <newTable>

            Where:
               dbName - The name of the database.
               newTable - The name of the table.
            """;

        if (args.length != 2) {
            System.out.println(usage);
            System.exit(1);
        }

        String dbName = args[0];
        String tableName =  args[1];
        TimestreamWriteClient timestreamWriteClient = TimestreamWriteClient.builder()
            .region(Region.US_EAST_1)
            .build();

        writeRecords(timestreamWriteClient, dbName, tableName);
    }

    public static void writeRecords(TimestreamWriteClient timestreamWriteClient,  String dbName,  String tableName) {

        System.out.println("Writing records");
        List<Record> records = new ArrayList<>();
        final long time = System.currentTimeMillis();

        List<Dimension> dimensions = new ArrayList<>();
        Dimension region = Dimension.builder()
            .name("region")
            .value("us-east-1")
            .build();

       Dimension az = Dimension.builder()
            .name("az")
            .value("az1").build();

       Dimension hostname = Dimension.builder()
            .name("hostname")
            .value("host1")
            .build();

       dimensions.add(region);
       dimensions.add(az);
       dimensions.add(hostname);

       Record cpuUtilization = Record.builder()
           .dimensions(dimensions)
           .measureValueType(MeasureValueType.DOUBLE)
           .measureName("cpu_utilization")
           .measureValue("13.5")
           .time(String.valueOf(time))
           .build();

       Record memoryUtilization = Record.builder()
           .dimensions(dimensions)
           .measureValueType(MeasureValueType.DOUBLE)
           .measureName("memory_utilization")
           .measureValue("40")
           .time(String.valueOf(time))
           .build();

        records.add(cpuUtilization);
        records.add(memoryUtilization);

        WriteRecordsRequest writeRecordsRequest = WriteRecordsRequest.builder()
           .databaseName(dbName)
           .tableName(tableName)
           .records(records)
           .build();

       try {
           WriteRecordsResponse writeRecordsResponse = timestreamWriteClient.writeRecords(writeRecordsRequest);
           System.out.println("WriteRecords Status: " + writeRecordsResponse.sdkHttpResponse().statusCode());

       } catch (RejectedRecordsException e) {
           System.out.println("RejectedRecords: " + e);
           for (RejectedRecord rejectedRecord : e.rejectedRecords()) {
               System.out.println("Rejected Index " + rejectedRecord.recordIndex() + ": "
                       + rejectedRecord.reason());
           }
       }
       System.out.println("Other records were written successfully.");
    }
    //snippet-end:[timestream.java2.write_table.main]
}
