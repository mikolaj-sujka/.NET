using Microsoft.Extensions.Logging;

namespace BasicConsoleApp;

// Define the source-generated logging extension methods
// for better performance and reduced allocations
public static partial class LoggerExtensions
{
    [LoggerMessage(Level = LogLevel.Information,
        EventId = 5001,
        Message = "Customer {Email} purchased product {ProductId} at {Amount}")]
    public static partial void LogPaymentCreation(
        this ILogger logger, string email, decimal amount, int productId);
}
