namespace CorporationSyncify.HRS.WebApi.Authentification
{
    public class AuthenticationOptions
    {
        public TimeSpan TokenClockSkew { get; set; }

        public bool VerifyTokenAudience { get; set; }

        public bool ValidateIssuer { get; set; }

        public bool IsSslRequired { get; set; }

        public string? Authority { get; set; }

        public string? ApiName { get; set; }

        public string? ClientId { get; set; }

        public string? ClientSecret { get; set; }

        public IEnumerable<string>? ValidIssuers { get; set; }
    }
}
