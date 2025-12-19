using MultiFunction.ConsoleApp.Console;
using MultiFunction.ConsoleApp.Time;

namespace MultiFunction.ConsoleApp.Handlers
{
    [CommandName("time")]
    public class GetCurrentTimeHandler : IHandler
    {
        private readonly IConsoleWriter _consoleWriter;
        private readonly IDateTimeProvider _dateTimeProvider;

        public GetCurrentTimeHandler(IConsoleWriter consoleWriter, IDateTimeProvider dateTimeProvider)
        {
            _consoleWriter = consoleWriter;
            _dateTimeProvider = dateTimeProvider;
        }

        public Task HandleAsync()
        {
            var currentTime = _dateTimeProvider.DateTimeNow;
            _consoleWriter.WriteLine($"The current time is: {currentTime:O}");
            return Task.CompletedTask;
        }
    }
}
