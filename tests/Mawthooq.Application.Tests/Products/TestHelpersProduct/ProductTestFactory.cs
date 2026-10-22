using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Categories.ValueObjects;
using Mawthooq.Domain.LocalAgents.ValueObjects;
using Mawthooq.Domain.Manufacturers.ValueObjects;
using Mawthooq.Domain.Products;
using Mawthooq.Domain.Products.Enums;
using Mawthooq.Domain.Products.ValueObjects;
using Mawthooq.Domain.Products.ValueObjects.Packaging;
using Mawthooq.Domain.SharedKernel.ValueObjects;

namespace Mawthooq.Application.Tests.Products.TestHelpersProduct
{
    internal static class ProductTestFactory
    {
        public static Product CreateValidProduct()
        {
            var packaging = SingleUnitPackaging.Create(UnitOfMeasure.Bottle);

            var product = Product.Create(
                ProductId.New(),
                "Panadol Extra",
                 SKU.Create("PANADOL-0001"),
                CategoryId.New(),
                ManufacturerId.New(),
                LocalAgentId.New(),
                packaging,
                1000,
                Image.Create("products/1/default.png", "default.png", "image/png"),
                requiresExpiryTracking: true,
                canBeReturnedIfOpened: true,
                nearExpiryThresholdInDays: Product.MinDays);

            product.ClearDomainEvents();
            return product;
        }

        public static Product CreateProductWithoutExpiryTracking()
        {
            var product = Product.Create(
                ProductId.New(), "Digital Thermometer", SKU.Create("THERMO-0001"),
                CategoryId.New(), ManufacturerId.New(), LocalAgentId.New(),
                SingleUnitPackaging.Create(UnitOfMeasure.Piece), 1000,
                Image.Create("products/1/default.png", "default.png", "image/png"),
                requiresExpiryTracking: false,
                canBeReturnedIfOpened: false,
                nearExpiryThresholdInDays: 0);
                
            product.ClearDomainEvents();
            return product;
        }
    }
}
