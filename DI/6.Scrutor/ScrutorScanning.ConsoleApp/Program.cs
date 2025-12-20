using Microsoft.Extensions.DependencyInjection;
using ScrutorScanning.ConsoleApp.Attributes;

var services = new ServiceCollection();

// services.AddTransient<IExampleAService, ExampleAService>();

services.Scan(selector =>
{
    /*selector
        .FromAssemblyOf<Program>()
            .AddClasses(filter => filter.AssignableTo<ISingletonService>())
                .AsMatchingInterface()
                .WithSingletonLifetime()

            .AddClasses(filter => filter.AssignableTo<ITransientService>())
                .AsMatchingInterface()
                .WithTransientLifetime()

            .AddClasses(filter => filter.AssignableTo<IScopedService>())
                .AsMatchingInterface()
                .WithScopedLifetime();*/

    selector
        .FromAssemblyOf<Program>()
            .AddClasses(filter => filter.WithAttribute<SingletonAttribute>())
                .AsMatchingInterface()
                .WithSingletonLifetime()

            .AddClasses(filter => filter.WithAttribute<TransientAttribute>())
                .AsMatchingInterface()
                .WithTransientLifetime()

            .AddClasses(filter => filter.WithAttribute<ScopedAttribute>())
                .AsMatchingInterface()
                .WithScopedLifetime();
});

PrintRegisteredService(services);
var serviceProvider = services.BuildServiceProvider();

void PrintRegisteredService(IServiceCollection serviceCollection)
{
    foreach (var service in serviceCollection)
    {
        Console.WriteLine($"{service.ServiceType.Name} -> {service.ImplementationType?.Name} as {service.Lifetime.ToString()}");
    }
}
