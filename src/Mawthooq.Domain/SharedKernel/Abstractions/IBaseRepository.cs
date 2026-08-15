using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Domain.SharedKernel.Abstractions
{
    public interface IBaseRepository<T, TId> where T : class
    {
        Task<T?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
        Task<byte[]> UpdateAsync(T entity, byte[] expectedRowVersion, CancellationToken cancellationToken = default);
        Task<T?> AddAsync(T entity, CancellationToken cancellationToken = default);
    }
}
