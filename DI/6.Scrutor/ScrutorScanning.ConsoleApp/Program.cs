using Microsoft.Extensions.DependencyInjection;
using ScrutorScanning.ConsoleApp.Services;

var services = new ServiceCollection();

services.AddTransient<IExampleAService, ExampleAService>();

PrintRegisteredService(services);
var serviceProvider = services.BuildServiceProvider();

void PrintRegisteredService(IServiceCollection serviceCollection)
{
    foreach (var service in serviceCollection)
    {
        Console.WriteLine($"{service.ServiceType.Name} -> {service.ImplementationType?.Name} as {service.Lifetime.ToString()}");
    }
}
