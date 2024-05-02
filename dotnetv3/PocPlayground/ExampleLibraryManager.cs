// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved. 
// SPDX-License-Identifier: Apache-2.0

namespace PocPlayground;

public static class ExampleLibraryManager
{
    public static List<ExampleModel> GetExampleModels()
    {
        var exampleModels = new List<ExampleModel>();
        var s3LocModel = new ExampleModel()
        {
            Title = "Work with Amazon S3 object lock features using an AWS SDK",
            Summary =
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

public class ExampleModel
{
    public string Title { get; set; }
    public string Summary { get; set; }
    public string Sdk { get; set; }
    public string Version { get; set; }
    public string Location { get; set; }
    public string Readme { get; set; }
    public string RepoUrl { get; set; }
}