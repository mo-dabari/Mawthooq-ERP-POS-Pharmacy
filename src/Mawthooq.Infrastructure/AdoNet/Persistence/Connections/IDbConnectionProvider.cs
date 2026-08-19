using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Mawthooq.Infrastructure.AdoNet.Persistence.Connections
{
    public interface IDbConnectionProvider
    {
        Task<SqlConnection> GetConnectionAsync(CancellationToken cancellationToken = default);
    }
}
