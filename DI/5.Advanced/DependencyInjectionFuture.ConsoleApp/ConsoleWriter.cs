namespace DependencyInjectionFuture.ConsoleApp;

public class ConsoleWriter : IConsoleWriter
{
    public void WriteLine(string text)
    {
        Console.WriteLine(text);
    }
}

public interface IConsoleWriter
{
    void WriteLine(string text);
}
