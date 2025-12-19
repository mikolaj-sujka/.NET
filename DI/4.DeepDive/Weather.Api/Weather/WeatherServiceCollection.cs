using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Weather.Api.Weather
{
    public static class WeatherServiceCollection
    {
        public static IServiceCollection AddWeatherServices(this IServiceCollection services)
        {
            // InMemoryWeatherService was registered over OpenWeatherService to demonstrate DI; 
            // Then to get OpenWeatherService, change the order of registration or use IEnumerable<IWeatherService> in the constructor.
            // builder.Services.AddTransient<IWeatherService, OpenWeatherService>();
            // builder.Services.AddTransient<IWeatherService, InMemoryWeatherService>();

            var openWeatherServiceDescriptor =
                new ServiceDescriptor(typeof(IWeatherService), typeof(OpenWeatherService), ServiceLifetime.Transient);

            var inMemoryWeatherServiceDescriptor =
                new ServiceDescriptor(typeof(IWeatherService), typeof(InMemoryWeatherService), ServiceLifetime.Transient);

            services.TryAddEnumerable(openWeatherServiceDescriptor);
            services.TryAddEnumerable(inMemoryWeatherServiceDescriptor);

            return services;
        }
    }
}
