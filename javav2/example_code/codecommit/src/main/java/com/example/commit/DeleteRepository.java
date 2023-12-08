// snippet-comment:[These are tags for the AWS doc team's sample catalog. Do not remove.]
// snippet-sourcedescription:[DeleteRepository.java demonstrates how to delete a repository.]
// snippet-keyword:[AWS SDK for Java v2]
// snippet-service:[AWS CodeCommit]
/*
   Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
   SPDX-License-Identifier: Apache-2.0
*/

package com.example.commit;

// snippet-start:[codecommit.java2.del_repo.main]
// snippet-start:[codecommit.java2.del_repo.import]
import software.amazon.awssdk.regions.Region;
import software.amazon.awssdk.services.codecommit.CodeCommitClient;
import software.amazon.awssdk.services.codecommit.model.CodeCommitException;
import software.amazon.awssdk.services.codecommit.model.DeleteRepositoryRequest;
// snippet-end:[codecommit.java2.del_repo.import]

/**
 * To run this Java V2 code example, ensure that you have setup your development environment, including your credentials.
 *
 * For information, see this documentation topic:
 *
 * https://docs.aws.amazon.com/sdk-for-java/latest/developer-guide/get-started.html
 */

public class DeleteRepository {
    public static void main(String[] args) {
        final String USAGE = """

            Usage:
                <repoName>\s

            Where:
                repoName - the name of the repository.
            """;

        if (args.length != 1) {
            System.out.println(USAGE);
            System.exit(1);
        }

        String repoName = args[0];
        Region region = Region.US_EAST_1;
        CodeCommitClient codeCommitClient = CodeCommitClient.builder()
            .region(region)
            .build();

        deleteRepo(codeCommitClient, repoName);
        codeCommitClient.close();
    }

    public static void deleteRepo(CodeCommitClient codeCommitClient, String repoName) {
        try {
            DeleteRepositoryRequest deleteRepositoryRequest = DeleteRepositoryRequest.builder()
                .repositoryName(repoName)
                .build();

            codeCommitClient.deleteRepository(deleteRepositoryRequest);

        } catch (CodeCommitException e) {
            System.err.println(e.getMessage());
            System.exit(1);
        }
    }
}
// snippet-end:[codecommit.java2.del_repo.main]