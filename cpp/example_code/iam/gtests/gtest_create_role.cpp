/*
   Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
   SPDX-License-Identifier: Apache-2.0
*/

#include <gtest/gtest.h>
#include "iam_samples.h"
#include "iam_gtests.h"

namespace AwsDocTest {
    // NOLINTNEXTLINE(readability-named-parameter)
    TEST_F(IAM_GTests, create_role) {
        Aws::String roleName = uuidName("role");
        auto result = AwsDoc::IAM::createIamRole(roleName, getAssumeRolePolicyJSON(),
                                                 *s_clientConfig);
        ASSERT_TRUE(result);

        deleteRole(roleName);
    }
} // namespace AwsDocTest
