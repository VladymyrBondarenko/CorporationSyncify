using CorporationSyncify.HRS.Domain.Abstractions;

namespace CorporationSyncify.HRS.Domain.Events
{
    public record IdentityCreatedEvent(
        Guid EventId,
        string Topic,
        string UserName,
        string Email) : IExternalEvent
    {
    }
}
