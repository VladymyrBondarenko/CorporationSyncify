using CorporationSyncify.Identity.WebApi.Services.Kafka;

namespace CorporationSyncify.Identity.WebApi.Installers
{
    public static class KafkaInstaller
    {
        public static IServiceCollection AddKafka(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            var kafkaProducerOptions = configuration
                .GetSection(nameof(KafkaProducerOptions))
                .Get<KafkaProducerOptions>()!;
            services.AddSingleton(kafkaProducerOptions);

            services.AddScoped<IKafkaProducerService, KafkaProducerService>();

            return services;
        }
    }
}
