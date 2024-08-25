
namespace CorporationSyncify.HRS.Infrastructure.Kafka
{
    public interface IKafkaConsumerService
    {
        Task StartProcessMessagesAsync(CancellationToken cancellationToken);
    }
}