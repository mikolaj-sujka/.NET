using Microsoft.AspNetCore.Mvc.Filters;

namespace Movies.Api.Auth
{
    public class ApiKeyAuthFilter : IAuthorizationFilter
    {
        private readonly IConfiguration _configuration;

        public ApiKeyAuthFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName, out var extractedApiKey))
            {
                context.HttpContext.Response.StatusCode = 401;
                context.Result = new Microsoft.AspNetCore.Mvc.JsonResult(new { message = "API Key was not provided." });
                return;
            }

            var apiKey = _configuration.GetValue<string>("ApiKey");

            if (apiKey.Equals(extractedApiKey)) return;

            context.HttpContext.Response.StatusCode = 401;
            context.Result = new Microsoft.AspNetCore.Mvc.JsonResult(new { message = "Unauthorized client." });
        }
    }
}
