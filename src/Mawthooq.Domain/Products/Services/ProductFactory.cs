using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Categories.ValueObjects;
using Mawthooq.Domain.Products.ValueObjects;
using Mawthooq.Domain.Products.ValueObjects.Barcodes;
using Mawthooq.Domain.Products.ValueObjects.Packaging;

namespace Mawthooq.Domain.Products.Services
{
    public sealed class ProductFactory
    {
        private readonly IInternalBarcodeSequenceGenerator _barcodeGenerator;

        public ProductFactory(IInternalBarcodeSequenceGenerator barcodeGenerator)
        {
            ArgumentNullException.ThrowIfNull(barcodeGenerator, nameof(barcodeGenerator));
            _barcodeGenerator = barcodeGenerator;
        }

        public Product Create(
            ProductId productId,
            string name,
            SKU sku,
            CategoryId categoryId,
            ProductPackaging packaging,
            bool requiresExpiryTracking = true,
            bool canBeReturnedIfOpened = false,
            int nearExpiryThresholdInDays = 30)
        {
            int unitsCount = packaging.GetUnitDefinitions().Count();
            long startBarcodeSequence = _barcodeGenerator.GetNextBarcodeSequence(unitsCount);


            return Product.Create(
                            productId,
                            name,
                            sku,
                            categoryId,
                            packaging,
                            startBarcodeSequence,
                            requiresExpiryTracking,
                            canBeReturnedIfOpened,
                            nearExpiryThresholdInDays); ;
        }
    }
}
