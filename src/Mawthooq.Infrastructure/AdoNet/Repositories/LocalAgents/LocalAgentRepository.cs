using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.LocalAgents;
using Mawthooq.Domain.LocalAgents.ValueObjects;
using Mawthooq.Infrastructure.AdoNet.Persistence.Connections;
using Microsoft.Data.SqlClient;

namespace Mawthooq.Infrastructure.AdoNet.Repositories.LocalAgents
{
    internal sealed class LocalAgentRepository : ILocalAgentRepository
    {
        private const string ExistsAndActiveSql =
            "SELECT 1 FROM dbo.LocalAgents WHERE LocalAgentId = @LocalAgentId AND Status = 1;";

        private readonly IDbConnectionProvider _connectionProvider;

        public LocalAgentRepository(IDbConnectionProvider connectionProvider)
        {
            ArgumentNullException.ThrowIfNull(connectionProvider, nameof(connectionProvider));
            _connectionProvider = connectionProvider;
        }

        public async Task<bool> ExistsAndActiveAsync(LocalAgentId localAgentId, CancellationToken cancellationToken = default)
        {
            SqlConnection connection = await _connectionProvider.GetConnectionAsync(cancellationToken);

            await using SqlCommand command = new(ExistsAndActiveSql, connection);

            command.Parameters.Add("@LocalAgentId", SqlDbType.UniqueIdentifier).Value = localAgentId.Value;

            return await command.ExecuteScalarAsync(cancellationToken) is not null;
        }
    }
}
