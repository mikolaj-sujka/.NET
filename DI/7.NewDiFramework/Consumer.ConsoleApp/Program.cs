using Consumer.ConsoleApp;
using Vax;

var services = new ServiceCollection();

services.AddSingleton<IConsoleWriter, ConsoleWriter>();

var serviceProvider = services.BuildServiceProvider();

var service = serviceProvider.GetService<IConsoleWriter>();

service?.WriteLine("Hello Dependency Injection World!");