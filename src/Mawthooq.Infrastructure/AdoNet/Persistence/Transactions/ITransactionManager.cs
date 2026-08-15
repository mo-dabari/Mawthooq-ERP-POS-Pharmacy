using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Infrastructure.AdoNet.Persistence.Abstractions
{
    public interface ITransactionManager : IAsyncDisposable
    {
        IDbConnection Connection { get; }

        IDbTransaction Transaction { get; }

        Task BeginTransactionAsync(
        CancellationToken cancellationToken = default);

        Task CommitAsync(
            CancellationToken cancellationToken = default);

        Task RollbackAsync(
            CancellationToken cancellationToken = default);
    }
}
