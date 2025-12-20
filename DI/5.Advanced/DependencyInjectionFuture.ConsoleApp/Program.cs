using DependencyInjectionFuture.ConsoleApp;

Console.WriteLine();

var serviceProvider = new MyServiceProvider();

var consoleWriter = serviceProvider.GetService<IConsoleWriter>();

consoleWriter.WriteLine("Hello from Dependency Injection Future!");