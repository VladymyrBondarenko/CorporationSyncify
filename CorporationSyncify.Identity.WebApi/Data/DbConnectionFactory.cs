using Microsoft.Data.SqlClient;

namespace CorporationSyncify.Identity.WebApi.Data
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly IConfiguration _configuration;

        public DbConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<SqlConnection> GetOpenConnection(CancellationToken cancellationToken = default)
        {
            var connection = new SqlConnection(_configuration.GetConnectionString("IdentityDbConnection"));
            await connection.OpenAsync(cancellationToken);
            return connection;
        }
    }
}
