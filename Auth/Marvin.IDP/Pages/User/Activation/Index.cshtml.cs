using Marvin.IDP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Marvin.IDP.Pages.User.Activation
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class IndexModel(ILocalUserService localUserService) : PageModel
    {
        [BindProperty]
        public InputModel Input { get; set; }

        public async Task<IActionResult> OnGet(string securityCode)
        {
            if (await localUserService.ActivateUserAsync(securityCode))
            {
                Input.Message = "Your account has been successfully activated. You can now log in.";
            }
            else
            {
                Input.Message = "Activation failed. The security code may be invalid or expired.";
            }

            await localUserService.SaveChangesAsync();

            return Page();
        }
    }
}
