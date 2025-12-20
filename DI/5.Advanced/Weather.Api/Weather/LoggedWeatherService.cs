using System.Diagnostics;

namespace Weather.Api.Weather
{
    public class LoggedWeatherService : IWeatherService
    {
        private readonly IWeatherService _weatherService;
        private readonly ILogger<IWeatherService> _logger;

        public LoggedWeatherService(IWeatherService weatherService, 
            ILogger<IWeatherService> logger)
        {
            _weatherService = weatherService;
            _logger = logger;
        }

        public async Task<WeatherResponse?> GetCurrentWeatherAsync(string city)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                return await _weatherService.GetCurrentWeatherAsync(city);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                sw.Stop();
                _logger.LogInformation("GetCurrentWeatherAsync for city {City} took {ElapsedMilliseconds} ms",
                    city, sw.ElapsedMilliseconds);
            }
        }
    }
}
