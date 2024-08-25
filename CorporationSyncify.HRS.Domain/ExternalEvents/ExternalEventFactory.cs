using CorporationSyncify.HRS.Domain.Abstractions;
using CorporationSyncify.HRS.Domain.Events;
using System.Text.Json;

namespace CorporationSyncify.HRS.Domain.ExternalEvents
{
    public class ExternalEventFactory : IExternalEventFactory
    {
        public IExternalEvent GetExternalEvent(
            string topic,
            string base64String)
        {
            var bytes = Convert.FromBase64String(base64String);

            var externalEvent = topic switch
            {
                "IdentityCreatedEvent" => JsonSerializer.Deserialize<IdentityCreatedEvent>(bytes),
                _ => throw new NotImplementedException($"Handler for specified {topic} topic was not implemented")
            };

            if (externalEvent == null)
            {
                throw new Exception("Failed while trying to deserialize event object");
            }

            return externalEvent;
        }
    }
}
