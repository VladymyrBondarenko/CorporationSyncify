namespace CorporationSyncify.Identity.WebApi.Services.Identity
{
    public interface IIdentityService
    {
        Task<bool> CreateIdentityAsync(string userName, string email, string password, CancellationToken cancellationToken);
    }
}