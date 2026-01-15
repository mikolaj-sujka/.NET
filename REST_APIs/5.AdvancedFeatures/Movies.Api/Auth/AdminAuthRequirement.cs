using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Movies.Api.Auth
{
    public class AdminAuthRequirement : IAuthorizationRequirement, IAuthorizationHandler
    {
        private readonly string _apiKey;

        public AdminAuthRequirement(string apiKey)
        {
            _apiKey = apiKey;
        }

        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            if (context.User.HasClaim(c => c is { Type: AuthConstants.AdminUserClaimName, Value: "true" }))
            {
                context.Succeed(this);
                return Task.CompletedTask;
            }

            if (context.Resource is HttpContext httpContext 
                && httpContext.Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName, out var extractedApiKey))
            {
                if (string.Equals(extractedApiKey, _apiKey, StringComparison.Ordinal))
                {
                    context.Succeed(this);
                }

                context.Fail();
                return Task.CompletedTask;
            }

            var identity = (ClaimsIdentity)context.User.Identity!;
            identity.AddClaim(new Claim("userid", Guid.NewGuid().ToString()));
            context.Succeed(this);
            return Task.CompletedTask;
        }
    }
}
