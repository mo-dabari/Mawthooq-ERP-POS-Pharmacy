using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mawthooq.Application.Categories.Commands.Mappers;
using Mawthooq.Application.Common.Mappers;
using Mawthooq.Application.LocalAgents.Mappers;
using Mawthooq.Application.Manufacturers.Mappers;
using Mawthooq.Application.Products.Commands.UseCases.CreateProduct;
using Mawthooq.Domain.Categories.ValueObjects;
using Mawthooq.Domain.LocalAgents.ValueObjects;
using Mawthooq.Domain.Manufacturers.ValueObjects;
using Mawthooq.Domain.Products;
using Mawthooq.Domain.Products.Services;
using Mawthooq.Domain.Products.ValueObjects;

namespace Mawthooq.Application.Products.Commands.Mappers
{
    public static class CreateProductMapper
    {
        public static async Task<Product> ToDomainAsync(
            this CreateProductRequest request,
            CategoryId categoryId,
            SKU sKU,
            IInternalBarcodeSequenceGenerator barcodeGenerator,
            CancellationToken cancellationToken = default
            )
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            ArgumentNullException.ThrowIfNull(categoryId, nameof(categoryId));
            ArgumentNullException.ThrowIfNull(sKU, nameof(sKU));
            ArgumentNullException.ThrowIfNull(barcodeGenerator, nameof(sKU));

            var info = request.ProductInformationDto;
            var packaging = request.PackagingDto.ToDomain(info.Template);
            var manufacturerId = ManufacturerId.From(info.ManufacturerIdDto.Value);
            var localAgentId = LocalAgentId.From(info.LocalAgentIdDto.Value);
            var image = info.Image.ToDomain();

            var factory = new ProductFactory(barcodeGenerator);

            var product = await factory.CreateAsync(
               ProductId.New(),
               info.TradeName,
               sKU,
               categoryId,
               manufacturerId,
               localAgentId,
               packaging,
               image,
               info.RequiresExpiryTracking,
               info.CanBeReturnedIfOpened,
               info.NearExpiryThresholdInDays,
               cancellationToken);

            if (request.UserBarcodes.Count > 0)
            {
                var firstVersionId = product.PackagingVersions.Single().PackagingVersionId;

                foreach (var userBarcode in request.UserBarcodes)
                {
                    if (userBarcode is null)
                        continue;

                    product.AddCompanyBarcodeToVersion(
                        firstVersionId,
                        userBarcode.Unit,
                        userBarcode.BarcodeDto.ToDomain());
                }
            }

            return product;
        }

        public static CreateProductResponse ToResponseCreate(this Product product, byte[] rowVersion)
        {
            ArgumentNullException.ThrowIfNull(product, nameof(product));

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
                product.PackagingVersions.Select(pv => pv.ToDto()).ToList(),
                rowVersion);
        }
    }
}
