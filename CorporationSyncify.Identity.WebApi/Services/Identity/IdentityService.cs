using CorporationSyncify.Identity.WebApi.Data;
using CorporationSyncify.Identity.WebApi.Events;
using CorporationSyncify.Identity.WebApi.Outbox;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace CorporationSyncify.Identity.WebApi.Services.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly CorporationSyncifyIdentityDbContext _dbContext;

        public IdentityService(
            UserManager<IdentityUser> userManager,
            CorporationSyncifyIdentityDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<bool> CreateIdentityAsync(
            string userName,
            string email,
            string password,
            CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(userName)
                && !string.IsNullOrWhiteSpace(password))
            {
                var existingUser = await _userManager.FindByNameAsync(userName);

                if (existingUser == null)
                {
                    var newUser = new IdentityUser
                    {
                        UserName = userName,
                        Email = email,
                        EmailConfirmed = true // for now - all emails are confirmed
                    };

                    var result = await _userManager.CreateAsync(
                       newUser,
                       password);

                    if (result.Succeeded)
                    {
                        var identityCreatedEvent = new IdentityCreatedEvent(Guid.NewGuid(), userName, email);

                        await _dbContext.OutboxMessages.AddAsync(new OutboxMessage
                        {
                            Id = Guid.NewGuid(),
                            OccuredOnUtc = DateTimeOffset.UtcNow,
                            Type = identityCreatedEvent.GetType().Name,
                            Content = JsonConvert.SerializeObject(identityCreatedEvent,
                                new JsonSerializerSettings
                                {
                                    TypeNameHandling = TypeNameHandling.All
                                })
                        }, cancellationToken);
                        await _dbContext.SaveChangesAsync(cancellationToken);

                        return true;
                    }
                }
            }

            return false;
        }
    }
}
