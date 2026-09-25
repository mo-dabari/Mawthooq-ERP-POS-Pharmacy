using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mawthooq.Application.Categories.Commands.Mappers;
using Mawthooq.Application.Categories.Exceptions;
using Mawthooq.Application.Common.Mappers;
using Mawthooq.Application.Products.Commands.Mappers;
using Mawthooq.Application.Products.Exceptions;
using Mawthooq.Domain.Categories;
using Mawthooq.Domain.Products;

namespace Mawthooq.Application.Products.Commands.UseCases.UpdateProductInformation
{
    public class UpdateProductInformationUseCase
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        public UpdateProductInformationUseCase(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            ArgumentNullException.ThrowIfNull(productRepository, nameof(productRepository));
            ArgumentNullException.ThrowIfNull(categoryRepository, nameof(categoryRepository));

            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<UpdateProductInformationResponse> ExecuteAsync(UpdateProductInformationRequest request, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            UpdateProductInformationValidator.Validate(request);

            var (product, _) = await _productRepository.GetByIdAsync(request.ProductIdDto.ToDomain(), cancellationToken);
            if (product is null)
                throw new ProductNotFoundException(request.ProductIdDto.Value);

            var (category, _) = await _categoryRepository.GetByIdAsync(request.CategoryIdDto.ToDomain(), cancellationToken);
            if (category is null)
                throw new CategoryNotFoundException(request.CategoryIdDto.Value);

            product.Rename(request.TradeName);

            product.ChangeCategory(category.CategoryId);

            if (request.CanBeReturnedIfOpened)
                product.AllowReturnIfOpened();
            else
                product.DisallowReturnIfOpened();

            product.ChangeNearExpiryThreshold(request.NearExpiryThresholdInDays);

            product.ChangeImage(request.Image.ToDomain());

            var newRowVersion = await _productRepository.UpdateAsync(product, request.RowVersion, cancellationToken);

            return new UpdateProductInformationResponse(
                product.ProductId.ToDto(),
                product.TradeName,
                product.CategoryId.ToDto(),
                product.Image.ToDto(),
                product.CanBeReturnedIfOpened,
                product.NearExpiryThresholdInDays,
                newRowVersion);
        }
    }
}
