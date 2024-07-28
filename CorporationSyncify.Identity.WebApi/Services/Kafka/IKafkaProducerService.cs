using CorporationSyncify.Identity.WebApi.Events;

namespace CorporationSyncify.Identity.WebApi.Services.Kafka
{
    public interface IKafkaProducerService
    {
        Task SendEventAsync(IIdentityEvent identityEvent, CancellationToken cancellationToken);
    }
}