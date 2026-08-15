using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Domain.Products
{
    public interface IInternalBarcodeSequenceGenerator
    {
        Task<long> GetNextBarcodeSequence(int count, CancellationToken cancellationToken = default);
    }
}
