using MediatR;

namespace CorporationSyncify.Identity.WebApi.Events
{
    public class IdentityCreatedEventHandler : INotificationHandler<IdentityCreatedEvent>
    {
        private readonly ILogger<IdentityCreatedEventHandler> _logger;

        public IdentityCreatedEventHandler(ILogger<IdentityCreatedEventHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(IdentityCreatedEvent request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Identity event handling. User creation: {UserName}, {Email}", 
                request.UserName, 
                request.Email);

            await Task.Delay(1000);

            // TODO: Push event to kafka
        }
    }
}
