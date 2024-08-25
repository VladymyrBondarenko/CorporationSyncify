
namespace CorporationSyncify.HRS.Infrastructure.Persistence
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}