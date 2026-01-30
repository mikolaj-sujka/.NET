using Duende.IdentityModel;
using Duende.IdentityServer;
using Duende.IdentityServer.Services;
using Marvin.IDP.Entities;
using Marvin.IDP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Marvin.IDP.Pages.User.Registration
{
    [AllowAnonymous]
    [SecurityHeaders]
    public class IndexModel(ILocalUserService localUserService, 
        IIdentityServerInteractionService identityServerInteractionService) : PageModel
    {
        [BindProperty]
        public InputModel Input { get; set; } 
        public IActionResult OnGet(string returnUrl)
        {
            BuildModel(returnUrl);

            return Page();
        }

        private void BuildModel(string returnUrl)
        {
            Input = new InputModel
            {
                ReturnUrl = returnUrl
            };
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                BuildModel(Input.ReturnUrl);
                return Page();
            }
            var user = new Entities.User()
            {
                UserName = Input.Username,
                Active = true,
                Subject = Guid.NewGuid().ToString()
            };

            user.Claims.Add(new UserClaim()
            {
                Type = "country",
                Value = Input.Country
            });

            user.Claims.Add(new UserClaim()
            {
                Type = JwtClaimTypes.GivenName,
                Value = Input.GivenName
            });

            user.Claims.Add(new UserClaim()
            {
                Type = JwtClaimTypes.FamilyName,
                Value = Input.FamilyName
            });

            
            localUserService.AddUser(user, Input.Password);
            await localUserService.SaveChangesAsync();

            // Issue authentication cookie
            var isUser = new IdentityServerUser(user.Subject)
            {
                DisplayName = user.UserName
            };
            await HttpContext.SignInAsync(isUser);

            // continue with the flow
            if (identityServerInteractionService.IsValidReturnUrl(Input.ReturnUrl) ||
                Url.IsLocalUrl(Input.ReturnUrl))
            {
                return Redirect(Input.ReturnUrl);
            }

            return Redirect("~/");
        }
    }
}
