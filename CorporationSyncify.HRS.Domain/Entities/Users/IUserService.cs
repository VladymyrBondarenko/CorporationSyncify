
namespace CorporationSyncify.HRS.Domain.Entities.Users
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(string userName, string email, CancellationToken cancellationToken);
    }
}