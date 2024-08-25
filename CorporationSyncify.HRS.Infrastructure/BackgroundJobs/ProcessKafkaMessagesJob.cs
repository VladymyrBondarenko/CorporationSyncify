using CorporationSyncify.HRS.Infrastructure.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CorporationSyncify.HRS.Infrastructure.BackgroundJobs
{
    public class ProcessKafkaMessagesJob : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public ProcessKafkaMessagesJob(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();

            var kafkaConsumerService = scope.ServiceProvider.GetRequiredService<IKafkaConsumerService>();

            await kafkaConsumerService.StartProcessMessagesAsync(stoppingToken);
        }
    }
}
