using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Marvin.IDP;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
    [
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
        new IdentityResource("roles",
            "Your role(s)",
            ["role"]),
        new IdentityResource("country",
            "The country you live in",
            ["country"])
    ];

    public static IEnumerable<ApiScope> ApiScopes =>
        [
            new ApiScope("imagegalleryapi.fullaccess"),
            new ApiScope("imagegalleryapi.write"),
            new ApiScope("imagegalleryapi.read")
        ];

    public static IEnumerable<ApiResource> ApiResources =>
    [
        new ApiResource("imagegalleryapi", "Image Gallery API",
            ["role", "country"])
        {
            Scopes = { "imagegalleryapi.fullaccess", 
                "imagegalleryapi.write", 
                "imagegalleryapi.read" }
        }
    ];

    public static IEnumerable<Client> Clients =>
    [
        new Client()
        {
            ClientName = "Image Gallery",
            ClientId = "imagegalleryclient",
            AllowedGrantTypes = GrantTypes.Code,
            //AuthorizationCodeLifetime = 
            //IdentityTokenLifetime = 
            AllowOfflineAccess = true,
            //RefreshTokenExpiration = 
            UpdateAccessTokenClaimsOnRefresh = true,
            AccessTokenLifetime = TimeSpan.FromMinutes(2).Minutes,
            RedirectUris =
            {
                "https://localhost:7184/signin-odc"
            },
            PostLogoutRedirectUris =
            {
                "https://localhost:7184/signout-callback-odc"
            },
            AllowedScopes =
            {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                "roles",
                //"imagegalleryapi.fullaccess",
                "country",
                "imagegalleryapi.read",
                "imagegalleryapi.write"
            },
            ClientSecrets =
            {
                new Secret("secret".Sha256())
            },
            RequireConsent = true
        }
    ];
}