using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Categories.Commands.Mappers;
using Mawthooq.Application.Common.Interfaces;
using Mawthooq.Application.Products.Commands.Mappers;
using Mawthooq.Domain.Categories;
using Mawthooq.Domain.Products;
using Mawthooq.Domain.Products.ValueObjects;

namespace Mawthooq.Application.Products.Commands.UseCases.CreateProduct
{
    public class CreateProductUseCase
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IInternalBarcodeSequenceGenerator _barcodeGenerator;

        public CreateProductUseCase(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IInternalBarcodeSequenceGenerator barcodeGenerator)
        {
            ArgumentNullException.ThrowIfNull(productRepository, nameof(productRepository));
            ArgumentNullException.ThrowIfNull(categoryRepository, nameof(categoryRepository));
            ArgumentNullException.ThrowIfNull(barcodeGenerator, nameof(barcodeGenerator));

            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _barcodeGenerator = barcodeGenerator;
        }

        public async Task<CreateProductResponse> ExecuteAsync(
            CreateProductRequest request,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            var info = request.ProductInformationDto;

            Category? category = await _categoryRepository.GetByIdAsync(
                info.CategoryIdDto.ToDomain(),
                cancellationToken);

            if (category == null)
                throw new InvalidOperationException($"Category with {info.CategoryIdDto.Value} not found.");

            SKU sKU = info.Sku.ToDomain();

            if (await _productRepository.ExistsBySkuAsync(sKU, cancellationToken))
                throw new InvalidOperationException($"SKU '{sKU.Value}' already exists.");

            var product = await request.ToDomainAsync(category.CategoryId, sKU, _barcodeGenerator);

            var savedProduct = await _productRepository.AddAsync(product,cancellationToken);

            if (savedProduct is null)
                throw new InvalidOperationException("Failed to create product");

            return savedProduct.ToResponseCreate();

        }
    }
}
