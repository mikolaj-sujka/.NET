using Microsoft.AspNetCore.Authorization;

namespace ImageGallery.Authorization
{
    public static class AuthorizationPolicies
    {
        public static AuthorizationPolicy CanAddImage()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim("country", "be") // you can add allowedValues as additional parameters
                .RequireRole("PayingUser")
                .Build();
        }
    }
}
