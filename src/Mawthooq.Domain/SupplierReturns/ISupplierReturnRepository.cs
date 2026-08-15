using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Inventories.ValueObjects;
using Mawthooq.Domain.SharedKernel.Abstractions;

namespace Mawthooq.Domain.SupplierReturns
{
    public interface ISupplierReturnRepository : IBaseRepository<SupplierReturn, int>
    {
        IReadOnlyList<SupplierReturn> GetSupplierReturns(int supplierId);

    }
}
