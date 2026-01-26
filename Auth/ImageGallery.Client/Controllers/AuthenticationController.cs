using Duende.IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace ImageGallery.Client.Controllers
{
    public class AuthenticationController(IHttpClientFactory httpClientFactory) : Controller
    {

        [Authorize]
        public async Task Logout()
        {
            var client = httpClientFactory.CreateClient("IDPClient");

            var discoveryDocumentResponse = await client.GetDiscoveryDocumentAsync();

            if (discoveryDocumentResponse.IsError)
            {
                throw new Exception(discoveryDocumentResponse.Error);
            }

            var accessTokenRevocationResponse = await client
                .RevokeTokenAsync(new()
                {
                    Address = discoveryDocumentResponse.RevocationEndpoint,
                    ClientId = "imagegalleryclient",
                    ClientSecret = "secret",
                    Token = await HttpContext.GetTokenAsync(
                        OpenIdConnectParameterNames.AccessToken),
                });

            if (accessTokenRevocationResponse.IsError)
            {
                throw new Exception(accessTokenRevocationResponse.Error);
            }

            var idTokenRevocationResponse = await client
                .RevokeTokenAsync(new()
                {
                    Address = discoveryDocumentResponse.RevocationEndpoint,
                    ClientId = "imagegalleryclient",
                    ClientSecret = "secret",
                    Token = await HttpContext.GetTokenAsync(
                        OpenIdConnectParameterNames.RefreshToken),
                });

            // Clears the local cookie
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirects to the identity provider's logout endpoint
            // "OpenIdConnect" is the default scheme name for OpenID Connect
            // so we can use the constant OpenIdConnectDefaults.AuthenticationScheme
            // to clear the OIDC cookie and trigger the logout at the IDP

            // Redirect back to the client after the IDP logout completes.
            /*return SignOut(
                new AuthenticationProperties { RedirectUri = Url.Action("Index", "Gallery") },
                CookieAuthenticationDefaults.AuthenticationScheme,
                OpenIdConnectDefaults.AuthenticationScheme);*/

            await HttpContext.SignOutAsync(
                OpenIdConnectDefaults.AuthenticationScheme);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
