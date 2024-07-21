using CorporationSyncify.Identity.WebApi.Data;
using CorporationSyncify.Identity.WebApi.Events;
using CorporationSyncify.Identity.WebApi.Outbox;
using Dapper;
using MediatR;
using Newtonsoft.Json;
using System.Data;

namespace CorporationSyncify.Identity.WebApi.BackgroundJobs
{
    public class ProcessOutboxMessagesJob : IProcessOutboxMessagesJob
    {
        private static readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        };
        private readonly BackgroundJobsOptions _backgroundJobsOptions;
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly ILogger<ProcessOutboxMessagesJob> _logger;
        private readonly IPublisher _publisher;

        public ProcessOutboxMessagesJob(
            BackgroundJobsOptions backgroundJobsOptions,
            IDbConnectionFactory dbConnectionFactory,
            ILogger<ProcessOutboxMessagesJob> logger,
            IPublisher publisher)
        {
            _backgroundJobsOptions = backgroundJobsOptions;
            _dbConnectionFactory = dbConnectionFactory;
            _logger = logger;
            _publisher = publisher;
        }

        public async Task ProcessAsync()
        {
            _logger.LogInformation("Beginning to process outbox messages");

            using IDbConnection connection = await _dbConnectionFactory.GetOpenConnection();
            using IDbTransaction transaction = connection.BeginTransaction();

            var outboxMessages = await GetOutboxMessagesAsync(connection, transaction);

            if (outboxMessages.Count == 0)
            {
                _logger.LogInformation("Completed processing outbox messages - no messages to process");
                return;
            }

            foreach (var outboxMessage in outboxMessages)
            {
                Exception? exception = null;

                try
                {
                    var domainEvent = JsonConvert.DeserializeObject<IIdentityEvent>(
                        outboxMessage.Content,
                        _jsonSerializerSettings)!;

                    await _publisher.Publish(domainEvent);
                }
                catch (Exception caughtException)
                {
                    _logger.LogError(
                        caughtException,
                        "Exception while processing message {MessageId}",
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

            _logger.LogInformation("Completed processing outbox messages");
        }

        private async Task<IReadOnlyList<OutboxMessage>> GetOutboxMessagesAsync(
            IDbConnection connection,
            IDbTransaction transaction)
        {
            var query = """
                SELECT TOP (@BatchSize) "Id", "Content"
                FROM "OutboxMessages" WITH (ROWLOCK, READPAST)
                WHERE "ProcessedOnUtc" IS NULL
                ORDER BY "OccuredOnUtc"
                """;

            var outboxMessages = await connection.QueryAsync<OutboxMessage>(
                query,
                new { BatchSize = _backgroundJobsOptions?.Outbox?.BatchSize ?? 15 },
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
