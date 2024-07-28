namespace CorporationSyncify.Identity.WebApi.Events
{
    public sealed record IdentityCreatedEvent(
        string UserName,
        string Email) : IIdentityEvent
    {
        public string EventName 
        { 
            get { return nameof(IdentityCreatedEvent); } 
        }
    }
}
