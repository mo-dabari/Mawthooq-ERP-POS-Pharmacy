using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.SharedKernel.Abstractions;

namespace Mawthooq.Domain.Sales
{
    public interface ISaleRepository : IBaseRepository<Sale, int>
    {
    }
}
