//snippet-sourcedescription:[GetId.java demonstrates how to retrieve the client ID from an identity provider.]
//snippet-keyword:[AWS SDK for Java v2]
//snippet-keyword:[Amazon Cognito]
/*
   Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
   SPDX-License-Identifier: Apache-2.0
*/

package com.example.cognito;

//snippet-start:[cognito.java2.GetID.main]
//snippet-start:[cognito.java2.GetId.import]
import software.amazon.awssdk.regions.Region;
import software.amazon.awssdk.services.cognitoidentity.CognitoIdentityClient;
import software.amazon.awssdk.services.cognitoidentity.model.GetIdRequest;
import software.amazon.awssdk.services.cognitoidentity.model.GetIdResponse;
import software.amazon.awssdk.services.cognitoidentityprovider.model.CognitoIdentityProviderException;
//snippet-end:[cognito.java2.GetId.import]

/**
 * Before running this Java V2 code example, set up your development environment, including your credentials.
 *
 * For more information, see the following documentation topic:
 *
 * https://docs.aws.amazon.com/sdk-for-java/latest/developer-guide/get-started.html
 */
public class GetId {
    public static void main(String[] args) {
        final String usage = """

            Usage:
                <identityPoolId>

            Where:
                identityPoolId - The GUID value of your identity pool.

            Example:
                GetId us-east-1:00eb915b-c521-417b-af0d-ebad008axxxx
            """;

        if (args.length != 1) {
            System.out.println(usage);
            System.exit(1);
        }

        String identityPoolId = args[0];
        CognitoIdentityClient cognitoClient = CognitoIdentityClient.builder()
            .region(Region.US_EAST_1)
            .build();

        getClientID(cognitoClient, identityPoolId);
        cognitoClient.close();
    }

    public static void getClientID(CognitoIdentityClient cognitoClient, String identityPoolId) {
        try {
            GetIdRequest request = GetIdRequest.builder()
                .identityPoolId(identityPoolId)
                .build();

            GetIdResponse response = cognitoClient.getId(request);
            System.out.println("Identity ID " + response.identityId());

        } catch (CognitoIdentityProviderException e) {
            System.err.println(e.awsErrorDetails().errorMessage());
            System.exit(1);
        }
    }
}
//snippet-end:[cognito.java2.GetID.main]
