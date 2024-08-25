using CorporationSyncify.HRS.Domain.Entities.Users;

namespace CorporationSyncify.HRS.Infrastructure.Persistence.Models.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly CorporationSyncifyHrsDbContext _dbContext;

        public UserRepository(CorporationSyncifyHrsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task InsertAsync(User user, CancellationToken cancellationToken)
        {
            var userModel = new UserModel 
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName
            };
            await _dbContext.Users.AddAsync(userModel, cancellationToken);
        }
    }
}
