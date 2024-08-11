using CorporationSyncify.Identity.WebApi.Services.Kafka;
using MediatR;

namespace CorporationSyncify.Identity.WebApi.Events
{
    public class IdentityCreatedEventHandler : INotificationHandler<IdentityCreatedEvent>
    {
        private readonly ILogger<IdentityCreatedEventHandler> _logger;
        private readonly IKafkaProducerService _kafkaProducerService;

        public IdentityCreatedEventHandler(
            IKafkaProducerService kafkaProducerService,
            ILogger<IdentityCreatedEventHandler> logger)
        {
            _kafkaProducerService = kafkaProducerService;
            _logger = logger;
        }

        public async Task Handle(
            IdentityCreatedEvent request, 
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Identity event handling. User creation: {UserName}, {Email}", 
                request.UserName, 
                request.Email);

            await _kafkaProducerService.SendMessageAsync(request, cancellationToken);
        }
    }
}
