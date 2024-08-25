namespace CorporationSyncify.HRS.Infrastructure.Persistence.Models.Users
{
    public class UserModel
    {
        public Guid Id { get; set; }

        public required string UserName { get; set; }

        public required string Email { get; set; }
    }
}
