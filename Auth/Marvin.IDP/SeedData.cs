using Marvin.IDP.Areas.Identity.Data;
using Marvin.IDP.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Marvin.IDP
{
    public class SeedData
    {
        public static void EnsureSeedData(WebApplication app)
        {
            using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetService<MarvinIDPContext>();
                context.Database.Migrate();

                var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var emma = userMgr.FindByNameAsync("Emma").Result;
                if (emma == null)
                {
                    emma = new ApplicationUser
                    {
                        UserName = "Emma",
                        Email = "emma@example.com",
                        EmailConfirmed = true
                    };

                    var result = userMgr.CreateAsync(emma, "P@ssw0rd").Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    result = userMgr.AddClaimsAsync(emma, new[]
                    {
                        new System.Security.Claims.Claim("given_name", "Emma"),
                        new System.Security.Claims.Claim("family_name", "Smith"),
                        new System.Security.Claims.Claim("address", "One Hacker Way, Silicon Valley, CA"),
                        new System.Security.Claims.Claim("role", "PayingUser"),
                        new System.Security.Claims.Claim("subscriptionlevel", "FreeUser"),
                        new System.Security.Claims.Claim("country", "us")
                    }).Result;

                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    Log.Debug("Emma created");
                }
                else
                {
                    Log.Debug("Emma already exists");
                }

            }
        }
    }
}