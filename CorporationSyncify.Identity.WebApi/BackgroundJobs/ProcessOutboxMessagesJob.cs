using CorporationSyncify.Identity.WebApi.Data;
using CorporationSyncify.Identity.WebApi.Events;
using CorporationSyncify.Identity.WebApi.Outbox;
using Dapper;
using MediatR;
using Newtonsoft.Json;
using Polly;
using System.Data;

namespace CorporationSyncify.Identity.WebApi.BackgroundJobs
{
    public class ProcessOutboxMessagesJob : BackgroundService
    {
        private static readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        };
        private readonly IServiceProvider _serviceProvider;

        public ProcessOutboxMessagesJob(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await using var scope = _serviceProvider.CreateAsyncScope();

                var logger = scope.ServiceProvider.GetRequiredService<ILogger<ProcessOutboxMessagesJob>>();
                var dbConnectionFactory = scope.ServiceProvider.GetRequiredService<IDbConnectionFactory>();
                var outboxProcessorOptions = scope.ServiceProvider.GetRequiredService<BackgroundJobOptions>()?.Outbox;
                var publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();

                logger.LogInformation("Beginning to process outbox messages");

                using IDbConnection connection = await dbConnectionFactory.GetOpenConnection();
                using IDbTransaction transaction = connection.BeginTransaction();

                var outboxMessages = await GetOutboxMessagesAsync(
                    connection, 
                    transaction, 
                    outboxProcessorOptions);

                if (outboxMessages.Count > 0)
                {
                    foreach (var outboxMessage in outboxMessages)
                    {
                        Exception? exception = null;

                        try
                        {
                            var resiliencePipeline = new ResiliencePipelineBuilder()
                                .AddRetry(new Polly.Retry.RetryStrategyOptions
                                {
                                    MaxRetryAttempts = outboxProcessorOptions?.RetryPolicyOptions?.MaxRetryAttempts ?? 5,
                                    BackoffType = DelayBackoffType.Constant,
                                    Delay = TimeSpan.FromSeconds(outboxProcessorOptions?.RetryPolicyOptions?.DelaySeconds ?? 5),
                                    ShouldHandle = new PredicateBuilder().Handle<Exception>(), // TODO: set relevant exceptions handling
                                    OnRetry = args =>
                                    {
                                        logger.LogError(
                                            args.Outcome.Exception,
                                            "Exception while processing message {MessageId}. Attempt number: {AttemptNumber}",
                                            outboxMessage.Id,
                                            args.AttemptNumber);

                                        return ValueTask.CompletedTask;
                                    }
                                }).Build();

                            await resiliencePipeline.ExecuteAsync(async cts =>
                            {
                                var domainEvent = JsonConvert.DeserializeObject<IIdentityEvent>(
                                    outboxMessage.Content,
                                    _jsonSerializerSettings)!;

                                await publisher.Publish(domainEvent, stoppingToken);
                            }, stoppingToken);
                        }
                        catch (Exception caughtException)
                        {
                            logger.LogError(
                                caughtException,
                                "Exception while processing message {MessageId}. Message exceed the maximum number of retry attempts",
                                outboxMessage.Id);

                            exception = caughtException;
                        }

                        await UpdateOutboxMessageAsync(
                            connection,
                            transaction,
                            outboxMessage,
                            exception);
                    }

                    transaction.Commit();

                    logger.LogInformation("Completed processing outbox messages");
                }
                else
                {
                    logger.LogInformation("Completed processing outbox messages - no messages to process");
                }

                await Task.Delay(
                    TimeSpan.FromSeconds(outboxProcessorOptions?.JobDelaySeconds ?? 60),
                    stoppingToken);
            }
        }

        private async Task<IReadOnlyList<OutboxMessage>> GetOutboxMessagesAsync(
            IDbConnection connection,
            IDbTransaction transaction,
            OutboxOptions? outboxProcessorOptions)
        {
            var query = """
                SELECT TOP (@BatchSize) "Id", "Content"
                FROM "OutboxMessages" WITH (ROWLOCK, READPAST)
                WHERE "ProcessedOnUtc" IS NULL
                ORDER BY "OccuredOnUtc"
                """;

            var outboxMessages = await connection.QueryAsync<OutboxMessage>(
                query,
                new { BatchSize = outboxProcessorOptions?.BatchSize ?? 15 },
                transaction
            );

            return outboxMessages.ToList();
        }

        private async Task UpdateOutboxMessageAsync(
            IDbConnection connection,
            IDbTransaction transaction,
            OutboxMessage outboxMessage,
            Exception? exception)
        {
            var command = """
                UPDATE "OutboxMessages"
                SET "ProcessedOnUtc" = @ProcessedDate, "Error" = @Error
                WHERE "Id" = @Id
                """;

            await connection.ExecuteAsync(
                command,
                new
                {
                    outboxMessage.Id,
                    Error = exception?.ToString(),
                    ProcessedDate = DateTimeOffset.UtcNow
                },
                transaction);
        }
    }
}
