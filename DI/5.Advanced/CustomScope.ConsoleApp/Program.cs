using CustomScope.ConsoleApp;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddScoped<ExampleService>();

var serviceProvider = services.BuildServiceProvider();

var serviceScopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();

using (var scopeServiceProvider1 = serviceScopeFactory.CreateScope())
{
    var exampleService1 = scopeServiceProvider1.ServiceProvider.GetRequiredService<ExampleService>();

    Console.WriteLine(exampleService1.Id);
}


using (var scopeServiceProvider2 = serviceScopeFactory.CreateScope())
{
    var exampleService2 = scopeServiceProvider2.ServiceProvider.GetRequiredService<ExampleService>();
    Console.WriteLine(exampleService2.Id);
}


