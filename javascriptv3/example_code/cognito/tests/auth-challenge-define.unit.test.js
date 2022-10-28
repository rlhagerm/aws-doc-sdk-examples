/**
 * Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
 * SPDX-License-Identifier: Apache-2.0
 */

import { testEqual } from "../../libs/utils/util-test.js";
import { handler } from "../scenarios/lambda-triggers/functions/auth-challenge-define.mjs";

describe("auth-challenge-define", () => {
  it(
    "should return a password verifier challenge if the challenge name is SRP_A",
    testEqual(
      expect.objectContaining({
        response: {
          issueTokens: false,
          failAuthentication: false,
          challengeName: "PASSWORD_VERIFIER",
        },
      }),
      handler({
        response: {},
        request: { session: [{ challengeName: "SRP_A" }] },
      })
    )
  );

  it(
    "should return a custom challenge if the password verifier challenge was successful",
    testEqual(
      expect.objectContaining({
        response: {
          issueTokens: false,
          failAuthentication: false,
          challengeName: "CUSTOM_CHALLENGE",
        },
      }),
      handler({
        request: {
          session: [
            {},
            { challengeName: "PASSWORD_VERIFIER", challengeResult: true },
          ],
        },
        response: {},
      })
    )
  );

  it(
    "should return a custom challenge if we're on the 4th challenge and the last challenge was successful",
    testEqual(
      expect.objectContaining({
        response: {
          issueTokens: false,
          failAuthentication: false,
          challengeName: "CUSTOM_CHALLENGE",
        },
      }),
      handler({
        request: {
          session: [
            {},
            {},
            { challengeName: "CUSTOM_CHALLENGE", challengeResult: true },
          ],
        },
        response: {},
      })
    )
  );

  it(
    "should return tokens if the last challenge was successful",
    testEqual(
      expect.objectContaining({
        response: {
          issueTokens: true,
          failAuthentication: false,
        },
      }),
      handler({
        request: {
          session: [
            {},
            {},
            {},
            { challengeName: "CUSTOM_CHALLENGE", challengeResult: true },
          ],
        },
        response: {},
      })
    )
  );

  it(
    "should fail authentication if there's no matching definition",
    testEqual(
      expect.objectContaining({
        response: {
          issueTokens: false,
          failAuthentication: true,
        },
      }),
      handler({
        request: {
          session: [
            {},
            {},
            {},
            {},
            { challengeName: "CUSTOM_CHALLENGE", challengeResult: true },
          ],
        },
        response: {},
      })
    )
  );
});
