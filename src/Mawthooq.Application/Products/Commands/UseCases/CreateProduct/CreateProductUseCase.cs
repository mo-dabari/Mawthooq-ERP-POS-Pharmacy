using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Categories.Commands.Mappers;
using Mawthooq.Application.Common.Mappers;
using Mawthooq.Application.Products.Commands.Mappers;
using Mawthooq.Domain.Categories;
using Mawthooq.Domain.LocalAgents.ValueObjects;
using Mawthooq.Domain.Manufacturers.ValueObjects;
using Mawthooq.Domain.Products;
using Mawthooq.Domain.Products.Services;
using Mawthooq.Domain.Products.ValueObjects;

namespace Mawthooq.Application.Products.Commands.UseCases.CreateProduct
{
    public class CreateProductUseCase
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ProductFactory _productFactory;

        public CreateProductUseCase(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            ProductFactory productFactory)
        {
            ArgumentNullException.ThrowIfNull(productRepository, nameof(productRepository));
            ArgumentNullException.ThrowIfNull(categoryRepository, nameof(categoryRepository));
            ArgumentNullException.ThrowIfNull(productFactory, nameof(productFactory));

            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _productFactory = productFactory;
        }

        public async Task<CreateProductResponse> ExecuteAsync(
            CreateProductRequest request,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            var info =
                request.ProductInformationDto;

            var (category, _) = await _categoryRepository.GetByIdAsync(
            info.CategoryIdDto.ToDomain(),
            cancellationToken);

            if (category == null)
                throw new InvalidOperationException($"Category with {info.CategoryIdDto.Value} not found.");

            SKU sKU = info.Sku.ToDomain();

            if (await _productRepository.ExistsBySkuAsync(sKU, cancellationToken))
                throw new InvalidOperationException($"SKU '{sKU.Value}' already exists.");


            var packaging =
                request.ToPackagingDomain();

            var manufacturerId =
                ManufacturerId.From(info.ManufacturerIdDto.Value);

            var localAgentId =
                LocalAgentId.From(info.LocalAgentIdDto.Value);

            var image =
                info.Image.ToDomain();

            var product = await _productFactory
                .CreateAsync(
                    ProductId.New(),
                    info.TradeName,
                    sKU,
                    category.CategoryId,
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
                var firstVersionId =
                    product.PackagingVersions.Single().PackagingVersionId;

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

            var (savedProduct, rowVersion) = await _productRepository.AddAsync(product, cancellationToken);

            if (savedProduct is null || rowVersion is null)
                throw new InvalidOperationException("Failed to create product.");

            return savedProduct.ToResponseCreate(rowVersion);
        }
    }
}
