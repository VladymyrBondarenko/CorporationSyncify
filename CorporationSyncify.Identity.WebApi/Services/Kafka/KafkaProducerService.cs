using Confluent.Kafka;
using CorporationSyncify.Identity.WebApi.Events;
using Newtonsoft.Json;

namespace CorporationSyncify.Identity.WebApi.Services.Kafka
{
    public class KafkaProducerService : IKafkaProducerService
    {
        private readonly KafkaProducerOptions _producerOptions;

        public KafkaProducerService(KafkaProducerOptions producerOptions)
        {
            _producerOptions = producerOptions;
        }

        public async Task SendEventAsync(
            IIdentityEvent identityEvent,
            CancellationToken cancellationToken)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = _producerOptions.BootstrapServers
            };
            using var producer = new ProducerBuilder<Null, string>(config).Build();

            var result = await producer.ProduceAsync(
                identityEvent.EventName,
                new Message<Null, string>
                {
                    Value = JsonConvert.SerializeObject(identityEvent,
                        new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.All
                        })
                }, cancellationToken);
        }
    }
}
