namespace CorporationSyncify.Identity.WebApi.Events
{
    public sealed record IdentityCreatedEvent(
        string UserName,
        string Email)
    {
    }
}
