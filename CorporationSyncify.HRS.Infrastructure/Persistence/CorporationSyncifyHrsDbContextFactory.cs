using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CorporationSyncify.HRS.Infrastructure.Persistence
{
    internal class CorporationSyncifyHrsDbContextFactory : IDesignTimeDbContextFactory<CorporationSyncifyHrsDbContext>
    {
        public CorporationSyncifyHrsDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CorporationSyncifyHrsDbContext>();
            optionsBuilder.UseSqlServer();

            return new CorporationSyncifyHrsDbContext(optionsBuilder.Options);
        }
    }
}
