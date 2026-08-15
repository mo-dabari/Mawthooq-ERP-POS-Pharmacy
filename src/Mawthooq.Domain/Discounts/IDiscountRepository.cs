using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.SharedKernel.Abstractions;

namespace Mawthooq.Domain.Discounts
{
    public interface IDiscountRepository : IBaseRepository<Discount, int>
    {
        IEnumerable<Discount> GetActiveDiscounts();
        Discount? GetByName(string name);
    }
}
