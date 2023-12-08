//snippet-sourcedescription:[DeleteCluster.java demonstrates how to delete an Amazon Redshift cluster.]
//snippet-keyword:[AWS SDK for Java v2]
//snippet-service:[Amazon Redshift]

/*
   Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
   SPDX-License-Identifier: Apache-2.0
*/

package com.example.redshift;

// snippet-start:[redshift.java2.delete_cluster.main]
// snippet-start:[redshift.java2.delete_cluster.import]
import software.amazon.awssdk.regions.Region;
import software.amazon.awssdk.services.redshift.RedshiftClient;
import software.amazon.awssdk.services.redshift.model.DeleteClusterRequest;
import software.amazon.awssdk.services.redshift.model.DeleteClusterResponse;
import software.amazon.awssdk.services.redshift.model.RedshiftException;
// snippet-end:[redshift.java2.delete_cluster.import]

/**
 * Before running this Java V2 code example, set up your development environment, including your credentials.
 *
 * For more information, see the following documentation topic:
 *
 * https://docs.aws.amazon.com/sdk-for-java/latest/developer-guide/get-started.html
 */
public class DeleteCluster {
    public static void main(String[] args) {

        final String usage = """

            Usage:
                <clusterId>\s

            Where:
                clusterId - The id of the cluster to delete.\s
            """;

        if (args.length != 1) {
            System.out.println(usage);
            System.exit(1);
        }

        String clusterId = args[0];
        Region region = Region.US_WEST_2;
        RedshiftClient redshiftClient = RedshiftClient.builder()
            .region(region)
            .build();

        deleteRedshiftCluster(redshiftClient, clusterId);
        redshiftClient.close();
    }

    public static void deleteRedshiftCluster(RedshiftClient redshiftClient, String clusterId) {
        try {
            DeleteClusterRequest deleteClusterRequest = DeleteClusterRequest.builder()
                .clusterIdentifier(clusterId)
                .skipFinalClusterSnapshot(true)
                .build();

            DeleteClusterResponse response = redshiftClient.deleteCluster(deleteClusterRequest);
            System.out.println("The status is " + response.cluster().clusterStatus());

        } catch (RedshiftException e) {
            System.err.println(e.getMessage());
            System.exit(1);
        }
    }
}
// snippet-end:[redshift.java2.delete_cluster.main]
