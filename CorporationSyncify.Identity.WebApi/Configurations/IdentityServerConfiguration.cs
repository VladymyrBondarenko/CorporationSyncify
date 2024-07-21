using IdentityServer4.Models;

namespace CorporationSyncify.Identity.WebApi.Configurations
{
    public class IdentityServerConfiguration
    {
        public IdentityServerConfiguration(IConfiguration configuration)
        {
            var identityServerOptions = configuration
                .GetSection(nameof(IdentityServerOptions))
                .Get<IdentityServerOptions>()!;

            if (identityServerOptions.IdentityServerScopes != null)
            {
                _apiScope = identityServerOptions.IdentityServerScopes
                    .Select(x => new ApiScope(x.Name));
            }

            if (identityServerOptions.IdentityServerResources != null)
            {
                _apiResources = identityServerOptions.IdentityServerResources
                    .Select(x => new ApiResource(x.Name)
                    {
                        Scopes = x.Scopes,
                        ApiSecrets = x.ApiSecrets?.Select(value => new Secret(value.Sha256())).ToArray(),
                        UserClaims = x.UserClaims
                    });
            }

            if (identityServerOptions.IdentityServerClients != null)
            {
                _clients = identityServerOptions.IdentityServerClients
                    .Select(x => new Client
                    {
                        ClientId = x.ClientId,
                        ClientName = x.ClientName,
                        AllowedGrantTypes = x.AllowedGrantTypes,
                        ClientSecrets = x.ClientSecrets?.Select(value => new Secret(value.Sha256())).ToArray(),
                        AllowedScopes = x.AllowedScopes,
                        RedirectUris = x.RedirectUris,
                        FrontChannelLogoutUri = x.FrontChannelLogoutUri,
                        PostLogoutRedirectUris = x.PostLogoutRedirectUris,
                        AllowOfflineAccess = x.AllowOfflineAccess,
                        RequirePkce = x.RequirePkce,
                        RequireConsent = x.RequireConsent,
                        AllowPlainTextPkce = x.AllowPlainTextPkce
                    });
            }
        }

        public IReadOnlyList<IdentityResource> IdentityResources => new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource
            {
                Name = "roles",
                UserClaims = new List<string> { "role" }
            }
        };

        private IEnumerable<ApiScope> _apiScope = Array.Empty<ApiScope>();

        public IReadOnlyCollection<ApiScope> ApiScopes => _apiScope.ToArray();

        private IEnumerable<ApiResource> _apiResources = Array.Empty<ApiResource>();

        public IReadOnlyCollection<ApiResource> ApiResources => _apiResources.ToArray();

        private IEnumerable<Client> _clients = Array.Empty<Client>();

        public IEnumerable<Client> Clients => _clients.ToArray();
    }
}
