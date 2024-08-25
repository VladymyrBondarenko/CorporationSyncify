namespace CorporationSyncify.HRS.Domain.Entities.Users
{
    public class User
    {
        public User(Guid id, string userName, string email)
        {
            Id = id;
            UserName = userName;
            Email = email;
        }

        public Guid Id { get; private set; }

        public string UserName { get; private set; }

        public string Email { get; private set; }
    }
}
