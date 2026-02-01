using Duende.IdentityServer;
using Marvin.IDP.DbContexts;
using Marvin.IDP.Entities;
using Marvin.IDP.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Marvin.IDP;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<IISOptions>(iis =>
        {
            iis.AuthenticationDisplayName = "Windows";
            iis.AutomaticAuthentication = false;
        });

        builder.Services.Configure<IISServerOptions>(iis =>
        {
            iis.AuthenticationDisplayName = "Windows";
            iis.AutomaticAuthentication = false;
        });

        // uncomment if you want to add a UI
        builder.Services.AddRazorPages();

        builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

        builder.Services.AddDbContext<IdentityDbContext>(opt =>
        {
            opt.UseSqlite(
                builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        builder.Services.AddIdentityServer(options =>
            {
                // https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/api_scopes#authorization-based-on-scopes
                options.EmitStaticAudienceClaim = true;
            })
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryApiResources(Config.ApiResources)
            .AddInMemoryClients(Config.Clients)
            .AddProfileService<LocalUserProfileService>();
            //.AddTestUsers(TestUsers.Users);

            builder.Services
                .AddAuthentication()
                .AddOpenIdConnect("AAD", "Azure Active Directory", opt =>
                {
                    opt.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    opt.ClientId = "8aba8fb0-e22c-48a5-b816-0aa8365f7b44";
                    opt.ClientSecret = "8el8Q~Kawa4Cl60ISxWEEP52vlUn59kfJpJZ7b3-";
                    opt.Authority = "https://login.microsoftonline.com/74cb19b5-dca1-4491-9474-db9e65ab538d/v2.0";

                    opt.ResponseType = "code";
                    opt.CallbackPath = new PathString("/signin-add");
                    opt.SignedOutCallbackPath = new PathString("/signout-add");
                    opt.Scope.Add("email");
                    opt.Scope.Add("offline_access");
                    opt.SaveTokens = true;
                });

        builder.Services.AddScoped<ILocalUserService, LocalUserService>();

        return builder.Build();
    }
    
    public static WebApplication ConfigurePipeline(this WebApplication app)
    { 
        app.UseSerilogRequestLogging();
    
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        // uncomment if you want to add a UI
        app.UseStaticFiles();
        app.UseRouting();

        app.UseAuthentication();

        app.UseIdentityServer();

        // uncomment if you want to add a UI
        app.UseAuthorization();
        app.MapRazorPages().RequireAuthorization();

        return app;
    }
}
