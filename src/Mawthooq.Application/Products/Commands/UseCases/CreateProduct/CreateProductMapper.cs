using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mawthooq.Application.Categories.Commands.Mappers;
using Mawthooq.Application.Common.Mappers;
using Mawthooq.Application.LocalAgents.Mappers;
using Mawthooq.Application.Manufacturers.Mappers;
using Mawthooq.Application.Products.Commands.Mappers;
using Mawthooq.Domain.Products;
using Mawthooq.Domain.Products.ValueObjects.Packaging;



namespace Mawthooq.Application.Products.Commands.UseCases.CreateProduct
{
    public static class CreateProductMapper
    {
        public static ProductPackaging ToPackagingDomain(
            this CreateProductRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var info = request.ProductInformation;

            return request.Packaging.ToDomain(info.Template);
        }

        public static CreateProductResponse ToResponseCreate(
            this Product product,
            byte[] rowVersion)
        {
            ArgumentNullException.ThrowIfNull(product);

            return new CreateProductResponse(
                product.ProductId.ToDto(),
                product.TradeName,
                product.SKU.ToDto(),
                product.CategoryId.ToDto(),
                product.ManufacturerId.ToDto(),
                product.LocalAgentId.ToDto(),
                product.RequiresExpiryTracking,
                product.CanBeReturnedIfOpened,
                product.NearExpiryThresholdInDays,
                product.Image.ToDto(),
                product.PackagingVersions
                    .Select(p => p.ToDto())
                    .ToList(),
                rowVersion);
        }
    }
}
