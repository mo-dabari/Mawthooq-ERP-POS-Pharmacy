using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Infrastructure.AdoNet.Persistence.Connections;
using Microsoft.Data.SqlClient;

namespace Mawthooq.Infrastructure.ADO.Persistence.Connections
{
    internal sealed class DbConnectionProvider : IDbConnectionProvider
    {
        private readonly string _connectionString;
        private SqlConnection? _connection;

        public DbConnectionProvider(string connectionString)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(connectionString, nameof(connectionString));
            _connectionString = connectionString;
        }

        public async Task<SqlConnection> GetConnectionAsync(CancellationToken cancellationToken = default)
        {
            if (_connection is null)
            {
                _connection = new SqlConnection(_connectionString);
                await _connection.OpenAsync(cancellationToken);
            }
            else if (_connection.State != ConnectionState.Open)
            {
                await _connection.OpenAsync(cancellationToken);
            }

            return _connection;
        }

        public async ValueTask DisposeAsync()
        {
            if (_connection is not null)
                await _connection.DisposeAsync();
        }
    }
}
