namespace CorporationSyncify.Identity.WebApi.Authentication
{
    public class AuthenticationOptions
    {
        public bool VerifyTokenAudience { get; set; }

        public bool ValidateIssuer { get; set; }

        public bool IsSslRequired { get; set; }

        public string? Authority { get; set; }

        public string? ApiName { get; set; }

        public IEnumerable<string>? ValidIssuers { get; set; }
    }
}
