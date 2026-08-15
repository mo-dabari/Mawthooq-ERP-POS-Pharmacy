using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Inventories.Entities;

namespace Mawthooq.Domain.Inventories.ValueObjects
{
    public record BatchAllocation(Batch Batch, int Quantity);
}
