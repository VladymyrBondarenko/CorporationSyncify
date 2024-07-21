using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CorporationSyncify.Identity.WebApi.Data
{
    public class CorporationSyncifyIdentityDbContextFactory : IDesignTimeDbContextFactory<CorporationSyncifyIdentityDbContext>
    {
        public CorporationSyncifyIdentityDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CorporationSyncifyIdentityDbContext>();
            optionsBuilder.UseSqlServer();

            return new CorporationSyncifyIdentityDbContext(optionsBuilder.Options);
        }
    }
}
