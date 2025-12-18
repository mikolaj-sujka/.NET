using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ResolvingDeps.WebApi.Attributes;

public class DurationLoggerAttribute : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            await next();
        }
        finally
        {
            var serviceProvider = context.HttpContext.RequestServices;

            // Resolve the logger from the service provider
            // Because attribute classes are instantiated by the framework and requires const parameterless constructor
            var logger = serviceProvider.GetRequiredService<ILogger<DurationLoggerAttribute>>();
            var text = $"Request completed in {sw.ElapsedMilliseconds}ms";
            logger.LogInformation(text);
            //Console.WriteLine(text);
        }
    }
}
