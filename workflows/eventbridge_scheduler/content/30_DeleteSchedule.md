









IAmazonScheduler.DeleteScheduleAsync Method (DeleteScheduleRequest, CancellationToken) | AWS SDK for .NET V3




AWS SDK Version 3 for .NET
API Reference



AWS services or capabilities described in AWS Documentation may vary by region/location. Click Getting Started with Amazon AWS to see specific differences applicable to the China (Beijing) Region.




IAmazonScheduler.DeleteScheduleAsync (DeleteScheduleRequest, CancellationToken)
Method








Search: 


Entire Site
Articles &amp; Tutorials
Documentation
Documentation - This Product
Documentation - This Guide
Release Notes
Sample Code &amp; Libraries
















            Deletes the specified schedule.
            
Note:This is an asynchronous operation using the standard naming convention for .NET 4.5 or higher. For .NET 3.5 the operation is implemented as a pair of methods using the standard naming convention of BeginDeleteSchedule and EndDeleteSchedule.


Namespace: Amazon.SchedulerAssembly: AWSSDK.Scheduler.dllVersion: 3.x.y.z



Syntax






C#




public abstract Task&lt;DeleteScheduleResponse&gt; DeleteScheduleAsync(
         DeleteScheduleRequest request,
         CancellationToken cancellationToken
)







Parameters



request


Type: Amazon.Scheduler.Model.DeleteScheduleRequest
Container for the necessary parameters to execute the DeleteSchedule service method.




cancellationToken


Type: System.Threading.CancellationToken

                A cancellation token that can be used by other objects or threads to receive notice of cancellation.
            





Return Value

Type: Task&lt;DeleteScheduleResponse&gt;
The response from the DeleteSchedule service method, as returned by Scheduler.




Exceptions






ExceptionCondition


ConflictException


            Updating or deleting the resource can cause an inconsistent state.
            




InternalServerException


            Unexpected error encountered while processing the request.
            




ResourceNotFoundException


            The request references a resource which does not exist.
            




ThrottlingException


            The request was denied due to request throttling.
            




ValidationException


            The input fails to satisfy the constraints specified by an AWS service.
            









Version Information



.NET: Supported in: 8.0 and newer, Core 3.1
.NET Standard: Supported in: 2.0
.NET Framework: Supported in: 4.5 and newer





See Also



REST API Reference for DeleteSchedule Operation




Link to this page
&nbsp;
Did this page help you?&nbsp;&nbsp;Yes&nbsp;&nbsp;No&nbsp;&nbsp;&nbsp;Tell us about it...



jQuery.noConflict();





jQuery(function ($) {
var host = parseUri($(window.parent.location).attr('href')).host;
if (AWSHelpObj.showRegionalDisclaimer(host)) {
$("div#regionDisclaimer").css("display", "block");
} else {
$("div#regionDisclaimer").remove();
}
AWSHelpObj.setAssemblyVersion("../../items/_sdk-versions.json", "Scheduler");
});






SyntaxHighlighter.all()

