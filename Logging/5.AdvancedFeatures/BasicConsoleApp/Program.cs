using System.Text.Json;
using BasicConsoleApp;
using Microsoft.Extensions.Logging;

using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddJsonConsole(x =>
    {
        x.IncludeScopes = true;
        x.JsonWriterOptions = new JsonWriterOptions
        {
            Indented = true
        };
    });
    //builder.SetMinimumLevel(LogLevel.Warning);
});

var paymentData = new PaymentData
{
    PaymentId = 1,
    Total = 15.99m,
};

ILogger logger = loggerFactory.CreateLogger<Program>();

var paymentId = 1;
var amount = 15.99;

//while (true)
//{
    /*logger.LogInformation(
        "New Payment with id {PaymentId} for ${Total}", paymentId, amount);
    await Task.Delay(1000);

    logger.LogInformation("New Payment with data {Payment}", paymentData);
//}


    using (logger.BeginScope("{PaymentId}", paymentId))
    {
        try
        {
            logger.LogInformation("New payment for ${Total}", amount);

            // processing
        }
        finally
        {
            logger.LogInformation("Processing completed.");
        }
    }*/

    using (logger.BeginTimedOperation("Handling new payment"))
    {
        if (logger.IsEnabled(LogLevel.Information))
        {
            logger.LogInformation("New payment for ${Total}", amount);
        }

        await Task.Delay(10);
    }


class PaymentData
{ // or record struct PaymentData
    public int PaymentId { get; set; }
    public decimal Total { get; set; }

    public override string ToString()
    {
        return $"PaymentId: {PaymentId}, Total: {Total}";
    }
}