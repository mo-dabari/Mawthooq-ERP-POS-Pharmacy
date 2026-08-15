using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Categories.Commands.Mappers;
using Mawthooq.Application.Common.Mappers;
using Mawthooq.Application.Products.Commands.Mappers;
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

        public UpdateProductInformationResponse Execute(UpdateProductInformationRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            var product = _productRepository.GetById(request.ProductId.ToDomain());
            if (product is null)
                throw new InvalidOperationException($"Product with id {request.ProductId.Value} not found.");

            var category = _categoryRepository.GetById(request.CategoryId.ToDomain());
            if (category is null)
                throw new InvalidOperationException($"Category with id {request.CategoryId} not found.");

            product.Rename(request.TradeName);

            product.ChangeCategory(category.CategoryId);

            if (request.CanBeReturnedIfOpened)
                product.AllowReturnIfOpened();
            else
                product.DisallowReturnIfOpened();

            product.ChangeNearExpiryThreshold(request.NearExpiryThresholdInDays);

            product.ChangeImage(request.Image.ToDomain());

            var newRowVersion = _productRepository.Update(product, request.RowVersion);

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
