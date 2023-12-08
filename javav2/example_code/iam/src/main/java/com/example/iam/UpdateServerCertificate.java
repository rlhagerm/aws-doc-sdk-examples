//snippet-sourcedescription:[UpdateServerCertificate.java demonstrates how to update the name of an AWS Identity and Access Management (IAM) server certificate.]
//snippet-keyword:[AWS SDK for Java v2]
//snippet-service:[IAM]
/*
   Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
   SPDX-License-Identifier: Apache-2.0
*/
package com.example.iam;

// snippet-start:[iam.java2.update_server_certificate.complete]
// snippet-start:[iam.java2.update_server_certificate.main]
// snippet-start:[iam.java2.update_server_certificate.import]
import software.amazon.awssdk.regions.Region;
import software.amazon.awssdk.services.iam.IamClient;
import software.amazon.awssdk.services.iam.model.IamException;
import software.amazon.awssdk.services.iam.model.UpdateServerCertificateRequest;
// snippet-end:[iam.java2.update_server_certificate.import]

/**
 * Before running this Java V2 code example, set up your development environment, including your credentials.
 *
 * For more information, see the following documentation topic:
 *
 * https://docs.aws.amazon.com/sdk-for-java/latest/developer-guide/get-started.html
 */
public class UpdateServerCertificate {
    public static void main(String[] args) {
        final String usage = """

            Usage:
                <curName> <newName>\s

            Where:
                curName - The current certificate name.\s
                newName - An updated certificate name.\s
            """;

        if (args.length != 2) {
            System.out.println(usage);
            System.exit(1);
        }

        String curName = args[0];
        String newName = args[1];
        Region region = Region.AWS_GLOBAL;
        IamClient iam = IamClient.builder()
            .region(region)
            .build();

        updateCertificate(iam, curName, newName) ;
        System.out.println("Done");
        iam.close();
    }
    public static void updateCertificate(IamClient iam, String curName, String newName) {
        try {
            UpdateServerCertificateRequest request = UpdateServerCertificateRequest.builder()
                .serverCertificateName(curName)
                .newServerCertificateName(newName)
                .build();

            iam.updateServerCertificate(request);
            System.out.printf("Successfully updated server certificate to name %s", newName);

        } catch (IamException e) {
            System.err.println(e.awsErrorDetails().errorMessage());
            System.exit(1);
        }
    }
}
// snippet-end:[iam.java2.update_server_certificate.main]
// snippet-end:[iam.java2.update_server_certificate.complete]
