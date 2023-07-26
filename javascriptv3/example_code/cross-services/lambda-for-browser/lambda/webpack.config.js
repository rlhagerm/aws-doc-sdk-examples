const config = {
  externalsType: "module",
  mode: "production",
  experiments: {
    outputModule: true,
  },
  output: {
    filename: "index.mjs",
    library: {
      type: "module",
    },
  },
  externals: [
    "@aws-sdk/client-cognito-identity",
    "@aws-sdk/client-dynamodb",
    "@aws-sdk/client-iam",
    "@aws-sdk/credential-provider-cognito-identity",
    "@aws-sdk/lib-dynamodb",
  ],
};

export default config;
