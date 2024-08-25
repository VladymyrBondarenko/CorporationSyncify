using Confluent.Kafka;

namespace CorporationSyncify.HRS.Infrastructure.Kafka
{
    public class KafkaConsumerOptions
    {
        public string? BootstrapServers { get; set; }

        public string? GroupId { get; set; }

        public AutoOffsetReset AutoOffsetReset { get; set; }

        public IEnumerable<string>? TargetTopics => Topics?.Split(',', StringSplitOptions.RemoveEmptyEntries);

        public string? Topics { get; set; }
    }
}
