
namespace CorporationSyncify.HRS.Domain.Entities.Users
{
    public interface IUserRepository
    {
        Task InsertAsync(User user, CancellationToken cancellationToken);
    }
}
