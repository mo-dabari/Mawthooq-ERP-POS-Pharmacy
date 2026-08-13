using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Domain.Products
{
    public interface IInternalBarcodeSequenceGenerator
    {
        long GetNextBarcodeSequence(int count);
    }
}
