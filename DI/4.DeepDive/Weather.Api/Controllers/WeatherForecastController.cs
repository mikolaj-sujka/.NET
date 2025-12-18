using Microsoft.AspNetCore.Mvc;
using Weather.Api.Logging;
using Weather.Api.Mappers;
using Weather.Api.Weather;

namespace Weather.Api.Controllers;

[ApiController]
public class WeatherForecastController : ControllerBase
{
    private readonly IWeatherService _weatherService;
    private readonly ILoggerAdapter<WeatherForecastController> _logger;

    public WeatherForecastController(IWeatherService weatherService, ILoggerAdapter<WeatherForecastController> logger)
    {
        _weatherService = weatherService;
        _logger = logger;
    }

    [HttpGet("weather/{city}")]
    public async Task<IActionResult> GetCurrentWeather([FromRoute] string city)
    {
        var weather = await _weatherService.GetCurrentWeatherAsync(city);
        if (weather == null)
        {
            _logger.LogInformation("Weather data not found for city: {City}", city);
            return NotFound();
        }

        var weatherResponse = weather.MapToWeatherResponse();

        _logger.LogInformation("Weather data found for city: {City}", city);
        return Ok(weatherResponse);
    }
}
