namespace CorporationSyncify.HRS.Domain.Entities.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> CreateUserAsync(
            string userName,
            string email,
            CancellationToken cancellationToken)
        {
            var user = new User(Guid.NewGuid(), userName, email);

            await _userRepository.InsertAsync(user, cancellationToken);

            return user;
        }
    }
}
