
namespace CorporationSyncify.Identity.WebApi.Events
{
    public sealed record IdentityCreatedEvent(
        Guid EventId,
        string UserName,
        string Email) : IIdentityEvent
    {
        public string Topic => nameof(IdentityCreatedEvent);
    }
}
