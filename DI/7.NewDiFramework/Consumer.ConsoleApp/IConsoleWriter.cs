namespace Consumer.ConsoleApp
{
    public interface IConsoleWriter
    {
        void WriteLine(string message);
    }
    

    public class ConsoleWriter : IConsoleWriter
    {
        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }
    }
}
