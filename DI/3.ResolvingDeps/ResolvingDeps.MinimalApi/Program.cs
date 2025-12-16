using ResolvingDeps.MinimalApi;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("weather", (ILogger<Program> logger) =>
{
    var weatherSummaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    var weather = Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = weatherSummaries[Random.Shared.Next(weatherSummaries.Length)]
        }).ToArray();
    logger.LogInformation("Hi from logger.");
    return Results.Ok(weather);
});

app.Run();
