// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved. 
// SPDX-License-Identifier: Apache-2.0

using Accessibility;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace PocPlayground;

public static class ExampleLibraryManager
{
    public static List<ExampleModel> GetExampleModels()
    {
        var exampleModels = new List<ExampleModel>();
        var s3LocModel = new ExampleModel()
        {
            Title = "Work with Amazon S3 object lock features using an AWS SDK",
            Synopsis = 
                "The following code examples show how to work with S3 object lock features.",
            Sdk = ".NET",
            Version = "3",
            Location = "",
            Readme = "",
            RepoUrl = ""
        };
        exampleModels.Add(s3LocModel);
        return exampleModels;
    }

    public static List<ExampleModel> GetExampleModelsFromYaml()
    {
        var exampleModels = new List<ExampleModel>();

        var s3LocModel = new ExampleModel()
        {
            Title = "Work with Amazon S3 object lock features using an AWS SDK",
            Synopsis = 
                "The following code examples show how to work with S3 object lock features.",
            Sdk = ".NET",
            Version = "3",
            Location = "",
            Readme = "",
            RepoUrl = ""
        };
        exampleModels.Add(s3LocModel);

        var yamlFile =
            @"C:\Work\Repos\Forks\aws-doc-sdk-examples\.doc_gen\metadata\s3_metadata.yaml";

        string yamlText = System.IO.File.ReadAllText(yamlFile);

        yamlText = yamlText.Replace("&S3;", "AWS S3");
        yamlText = yamlText.Replace("&AWS;", "AWS");

        var deserializer = new DeserializerBuilder()
            .IgnoreUnmatchedProperties()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)  // see height_in_inches in sample yml 
            .Build();

        var examples = deserializer.Deserialize<Dictionary<string, ExampleModel>>(yamlText);

        foreach (var model in examples)
        {
            model.Value.Synopsis = "The following code examples show how to " +
                                   model.Value.Synopsis;
            exampleModels.Add(model.Value);
        }


        return exampleModels;
    }

    public static void RunExample(string filePath)
    {
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo()
        {
            WorkingDirectory = @"C:\Work\Repos\Forks\aws-doc-sdk-examples\dotnetv3\S3\scenarios\S3ObjectLockScenario\S3ObjectLockWorkflow"
        };
        startInfo.FileName = "cmd.exe";
        startInfo.Arguments = @"/C dotnet run";
        process.StartInfo = startInfo;
        process.Start();
        process.WaitForExit();
    }

    public static void ViewCode(string filePath)
    {
        System.Diagnostics.Process.Start(
            "explorer","\"" +
                       @"C:\Work\Repos\Forks\aws-doc-sdk-examples\dotnetv3\S3\scenarios\S3ObjectLockScenario\S3ObjectLockScenario.sln" + "\"");
    }
}

