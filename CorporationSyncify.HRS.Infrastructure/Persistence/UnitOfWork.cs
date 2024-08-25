namespace CorporationSyncify.HRS.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CorporationSyncifyHrsDbContext _dbContext;

        public UnitOfWork(CorporationSyncifyHrsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
