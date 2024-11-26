using Duende.IdentityServer.Models;
using Duende.IdentityServer;

namespace IdentityServer
{
    public static class Configuration
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("api")
                {
                    Scopes =
                    {
                        "api.read",
                        "api.write"
                    }
                }
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client> {
                new Client
                {
                    ClientId = "react_client_app",
                    AllowedGrantTypes = GrantTypes.Code,
                    AccessTokenLifetime = 3600, // Время жизни access token в секундах (1 час)
                    AllowOfflineAccess = true,  // Разрешает использование refresh tokens
                    RefreshTokenUsage = TokenUsage.OneTimeOnly, // Refresh токен можно использовать только один раз
                    AbsoluteRefreshTokenLifetime = 2592000, // Время жизни refresh токена (30 дней)
                    ClientSecrets = { new Secret("MegaSuperSecureSecret".Sha256()) },
                    RedirectUris = { "https://localhost:3000/callback" },
                    PostLogoutRedirectUris = { "https://localhost:3000/" },
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api.read",
                        "api.write"
                    }
                }

            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile()
            };
        }
    }
}
