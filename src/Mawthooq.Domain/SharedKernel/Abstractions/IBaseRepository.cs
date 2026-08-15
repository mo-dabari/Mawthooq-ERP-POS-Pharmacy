using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Domain.SharedKernel.Abstractions
{
    public interface IBaseRepository<T, TId> where T : class
    {
        T? GetById(TId id);
        byte[] Update(T entity, byte[] expectedRowVersion);
        T? Add(T entity);
    }
}
