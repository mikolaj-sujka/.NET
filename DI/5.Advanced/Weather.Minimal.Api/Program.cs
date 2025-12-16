using Microsoft.AspNetCore.Mvc;
using Weather.Minimal.Api.Weather;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddTransient<IWeatherService, OpenWeatherService>();

var app = builder.Build();

app.MapGet("weather/{city}",
    async ([FromRoute] string city, IWeatherService weatherService) =>
{
    var weather = await weatherService.GetCurrentWeatherAsync(city);
    return weather == null ? Results.NotFound() : Results.Ok(weather);
});

app.Run();
