//snippet-sourcedescription:[DescribeSolution.java demonstrates how to describe an Amazon Personalize solution.]
//snippet-keyword:[AWS SDK for Java v2]
//snippet-service:[Amazon Personalize]

/*
   Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
   SPDX-License-Identifier: Apache-2.0
*/

package com.example.personalize;

//snippet-start:[personalize.java2.describe_solution.import]
import software.amazon.awssdk.regions.Region;
import software.amazon.awssdk.services.personalize.PersonalizeClient;
import software.amazon.awssdk.services.personalize.model.DescribeSolutionRequest;
import software.amazon.awssdk.services.personalize.model.DescribeSolutionResponse;
import software.amazon.awssdk.services.personalize.model.PersonalizeException;
//snippet-end:[personalize.java2.describe_solution.import]

/**
 * To run this Java V2 code example, ensure that you have setup your development environment, including your credentials.
 *
 * For information, see this documentation topic:
 *
 * https://docs.aws.amazon.com/sdk-for-java/latest/developer-guide/get-started.html
 */
public class DescribeSolution {

    public static void main(String[] args) {

        final String USAGE = """

            Usage:
                DescribeSolution <solutionArn>

            Where:
                solutionArn - The ARN of the solution.

            """;

        if (args.length < 1) {
            System.out.println(USAGE);
            System.exit(1);
        }

        String solutionArn = args[0];
        Region region = Region.US_EAST_1;
        PersonalizeClient personalizeClient = PersonalizeClient.builder()
            .region(region)
            .build();

        describeSpecificSolution(personalizeClient, solutionArn);
        personalizeClient.close();
    }

    //snippet-start:[personalize.java2.describe_solution.main]
   public static void describeSpecificSolution(PersonalizeClient personalizeClient, String solutionArn) {

       try {
           DescribeSolutionRequest solutionRequest = DescribeSolutionRequest.builder()
               .solutionArn(solutionArn)
               .build();

           DescribeSolutionResponse response = personalizeClient.describeSolution(solutionRequest);
           System.out.println("The Solution name is "+response.solution().name());

       } catch (PersonalizeException e) {
           System.err.println(e.awsErrorDetails().errorMessage());
           System.exit(1);
       }
   }
   //snippet-end:[personalize.java2.describe_solution.main]
}
