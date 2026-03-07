using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Domain.Common.Interfaces
{
    public interface IBaseRepository <T, TId> where T : class
    {
        T? GetById(TId id);
        T Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
