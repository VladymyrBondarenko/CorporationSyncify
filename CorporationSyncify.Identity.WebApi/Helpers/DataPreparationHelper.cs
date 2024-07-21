using CorporationSyncify.Identity.WebApi.Configurations;
using CorporationSyncify.Identity.WebApi.Data;
using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.EntityFramework.Storage;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CorporationSyncify.Identity.WebApi.Helpers
{
    public static class DataPreparationHelper
    {
        public static void PrepareDataPopulation(this WebApplication app)
        {
            var connectionString = app.Configuration.GetConnectionString("IdentityDbConnection");

            var services = new ServiceCollection();

            services.AddLogging();
            services.AddDbContext<CorporationSyncifyIdentityDbContext>(
                options => options.UseSqlServer(connectionString)
            );

            services
                .AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<CorporationSyncifyIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.AddOperationalDbContext(
                options =>
                {
                    options.ConfigureDbContext = db =>
                        db.UseSqlServer(
                            connectionString,
                            sql => sql.MigrationsAssembly(typeof(DataPreparationHelper).Assembly.FullName)
                        );
                }
            );
            services.AddConfigurationDbContext(
                options =>
                {
                    options.ConfigureDbContext = db =>
                        db.UseSqlServer(
                            connectionString,
                            sql => sql.MigrationsAssembly(typeof(DataPreparationHelper).Assembly.FullName)
                        );
                }
            );

            var serviceProvider = services.BuildServiceProvider();

            using var scope = serviceProvider
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();

            // Migrate PersistedGrantDbContext
            scope.ServiceProvider
                .GetService<PersistedGrantDbContext>()!
                .Database.Migrate();

            // Migrate ConfigurationDbContext and ensure seed data
            scope.ServiceProvider
                .GetService<ConfigurationDbContext>()!
                .MigrateAndEnsureSeedData(app.Configuration);

            // Migrate CorporationSyncifyIdentityDbContext and ensure seed data
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            scope.ServiceProvider
                .GetService<CorporationSyncifyIdentityDbContext>()!
                .MigrateAndEnsureSeedData(userManager, app.Configuration);
        }
    }

    #region Extensions

    public static class ConfigurationDbContextExtensions
    {
        public static void MigrateAndEnsureSeedData(
            this ConfigurationDbContext context,
            IConfiguration configuration)
        {
            context.Database.Migrate();

            var config = new IdentityServerConfiguration(configuration);

            if (!context.Clients.Any())
            {
                foreach (var client in config.Clients.ToList())
                {
                    context.Clients.Add(client.ToEntity());
                }

                context.SaveChanges();
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in config.IdentityResources.ToList())
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }

            if (!context.ApiScopes.Any())
            {
                foreach (var resource in config.ApiScopes.ToList())
                {
                    context.ApiScopes.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }

            if (!context.ApiResources.Any())
            {
                foreach (var resource in config.ApiResources.ToList())
                {
                    context.ApiResources.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }
        }
    }

    public static class CorporationSyncifyIdentityDbContextExtensions
    {
        public static void MigrateAndEnsureSeedData(
            this CorporationSyncifyIdentityDbContext context,
            UserManager<IdentityUser> userMgr,
            IConfiguration configuration)
        {
            context.Database.Migrate();

            var adminCreds = configuration
                .GetSection(nameof(IdentityServerOptions))
                .Get<IdentityServerOptions>()?.IdentityServerAdmin;

            if (adminCreds == null
                || string.IsNullOrWhiteSpace(adminCreds.UserName)
                || string.IsNullOrWhiteSpace(adminCreds.Password))
            {
                return;
            }

            var adminUser = userMgr.FindByNameAsync(adminCreds.UserName).Result;

            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminCreds.UserName,
                    Email = adminCreds.Email,
                    EmailConfirmed = true
                };

                var result = userMgr.CreateAsync(
                    adminUser,
                    adminCreds.Password).Result;

                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userMgr.AddClaimsAsync(adminUser,
                    new Claim[]
                    {
                        new Claim(JwtClaimTypes.Name, adminCreds.UserName),
                        new Claim(JwtClaimTypes.GivenName,adminCreds.UserName),
                        new Claim(JwtClaimTypes.FamilyName, adminCreds.UserName)
                    }).Result;

                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
            }
        }
    }

    #endregion
}
