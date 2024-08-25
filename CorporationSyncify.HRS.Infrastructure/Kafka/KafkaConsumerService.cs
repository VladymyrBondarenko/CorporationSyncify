using Confluent.Kafka;
using CorporationSyncify.HRS.Domain.ExternalEvents;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace CorporationSyncify.HRS.Infrastructure.Kafka
{
    public class KafkaConsumerService : IKafkaConsumerService
    {
        private readonly KafkaConsumerOptions _kafkaConsumerOptions;
        private readonly ILogger<KafkaConsumerService> _logger;
        private readonly IExternalEventFactory _externalEventFactory;
        private readonly IPublisher _publisher;

        public KafkaConsumerService(KafkaConsumerOptions kafkaConsumerOptions,
            ILogger<KafkaConsumerService> logger,
            IExternalEventFactory externalEventFactory,
            IPublisher publisher)
        {
            _kafkaConsumerOptions = kafkaConsumerOptions;
            _logger = logger;
            _externalEventFactory = externalEventFactory;
            _publisher = publisher;
        }

        public async Task StartProcessMessagesAsync(CancellationToken cancellationToken)
        {
            await Task.Yield();

            var config = new ConsumerConfig
            {
                BootstrapServers = _kafkaConsumerOptions.BootstrapServers,
                GroupId = _kafkaConsumerOptions.GroupId,
                AutoOffsetReset = _kafkaConsumerOptions.AutoOffsetReset,
                EnableAutoCommit = false
            };
            using var consumer = new ConsumerBuilder<byte[], byte[]>(config).Build();
            consumer.Subscribe(_kafkaConsumerOptions.TargetTopics);

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = consumer.Consume(cancellationToken);

                    if (consumeResult != null)
                    {
                        _logger.LogInformation("Processing kafka message. Topic: {Topic}, Value: {Value}.",
                            consumeResult.Topic,
                            consumeResult.Message.Value);

                        var kafkaMessage = JsonSerializer.Deserialize<KafkaMessage>(
                            consumeResult.Message.Value);

                        if (kafkaMessage != null 
                            && !string.IsNullOrWhiteSpace(kafkaMessage.ContentBlob))
                        {
                            var externalEvent = _externalEventFactory.GetExternalEvent(
                                consumeResult.Topic, 
                                kafkaMessage.ContentBlob);

                            await _publisher.Publish(externalEvent);
                        }

                        consumer.Commit(consumeResult);
                    }
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError(ex, "Exception occured while trying to consume kafka message.");
                }
            }
        }
    }
}
