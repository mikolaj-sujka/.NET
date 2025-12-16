namespace ResolvingDeps.WebApi.HostedServices;

public class BackgroundTicker : BackgroundService
{
    private readonly ILogger<BackgroundTicker> _logger;

    public BackgroundTicker(ILogger<BackgroundTicker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation($"Hi from {nameof(BackgroundTicker)}");
            await Task.Delay(1000, stoppingToken);
        }
    }
}
