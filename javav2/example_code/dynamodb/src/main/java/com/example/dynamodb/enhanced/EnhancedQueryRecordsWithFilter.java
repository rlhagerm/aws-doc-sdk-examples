//snippet-sourcedescription:[EnhancedPutItem.java demonstrates how to put an item into an Amazon DynamoDB table by using the enhanced client.]
//snippet-keyword:[SDK for Java v2]
//snippet-service:[Amazon DynamoDB]

/*
   Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
   SPDX-License-Identifier: Apache-2.0
*/
package com.example.dynamodb.enhanced;

// snippet-start:[dynamodb.java2.mapping.queryfilter.main]
// snippet-start:[dynamodb.java2.mapping.queryfilter.import]
import com.example.dynamodb.Customer;
import software.amazon.awssdk.enhanced.dynamodb.DynamoDbEnhancedClient;
import software.amazon.awssdk.enhanced.dynamodb.DynamoDbTable;
import software.amazon.awssdk.enhanced.dynamodb.Expression;
import software.amazon.awssdk.enhanced.dynamodb.Key;
import software.amazon.awssdk.enhanced.dynamodb.TableSchema;
import software.amazon.awssdk.enhanced.dynamodb.model.QueryConditional;
import software.amazon.awssdk.regions.Region;
import software.amazon.awssdk.services.dynamodb.DynamoDbClient;
import software.amazon.awssdk.services.dynamodb.model.AttributeValue;
import software.amazon.awssdk.services.dynamodb.model.DynamoDbException;

import java.util.HashMap;
import java.util.Map;
// snippet-end:[dynamodb.java2.mapping.queryfilter.import]

public class EnhancedQueryRecordsWithFilter {
    // Query the Customer table using a filter.
    public static void main(String[] args) {
        Region region = Region.US_EAST_1;
        DynamoDbClient ddb = DynamoDbClient.builder()
            .region(region)
            .build();

        DynamoDbEnhancedClient enhancedClient = DynamoDbEnhancedClient.builder()
            .dynamoDbClient(ddb)
            .build();

        queryTableFilter(enhancedClient);
        ddb.close();
    }

    public static Integer queryTableFilter(DynamoDbEnhancedClient enhancedClient) {
        Integer countOfCustomers = 0;
        try {
            DynamoDbTable<Customer> mappedTable = enhancedClient.table("Customer", TableSchema.fromBean(Customer.class));
            AttributeValue att = AttributeValue.builder()
                .s("Tom red")
                .build();

            Map<String, AttributeValue> expressionValues = new HashMap<>();
            expressionValues.put(":value", att);
            Expression expression = Expression.builder()
                .expression("custName = :value")
                .expressionValues(expressionValues)
                .build();

            // Create a QueryConditional object to query by partitionValue.
            // Since the Customer table has a sort key attribute (email), we can use an expression
            // to filter the query results if multiple items have the same partition key value.
            QueryConditional queryConditional = QueryConditional
                .keyEqualTo(Key.builder().partitionValue("id101")
                    .build());

            for (Customer customer : mappedTable.query(
                r -> r.queryConditional(queryConditional)
                    .filterExpression(expression)
            ).items()) {
                countOfCustomers++;
                System.out.println(customer);
            }

        } catch (DynamoDbException e) {
            System.err.println(e.getMessage());
            System.exit(1);
        }
        System.out.println("Done");
        return countOfCustomers;
    }
}
 // snippet-end:[dynamodb.java2.mapping.queryfilter.main]