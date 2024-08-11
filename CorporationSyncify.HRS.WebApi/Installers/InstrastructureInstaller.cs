using CorporationSyncify.HRS.Application;
using CorporationSyncify.HRS.Domain.Entities.Users;
using CorporationSyncify.HRS.Infrastructure.BackgroundJobs;
using CorporationSyncify.HRS.Infrastructure.Kafka;
using CorporationSyncify.HRS.Infrastructure.Persistence;
using CorporationSyncify.HRS.Infrastructure.Persistence.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace CorporationSyncify.HRS.WebApi.Installers
{
    public static class InstrastructureInstaller
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(CorporationSyncifyHrsAppRoot).Assembly);
            });

            var kafkaConsumerOptions = configuration
                .GetSection(nameof(KafkaConsumerOptions))
                .Get<KafkaConsumerOptions>()!;
            services.AddSingleton(kafkaConsumerOptions);

            services.AddScoped<IKafkaConsumerService, KafkaConsumerService>();

            services.AddHostedService<ProcessKafkaMessagesJob>();

            services.AddPersistence(configuration);

            return services;
        }

        private static IServiceCollection AddPersistence(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<CorporationSyncifyHrsDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("HrsDbConnection"),
                    opt => opt.MigrationsAssembly(typeof(CorporationSyncifyHrsDbContext).Assembly.FullName));
            });

            services
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
