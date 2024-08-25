using CorporationSyncify.HRS.Domain.Entities.Users;
using CorporationSyncify.HRS.Domain.Events;
using CorporationSyncify.HRS.Infrastructure.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CorporationSyncify.HRS.Application.ExternalEvents
{
    internal class IdentityCreatedEventHandler : INotificationHandler<IdentityCreatedEvent>
    {
        private readonly ILogger<IdentityCreatedEventHandler> _logger;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;

        public IdentityCreatedEventHandler(
            ILogger<IdentityCreatedEventHandler> logger,
            IUserService userService,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _userService = userService;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(IdentityCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Handling event IdentityCreatedEvent: {EventId}, {UserName}, {Email}", 
                notification.EventId, 
                notification.UserName, 
                notification.Email);

            await _userService.CreateUserAsync(
                notification.UserName, 
                notification.Email, 
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
