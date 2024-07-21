namespace CorporationSyncify.Identity.WebApi.BackgroundJobs
{
    public class BackgroundJobsOptions
    {
        public OutboxOptions? Outbox { get; set; }
    }

    public class OutboxOptions
    {
        public string? Schedule { get; set; }

        public int BatchSize { get; set; }
    }
}