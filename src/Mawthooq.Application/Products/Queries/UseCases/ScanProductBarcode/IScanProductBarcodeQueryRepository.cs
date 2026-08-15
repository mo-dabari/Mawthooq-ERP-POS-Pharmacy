using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Application.Products.Queries.UseCases.ScanProductBarcode
{
    public interface IScanProductBarcodeQueryRepository
    {
        Task<ScanProductBarcodeDto?> ScanProductBarcodeAsync(
            ScanProductBarcodeQuery query,
            CancellationToken cancellationToken = default);
    }
}
