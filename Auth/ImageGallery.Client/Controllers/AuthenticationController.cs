using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImageGallery.Client.Controllers
{
    public class AuthenticationController : Controller
    {
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            // Clears the local cookie
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirects to the identity provider's logout endpoint
            // "OpenIdConnect" is the default scheme name for OpenID Connect
            // so we can use the constant OpenIdConnectDefaults.AuthenticationScheme
            // to clear the OIDC cookie and trigger the logout at the IDP

            // Redirect back to the client after the IDP logout completes.
            return SignOut(
                new AuthenticationProperties { RedirectUri = Url.Action("Index", "Gallery") },
                CookieAuthenticationDefaults.AuthenticationScheme,
                OpenIdConnectDefaults.AuthenticationScheme);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
