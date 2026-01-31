using Duende.IdentityModel;
using Duende.IdentityServer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Authorization;

namespace Marvin.IDP.Pages.Windows
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        public async Task<IActionResult> OnGet(string returnUrl)
        {
            // see if windows auth has already been requested and succeeded
            var result = await HttpContext.AuthenticateAsync("Windows");
            if (result?.Principal is WindowsPrincipal wp)
            {
                // beware the performance penalty for loading these group claims
                var wi = wp.Identity as WindowsIdentity;
                var groups = wi.Groups.Translate(typeof(NTAccount));
                var roles = groups.Select(x => new Claim(JwtClaimTypes.Role, x.Value));

                var displayName = wp.Identity?.Name ?? string.Empty; // e.g. DOMAIN\user
                var givenName = displayName.Contains('\\', StringComparison.Ordinal)
                    ? displayName[(displayName.IndexOf('\\') + 1)..]
                    : displayName;

                var user = new IdentityServerUser(wp.FindFirst(ClaimTypes.PrimarySid).Value)
                {
                    IdentityProvider = "Windows",
                    DisplayName = displayName,
                    AdditionalClaims = roles
                        .Append(new Claim(JwtClaimTypes.Name, displayName))
                        .Append(new Claim(JwtClaimTypes.GivenName, givenName))
                        .ToList(),
                };

                await HttpContext.SignInAsync(user);
                return LocalRedirect(returnUrl);
            }
            else
            {
                // trigger windows auth
                // since windows auth don't support the redirect uri,
                // this URL is re-triggered when we call challenge
                return Challenge("Windows");
            }
        }
    }
}
