namespace CorporationSyncify.Identity.WebApi.Configurations
{
    public class IdentityServerOptions
    {
        public IEnumerable<IdentityServerScope>? IdentityServerScopes { get; set; }

        public IEnumerable<IdentityServerResource>? IdentityServerResources { get; set; }

        public IEnumerable<IdentityServerClient>? IdentityServerClients { get; set; }

        public IdentityServerAdmin? IdentityServerAdmin { get; set; }
    }

    public class IdentityServerScope
    {
        public string? Name { get; set; }
    }

    public class IdentityServerResource
    {
        public string? Name { get; set; }

        public string[]? Scopes { get; set; }

        public string[]? ApiSecrets { get; set; }

        public string[]? UserClaims { get; set; }
    }

    public class IdentityServerClient
    {
        public string? ClientId { get; set; }

        public string? ClientName { get; set; }

        public string[]? AllowedGrantTypes { get; set; }

        public string[]? ClientSecrets { get; set; }

        public string[]? AllowedScopes { get; set; }

        public string[]? RedirectUris { get; set; }

        public string? FrontChannelLogoutUri { get; set; }

        public string[]? PostLogoutRedirectUris { get; set; }

        public bool AllowOfflineAccess { get; set; }

        public bool RequirePkce { get; set; }

        public bool RequireConsent { get; set; }

        public bool AllowPlainTextPkce { get; set; }
    }

    public class IdentityServerAdmin
    {
        public string? UserName { get; set; }

        public string? Password { get; set; }

        public string? Email { get; set; }
    }
}
