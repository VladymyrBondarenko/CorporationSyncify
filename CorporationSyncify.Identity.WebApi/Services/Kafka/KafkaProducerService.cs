using Confluent.Kafka;
using CorporationSyncify.Identity.WebApi.Events;
using Newtonsoft.Json;
using System.Text;

namespace CorporationSyncify.Identity.WebApi.Services.Kafka
{
    public class KafkaProducerService : IKafkaProducerService
    {
        private readonly KafkaProducerOptions _producerOptions;

        public KafkaProducerService(KafkaProducerOptions producerOptions)
        {
            _producerOptions = producerOptions;
        }

        public async Task SendMessageAsync(
            IIdentityEvent identityEvent,
            CancellationToken cancellationToken)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = _producerOptions.BootstrapServers
            };
            using var producer = new ProducerBuilder<string, string>(config).Build();

            var kafkaMessage = new KafkaMessage
            {
                EventId = identityEvent.EventId,
                EmittedAt = DateTimeOffset.UtcNow,
                ContentBlob = Convert.ToBase64String(Encoding.UTF8.GetBytes(
                    JsonConvert.SerializeObject(identityEvent)))
            };

            await producer.ProduceAsync(
                identityEvent.Topic,
                new Message<string, string> 
                { 
                    Key = kafkaMessage.EventId.ToString(),
                    Value = JsonConvert.SerializeObject(kafkaMessage),
                    Timestamp = new Timestamp(kafkaMessage.EmittedAt)
                });
        }
    }
}
