using CorporationSyncify.Identity.WebApi.BackgroundJobs;
using Hangfire;

namespace CorporationSyncify.Identity.WebApi.Installers
{
    public static class BackgroundJobsInstaller
    {
        public static void AddBackgroundJobs(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
            });

            services.AddHangfire(config => 
                config.UseSqlServerStorage(configuration.GetConnectionString("IdentityDbConnection")));
            services.AddHangfireServer(options => 
                options.SchedulePollingInterval = TimeSpan.FromSeconds(1));

            services.AddScoped<IProcessOutboxMessagesJob, ProcessOutboxMessagesJob>();

            var backgroundJobsOptions = configuration
                .GetSection(nameof(BackgroundJobsOptions))
                .Get<BackgroundJobsOptions>()!;
            services.AddSingleton(backgroundJobsOptions);
        }
    }
}
