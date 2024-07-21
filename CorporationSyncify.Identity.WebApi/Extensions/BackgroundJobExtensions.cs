using CorporationSyncify.Identity.WebApi.BackgroundJobs;
using Hangfire;

namespace CorporationSyncify.Identity.WebApi.Extensions
{
    public static class BackgroundJobExtensions
    {
        public static IApplicationBuilder UseBackgroundJob(this WebApplication app)
        {
            app.Services
                .GetRequiredService<IRecurringJobManager>()
                .AddOrUpdate<IProcessOutboxMessagesJob>(
                    "outbox-processor",
                    job => job.ProcessAsync(),
                    app.Configuration["BackgroundJobsOptions:Outbox:Schedule"]);

            return app;
        }
    }
}
