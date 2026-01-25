using ImageGallery.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddJsonOptions(configure => 
        configure.JsonSerializerOptions.PropertyNamingPolicy = null);


var apiRoot = builder.Configuration["ImageGalleryAPIRoot"];

JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();

// enables AddUserAccessTokenHandler() for outgoing HttpClient calls
builder.Services.AddAccessTokenManagement();

// create an HttpClient used for accessing the API
builder.Services.AddHttpClient("APIClient", client =>
{
    client.BaseAddress = apiRoot == null ? null : new Uri(apiRoot);
    client.DefaultRequestHeaders.Clear();
    client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
}).AddUserAccessTokenHandler();

builder.Services.AddAuthentication(opt =>
    {
        opt.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opt =>
    {
        opt.AccessDeniedPath = "/Authentication/AccessDenied";
    })
    .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, opt =>
    {
        opt.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        opt.Authority = "https://localhost:5001/";
        opt.ClientId = "imagegalleryclient";
        opt.ClientSecret = "secret";
        opt.ResponseType = "code";

        //opt.Scope.Add("profile");
        opt.CallbackPath = "/signin-odc";
        opt.SignedOutCallbackPath = "/signout-callback-odc";
        opt.SaveTokens = true;
        opt.GetClaimsFromUserInfoEndpoint = true;

        opt.ClaimActions.Remove("aud"); // remove audience claim
        opt.ClaimActions.DeleteClaim("sid"); // remove sid claim, extension method
        opt.ClaimActions.DeleteClaim("idp"); // remove idp claim, extension method

        //opt.Scope.Add("imagegalleryapi.fullaccess");
        opt.Scope.Add("country");

        opt.Scope.Add("imagegalleryapi.read");
        opt.Scope.Add("imagegalleryapi.write");

        opt.Scope.Add("roles");
        opt.ClaimActions.MapJsonKey("role", "role");
        opt.ClaimActions.MapUniqueJsonKey("country", "country");

        opt.TokenValidationParameters = new()
        {
            NameClaimType = "given_name",
            RoleClaimType = "role"
        };
    });

builder.Services.AddAuthorization(authOptions =>
{
    authOptions.AddPolicy("UserCanAddImage", 
        AuthorizationPolicies.CanAddImage());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Gallery}/{action=Index}/{id?}");

app.Run();
