using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Application.Products.Queries.UseCases.ScanProductBarcode
{
    public class ScanProductBarcodeHandler
    {
        private readonly IScanProductBarcodeQueryRepository _scanProductBarcodeQueryRepository;

        public ScanProductBarcodeHandler(IScanProductBarcodeQueryRepository scanProductBarcodeQueryRepository)
        {
            ArgumentNullException.ThrowIfNull(scanProductBarcodeQueryRepository, nameof(scanProductBarcodeQueryRepository));

            _scanProductBarcodeQueryRepository = scanProductBarcodeQueryRepository;
        }
        public Task<ScanProductBarcodeDto?> HandleAsync(
            ScanProductBarcodeQuery query,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(query, nameof(query));

            ScanProductBarcodeQueryValidator.Validate(query);

            return _scanProductBarcodeQueryRepository.ScanProductBarcodeAsync(query, cancellationToken);
        }
    }
}
