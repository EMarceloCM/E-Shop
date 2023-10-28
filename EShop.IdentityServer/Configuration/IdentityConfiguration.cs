using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace EShop.IdentityServer.Configuration
{
    public class IdentityConfiguration
    {
        public const string Admin = "Admin";
        public const string Client = "Client";

        public static IEnumerable<IdentityResource> IdentityResources => new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Email(),
            new IdentityResources.Profile()
        };

        public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope>
        {
            //eshop acessa identityserver para obter o token
            new ApiScope("eshop", "EShop Server"),
            new ApiScope(name: "read", "Read data."),
            new ApiScope(name: "write", "Write data."),
            new ApiScope(name: "delete", "Delete data.")
        };

        public static IEnumerable<Client> Clients => new List<Client>
        {
            //cliente genérico
            new Client
            {
                ClientId = "client",
                ClientSecrets = {new Secret("abracadabra#simsalabim".Sha256())},
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = {"read", "write", "profile"},
                RedirectUris = { "https://localhost:7297/signin-oidc", "http://localhost:5242/signin-oidc" }
            },
            new Client
            {
                ClientId = "eshop",
                ClientSecrets = {new Secret("abracadabra#simsalabim".Sha256())},
                AllowedGrantTypes = GrantTypes.Code, //via código
                RedirectUris = { "https://localhost:7297/signin-oidc", "http://localhost:5242/signin-oidc"}, //login
                PostLogoutRedirectUris = {"https://localhost:7297/signout-callback-oidc"},
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    "eshop"
                }
            }
        };
    }
}