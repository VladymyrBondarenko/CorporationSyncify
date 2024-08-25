using CorporationSyncify.HRS.Domain.Abstractions;

namespace CorporationSyncify.HRS.Domain.ExternalEvents
{
    public interface IExternalEventFactory
    {
        IExternalEvent GetExternalEvent(string topic, string base64String);
    }
}