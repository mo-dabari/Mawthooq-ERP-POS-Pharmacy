using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Infrastructure.AdoNet.Persistence.Abstractions;
using Mawthooq.Infrastructure.AdoNet.Persistence.Connections;

namespace Mawthooq.Infrastructure.AdoNet.Persistence.Transactions
{
    internal sealed class SqlTransactionManager : ITransactionManager
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private SqlConnection? _connection;
        private SqlTransaction? _transaction;

        public SqlTransactionManager(IDbConnectionFactory connectionFactory)
        {
            ArgumentNullException.ThrowIfNull(connectionFactory, nameof(connectionFactory));
            _connectionFactory = connectionFactory;
        }
        public IDbConnection Connection =>
            _connection ?? throw new InvalidOperationException(
                "Transaction has not been started.");

        public IDbTransaction Transaction =>
                _transaction ?? throw new InvalidOperationException(
                    "Transaction has not been started.");
        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction is not null)
                throw new InvalidOperationException(
                    "Transaction has already been started.");

            _connection = (SqlConnection)_connectionFactory.CreateConnection();

            await _connection.OpenAsync(cancellationToken);

            _transaction = (SqlTransaction)await _connection.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction is null)
                throw new InvalidOperationException(
                    "No active transaction.");

            await _transaction.CommitAsync(cancellationToken);
        }

        public async ValueTask DisposeAsync()
        {
            if (_transaction is not null)
                await _transaction.DisposeAsync();

            if (_connection is not null)
                await _connection.DisposeAsync();
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction is null)
                throw new InvalidOperationException(
                    "No active transaction.");
            await _transaction.RollbackAsync(cancellationToken);

        }
    }
}
