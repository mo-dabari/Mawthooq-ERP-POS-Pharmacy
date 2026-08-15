using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.SharedKernel.ValueObjects;

namespace Mawthooq.Domain.Inventories.ValueObjects
{
    public record class DeductionResult(Money TotalCost, Money SellingPrice, int quantity)
    {
        public Money TotalRevenue => SellingPrice.Multiply(quantity);
        public Money GrossProfit => TotalRevenue - TotalCost;
    }
}
