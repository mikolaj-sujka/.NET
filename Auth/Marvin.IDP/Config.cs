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
            ["role"])
    ];

    public static IEnumerable<ApiScope> ApiScopes =>
        [
            new ApiScope("imagegalleryapi.fullaccess")
        ];

    public static IEnumerable<ApiResource> ApiResources =>
    [
        new ApiResource("imagegalleryapi", "Image Gallery API")
        {
            Scopes = { "imagegalleryapi.fullaccess" }
        }
    ];

    public static IEnumerable<Client> Clients =>
    [
        new Client()
        {
            ClientName = "Image Gallery",
            ClientId = "imagegalleryclient",
            AllowedGrantTypes = GrantTypes.Code,
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
                "imagegalleryapi.fullaccess"
            },
            ClientSecrets =
            {
                new Secret("secret".Sha256())
            },
            RequireConsent = true
        }
    ];
}