using Microsoft.Data.SqlClient;

namespace CorporationSyncify.Identity.WebApi.Data
{
    public interface IDbConnectionFactory
    {
        Task<SqlConnection> GetOpenConnection(CancellationToken cancellationToken = default);
    }
}