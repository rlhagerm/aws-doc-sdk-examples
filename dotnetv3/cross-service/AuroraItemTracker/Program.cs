// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
// SPDX-License-Identifier:  Apache-2.0

using System.Net;
using Amazon.RDSDataService;
using AuroraItemTracker;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging.Debug;

// todo: configure logging.
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAWSService<IAmazonRDSDataService>();
builder.Services.AddScoped<WorkItemService>();

// Add services to the container.
builder.Services.AddAuthorization();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", (HttpContext httpContext) =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = summaries[Random.Shared.Next(summaries.Length)]
                })
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast");

app.MapGet("/testrds", (WorkItemService workItemService) =>
    {
        var result = workItemService.TestRequest();

        return result;
    })
    .WithName("TestRDS");

/*app.MapGet("/items", (RDSDataClientWrapper wrapper) =>
{
    var result = wrapper.TestRequest();

    return result;
})
    .WithName("Items");

*/

app.MapGet("/items", (WorkItemService workItemService, string? status) =>
{
    // If status is not sent, use active as the status.
    status ??= "active";
    Enum.TryParse<ArchiveState>(status, true, out var archiveState);
    var result = workItemService.GetItemsByArchiveState(archiveState);

    return result;
});

app.MapGet("/items/{item_id}", (WorkItemService workItemService, string item_id) =>
{
    var result = workItemService.GetItem(item_id);

    return result;
});

app.MapPost("/items", (WorkItemService workItemService, WorkItem workItem) =>
{
    var result = workItemService.CreateItem(workItem);

    return result;
});

//app.MapPut("/items/{item_id}", (WorkItemService workItemService, WorkItem workItem, string item_id) =>
//{
//    var result = workItemService.TestRequest();

//    return result;
//});

app.MapPut("/items/{item_id}:archive", (WorkItemService workItemService, string item_id) =>
{
    var result = workItemService.ArchiveItem(item_id);

    return result;
});

app.MapPost("/items:report", (WorkItemService workItemService, string email) =>
{
    var result = workItemService.TestRequest();

    return result;
});

app.Run();