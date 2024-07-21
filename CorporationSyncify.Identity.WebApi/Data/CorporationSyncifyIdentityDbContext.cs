using CorporationSyncify.Identity.WebApi.Data.Configurations;
using CorporationSyncify.Identity.WebApi.Outbox;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CorporationSyncify.Identity.WebApi.Data
{
    public class CorporationSyncifyIdentityDbContext : IdentityDbContext
    {
        public CorporationSyncifyIdentityDbContext(
            DbContextOptions<CorporationSyncifyIdentityDbContext> options) : base(options)
        {

        }

        public DbSet<OutboxMessage> OutboxMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
