using CorporationSyncify.HRS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CorporationSyncify.HRS.WebApi.Helpers
{
    public static class DataPreparationHelper
    {
        public static void PrepareDataPopulation(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<CorporationSyncifyHrsDbContext>()!;
            dbContext.Database.Migrate();
        }
    }
}
