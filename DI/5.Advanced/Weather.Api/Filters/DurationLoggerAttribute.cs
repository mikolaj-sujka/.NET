using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Weather.Api.Filters;

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
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<DurationLoggerAttribute>>();
            logger.LogInformation("Request with name {0} completed in {1}ms",
                context.ActionDescriptor.DisplayName,
                sw.ElapsedMilliseconds);
        }
    }
}
