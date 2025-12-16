using MultiFunction.ConsoleApp.Console;

namespace MultiFunction.ConsoleApp;

public class Application
{
    private readonly IConsoleWriter _consoleWriter;

    public Application(IConsoleWriter consoleWriter)
    {
        _consoleWriter = consoleWriter;
    }

    public async Task RunAsync(string[] args)
    {
        // var command = args[0];
        //
        // var handler = _handlerOrchestrator.GetHandlerForCommandName(command);
        // if (handler == null)
        // {
        //     _consoleWriter.WriteLine($"No handler found for command name {command}");
        //     return;
        // }
        // await handler.HandleAsync();
    }
}
