using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Manufacturers;
using Mawthooq.Domain.Manufacturers.ValueObjects;
using Mawthooq.Infrastructure.AdoNet.Persistence.Connections;
using Microsoft.Data.SqlClient;

namespace Mawthooq.Infrastructure.AdoNet.Repositories.Manufacturers
{
    internal sealed class ManufacturerRepository : IManufacturerRepository
    {
        private const string ExistsAndActiveSql =
            "SELECT 1 FROM dbo.Manufacturers WHERE ManufacturerId = @ManufacturerId AND Status = 1;";

        private readonly IDbConnectionProvider _connectionProvider;

        public ManufacturerRepository(IDbConnectionProvider connectionProvider)
        {
            ArgumentNullException.ThrowIfNull(connectionProvider, nameof(connectionProvider));
            _connectionProvider = connectionProvider;
        }

        public async Task<bool> ExistsAndActiveAsync(ManufacturerId manufacturerId, CancellationToken cancellationToken = default)
        {
            SqlConnection connection = await _connectionProvider.GetConnectionAsync(cancellationToken);

            await using SqlCommand command = new(ExistsAndActiveSql, connection);

            command.Parameters.Add("@ManufacturerId", SqlDbType.UniqueIdentifier).Value = manufacturerId.Value;

            return await command.ExecuteScalarAsync(cancellationToken) is not null;
        }
    }
}
