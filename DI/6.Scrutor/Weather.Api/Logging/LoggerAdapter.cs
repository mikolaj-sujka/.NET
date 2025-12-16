namespace Weather.Api.Logging;

public class LoggerAdapter<TType> : ILoggerAdapter<TType>
{
    private readonly ILogger<LoggerAdapter<TType>> _logger;

    public LoggerAdapter(ILogger<LoggerAdapter<TType>> logger)
    {
        _logger = logger;
    }

    public void Log(LogLevel logLevel, string template, params object[] args)
    {
        _logger.Log(logLevel, template, args);
    }

    public void LogInformation(string template, params object[] args)
    {
        Log(LogLevel.Information, template, args);
    }
}
