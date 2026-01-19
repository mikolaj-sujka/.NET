using Duende.IdentityServer.Models;

namespace Marvin.IDP;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
    [
        new IdentityResources.OpenId(),
            new IdentityResources.Profile()
    ];

    public static IEnumerable<ApiScope> ApiScopes =>
        [];

    public static IEnumerable<Client> Clients =>
        [];
}