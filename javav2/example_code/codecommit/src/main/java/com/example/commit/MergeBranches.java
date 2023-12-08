// snippet-comment:[These are tags for the AWS doc team's sample catalog. Do not remove.]
// snippet-sourcedescription:[MergeBranches.java demonstrates how to merge two branches.]
// snippet-keyword:[AWS SDK for Java v2]
// snippet-service:[AWS CodeCommit]

/*
   Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
   SPDX-License-Identifier: Apache-2.0
*/
package com.example.commit;

// snippet-start:[codecommit.java2.merge.main]
// snippet-start:[codecommit.java2.merge.import]
import software.amazon.awssdk.regions.Region;
import software.amazon.awssdk.services.codecommit.CodeCommitClient;
import software.amazon.awssdk.services.codecommit.model.CodeCommitException;
import software.amazon.awssdk.services.codecommit.model.MergeBranchesByFastForwardRequest;
import software.amazon.awssdk.services.codecommit.model.MergeBranchesByFastForwardResponse;
// snippet-end:[codecommit.java2.merge.import]

/**
 * To run this Java V2 code example, ensure that you have setup your development environment, including your credentials.
 *
 * For information, see this documentation topic:
 *
 * https://docs.aws.amazon.com/sdk-for-java/latest/developer-guide/get-started.html
 */
public class MergeBranches {

    public static void main(String[] args) {
        final String USAGE = """

            Usage:
                <repoName> <targetBranch> <sourceReference> <destinationCommitId>

            Where:
                repoName - the name of the repository.
                targetBranch -  the branch where the merge is applied.
                sourceReference  - the branch of the repository that contains the changes.
                destinationCommitId  - a full commit ID.
            """;

        if (args.length != 4) {
            System.out.println(USAGE);
            System.exit(1);
        }

        String repoName = args[0];
        String targetBranch = args[1];
        String sourceReference = args[2];
        String destinationCommitId = args[3];

        Region region = Region.US_EAST_1;
        CodeCommitClient codeCommitClient = CodeCommitClient.builder()
                .region(region)
                .build();

        merge(codeCommitClient, repoName, targetBranch, sourceReference, destinationCommitId) ;
        codeCommitClient.close();
    }

    public static void merge(CodeCommitClient codeCommitClient,
                      String repoName,
                      String targetBranch,
                      String sourceReference,
                      String destinationCommitId) {

        try {
            MergeBranchesByFastForwardRequest fastForwardRequest = MergeBranchesByFastForwardRequest.builder()
                .destinationCommitSpecifier(destinationCommitId)
                .targetBranch(targetBranch)
                .sourceCommitSpecifier(sourceReference)
                .repositoryName(repoName)
                .build();

            MergeBranchesByFastForwardResponse response = codeCommitClient.mergeBranchesByFastForward(fastForwardRequest);
            System.out.println("The commit id is "+response.commitId());

        } catch (CodeCommitException e) {
            System.err.println(e.getMessage());
            System.exit(1);
        }
    }
}
// snippet-end:[codecommit.java2.merge.main]
