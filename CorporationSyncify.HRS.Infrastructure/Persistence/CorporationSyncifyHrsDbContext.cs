using CorporationSyncify.HRS.Infrastructure.Persistence.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace CorporationSyncify.HRS.Infrastructure.Persistence
{
    public class CorporationSyncifyHrsDbContext : DbContext
    {
        public CorporationSyncifyHrsDbContext(
            DbContextOptions<CorporationSyncifyHrsDbContext> options) : base(options)
        {
        }

        public DbSet<UserModel> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
