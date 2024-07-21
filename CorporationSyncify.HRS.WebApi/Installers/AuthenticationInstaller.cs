using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace CorporationSyncify.HRS.WebApi.Authentification
{
    public static class AuthenticationInstaller
    {
        public static void AddIdentityServerAuthentication(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            var authenticationOptions = configuration
                .GetSection(nameof(AuthenticationOptions))
                .Get<AuthenticationOptions>()!;

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(jwtOptions =>
                {
                    jwtOptions.Authority = authenticationOptions.Authority;
                    jwtOptions.Audience = authenticationOptions.ApiName;
                    jwtOptions.RequireHttpsMetadata = authenticationOptions.IsSslRequired;
                    jwtOptions.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
                    jwtOptions.TokenValidationParameters.ValidIssuers = authenticationOptions.ValidIssuers;

                    jwtOptions.SaveToken = true;
                });
        }
    }
}
