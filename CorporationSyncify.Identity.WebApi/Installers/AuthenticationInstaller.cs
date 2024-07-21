using CorporationSyncify.Identity.WebApi.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace CorporationSyncify.Identity.WebApi.Installers
{
    public static class AuthenticationInstaller
    {
        public static void AddIdentityServerAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var authOptions = configuration
                .GetSection(nameof(AuthenticationOptions))
                .Get<AuthenticationOptions>()!;

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(jwtOptions =>
                {
                    jwtOptions.Authority = authOptions.Authority;
                    jwtOptions.Audience = authOptions.ApiName;
                    jwtOptions.RequireHttpsMetadata = authOptions.IsSslRequired;
                    jwtOptions.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
                    jwtOptions.TokenValidationParameters.ValidIssuers = authOptions.ValidIssuers;

                    jwtOptions.SaveToken = true;
                });
        }
    }
}
