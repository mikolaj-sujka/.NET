using Microsoft.AspNetCore.Mvc;
using Weather.Minimal.Api;
using Weather.Minimal.Api.Weather;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddSingleton<IdGenerator>();
builder.Services.AddTransient<IWeatherService, OpenWeatherService>();

var idGeneratorProvider = builder.Services.BuildServiceProvider().GetRequiredService<IdGenerator>();
Console.WriteLine($"IdGenerator ID: {idGeneratorProvider.Id}");

var app = builder.Build();

app.MapGet("weather/{city}",
    async ([FromRoute] string city, IWeatherService weatherService, IdGenerator idGenerator) =>
{
    Console.WriteLine($"IdGenerator ID: {idGenerator.Id}");
    var weather = await weatherService.GetCurrentWeatherAsync(city);
    return weather == null ? Results.NotFound() : Results.Ok(weather);
});

app.Run();
