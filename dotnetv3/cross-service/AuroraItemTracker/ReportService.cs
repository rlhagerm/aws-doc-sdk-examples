// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
// SPDX-License-Identifier:  Apache-2.0

using System.Globalization;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using Amazon.SimpleEmailV2;
using Amazon.SimpleEmailV2.Model;
using CsvHelper;
using CsvHelper.Configuration;

namespace AuroraItemTracker;

/// <summary>
/// Class for sending work item reports using the Amazon SES service.
/// </summary>
public class ReportService
{
    private readonly IAmazonSimpleEmailServiceV2 _amazonSESService;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Constructor that uses the injected Amazon SES client.
    /// </summary>
    /// <param name="amazonSESService">Amazon SES client.</param>
    public ReportService(IAmazonSimpleEmailServiceV2 amazonSESService, IConfiguration configuration)
    {
        _amazonSESService = amazonSESService;
        _configuration = configuration;
    }

    ///// <summary>
    ///// Get a CSV report from a collection of work items.
    ///// </summary>
    ///// <param name="workItems"></param>
    ///// <returns></returns>
    //public async Task<MemoryStream> GetCsvReport(IList<WorkItem> workItems)
    //{


    //}

    /// <summary>
    /// Send the report to an email address. Both the sender and recipient must be validated email addresses if using the SES Sandbox.
    /// </summary>
    /// <param name="workItems"></param>
    /// <param name="emailAddress"></param>
    /// <returns></returns>
    public async Task<string> SendReport3(IList<WorkItem> workItems, string emailAddress)
    {
        await using var memoryStream = new MemoryStream();
        await using var streamWriter = new StreamWriter(memoryStream);
        await using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

        csvWriter.WriteHeader<WorkItem>();
        await csvWriter.NextRecordAsync();
        await csvWriter.WriteRecordsAsync(workItems);
        await csvWriter.FlushAsync();
        await streamWriter.FlushAsync();
        memoryStream.Position = 0;

        var fromEmailAddress = _configuration["EmailSourceAddress"];
        var testMailMessage = new MailMessage(
            fromEmailAddress,
            emailAddress,
            "Item Tracker Report: Active Work Items",
            "This email was sent by Amazon SES, and includes an attached report of the current active work items." +
            $"There are {workItems.Count} as of {System.DateTime.Now.ToString("g")}.");

        testMailMessage.Attachments.Add(new Attachment(memoryStream, "activeWorkItems.csv"));

        var dataStream = FromMailMessageToMemoryStream(testMailMessage);

        var response = await _amazonSESService.SendEmailAsync(
            new SendEmailRequest
            {
                Destination = new Destination
                {
                    ToAddresses = new List<string> { emailAddress }
                },
                Content = new EmailContent()
                {
                    Raw = new RawMessage()
                    {
                        Data = dataStream
                    }
                },
            });

        await dataStream.DisposeAsync();
        return response.MessageId;
    }

    /// <summary>
    /// Helper method to convert a MailMessage to a stream compatible with a Amazon SES raw email method.
    /// This method is compatible with the .NET6 version of MailWriter. Other versions of .NET may need to use different constructors. 
    /// </summary>
    /// <param name="message">The MailMessage to convert.</param>
    /// <returns>A memory stream for the email.</returns>
    public MemoryStream FromMailMessageToMemoryStream(MailMessage message)
    {
        Assembly assembly = typeof(SmtpClient).Assembly;
        Type mailWriterType = assembly.GetType("System.Net.Mail.MailWriter")!;
        // Use the MailWriter type to write the MailMessage to a properly encoded MemoryStream.
        MemoryStream stream = new MemoryStream();
        ConstructorInfo mailWriterConstructor = mailWriterType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { typeof(Stream), typeof(Boolean) }, null)!;
        MethodInfo sendMethod = typeof(MailMessage).GetMethod("Send", BindingFlags.Instance | BindingFlags.NonPublic)!;
        MethodInfo closeMethod = mailWriterType.GetMethod("Close", BindingFlags.Instance | BindingFlags.NonPublic)!;

        object mailWriter = mailWriterConstructor.Invoke(new object[] { stream, true });
        _ = sendMethod.Invoke(message, BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { mailWriter, true, true }, null);
        _ = closeMethod.Invoke(mailWriter, BindingFlags.Instance | BindingFlags.NonPublic, null, new object[] { }, null);

        return stream;
    }

    /// <summary>
    /// Send the report to an email address.
    /// </summary>
    /// <param name="workItems"></param>
    /// <param name="emailAddress"></param>
    /// <returns></returns>
    public async Task SendReport(IList<WorkItem> workItems, string emailAddress)
    {
        var response = await _amazonSESService.SendEmailAsync(
            new SendEmailRequest
            {
                Destination = new Destination
                {
                    ToAddresses = new List<string>{emailAddress}
                },
                Content = new EmailContent()
                {
                    Simple = new Message()
                    {
                        Body = new Body
                        {
                            Text = new Content
                            {
                                Charset = "UTF-8",
                                Data = "Active work items report is attached to this email."
                            }
                        },
                        Subject = new Content
                        {
                            Charset = "UTF-8",
                            Data = "Active Work Items"
                        }
                    }
                },
            });
    }
}