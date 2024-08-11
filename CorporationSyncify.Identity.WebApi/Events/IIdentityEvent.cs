using MediatR;

namespace CorporationSyncify.Identity.WebApi.Events
{
    public interface IIdentityEvent : INotification
    {
        public Guid EventId { get; }

        public string Topic { get; }
    }
}
