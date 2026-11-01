using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Mawthooq.Infrastructure.AdoNet.Persistence.Connections
{
    internal sealed class DbConnectionProvider : IDbConnectionProvider, IAsyncDisposable
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
            _connection ??= new SqlConnection(_connectionString);

            if (_connection.State == ConnectionState.Broken)
                await _connection.CloseAsync();

            if (_connection.State != ConnectionState.Open)
                await _connection.OpenAsync(cancellationToken);

            return _connection;
        }

        public async ValueTask DisposeAsync()
        {
            if (_connection is not null)
            {
                await _connection.DisposeAsync();
                _connection = null;
            }
        }
    }
}
