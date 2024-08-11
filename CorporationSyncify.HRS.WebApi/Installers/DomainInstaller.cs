using CorporationSyncify.HRS.Domain.Entities.Users;
using CorporationSyncify.HRS.Domain.ExternalEvents;

namespace CorporationSyncify.HRS.WebApi.Installers
{
    public static class DomainInstaller
    {
        public static IServiceCollection AddDomain(
            this IServiceCollection services)
        {
            services.AddSingleton<IExternalEventFactory, ExternalEventFactory>();

            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
