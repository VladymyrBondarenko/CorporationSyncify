using CorporationSyncify.Identity.WebApi.BackgroundJobs;

namespace CorporationSyncify.Identity.WebApi.Installers
{
    public static class BackgroundJobInstaller
    {
        public static void AddBackgroundJobs(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
            });
            services.AddHostedService<ProcessOutboxMessagesJob>();

            var backgroundJobsOptions = configuration
                .GetSection(nameof(BackgroundJobOptions))
                .Get<BackgroundJobOptions>()!;
            services.AddSingleton(backgroundJobsOptions);
        }
    }
}
