namespace CorporationSyncify.Identity.WebApi.BackgroundJobs
{
    public class BackgroundJobOptions
    {
        public OutboxOptions? Outbox { get; set; }
    }

    public class OutboxOptions
    {
        public int JobDelaySeconds { get; set; }

        public int BatchSize { get; set; }

        public RetryPolicyOptions? RetryPolicyOptions { get; set; }
    }

    public class RetryPolicyOptions
    {
        public int MaxRetryAttempts { get; set; }

        public int DelaySeconds { get; set; }
    }
}