using Amazon.RDSDataService;
using AuroraItemTracker;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging.Debug;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAWSService<IAmazonRDSDataService>();
builder.Services.AddScoped<RDSDataClientWrapper>();

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

app.MapGet("/testrds", (RDSDataClientWrapper wrapper) =>
    {
        var result = wrapper.TestRequest();

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
app.MapGet("/items", (RDSDataClientWrapper wrapper, string? status) =>
{
    var result = wrapper.TestRequest();

    return result;
});

app.MapGet("/items/{item_id}", (RDSDataClientWrapper wrapper, int item_id) =>
{
    var result = wrapper.TestRequest();

    return result;
});

app.MapPost("/items", (RDSDataClientWrapper wrapper, WorkItem workItem) =>
{
    var result = wrapper.TestRequest();

    return result;
});

app.MapPut("/items/{item_id}", (RDSDataClientWrapper wrapper, WorkItem workItem, string item_id) =>
{
    var result = wrapper.TestRequest();

    return result;
});

app.MapPut("/items/{item_id}:archive", (RDSDataClientWrapper wrapper, string item_id) =>
{
    var result = wrapper.TestRequest();

    return result;
});

app.MapPost("/items:report", (RDSDataClientWrapper wrapper, string email) =>
{
    var result = wrapper.TestRequest();

    return result;
});

app.Run();