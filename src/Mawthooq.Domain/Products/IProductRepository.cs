using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Products.ValueObjects;
using Mawthooq.Domain.Products.ValueObjects.Barcodes;
using Mawthooq.Domain.SharedKernel.Abstractions;

namespace Mawthooq.Domain.Products
{
    public interface IProductRepository : IBaseRepository<Product, ProductId>
    {
        Task<bool> ExistsBySkuAsync(SKU sku, CancellationToken cancellationToken = default);
        Task<bool> ExistsByBarcodeAsync(Barcode barcode, CancellationToken cancellationToken = default);
    }
}
