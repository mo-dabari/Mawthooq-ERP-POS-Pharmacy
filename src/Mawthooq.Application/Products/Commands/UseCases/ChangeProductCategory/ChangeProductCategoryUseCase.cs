using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mawthooq.Application.Categories.Commands.Mappers;
using Mawthooq.Application.Products.Commands.Mappers;
using Mawthooq.Domain.Categories;
using Mawthooq.Domain.Products;

namespace Mawthooq.Application.Products.Commands.UseCases.ChangeProductCategory
{
    public class ChangeProductCategoryUseCase
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ChangeProductCategoryUseCase(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            ArgumentNullException.ThrowIfNull(productRepository, nameof(productRepository));
            ArgumentNullException.ThrowIfNull(categoryRepository, nameof(categoryRepository));

            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<ChangeProductCategoryResponse> ExecuteAsync(ChangeProductCategoryRequest request, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            var product = await _productRepository.GetByIdAsync(request.ProductId.ToDomain(), cancellationToken);

            if (product is null)
                throw new InvalidOperationException($"Product with ID {request.ProductId.Value} not found.");

            var newCategoryExists = await _categoryRepository.ExistsAndActiveAsync(request.NewCategoryId.ToDomain(), cancellationToken);

            if (!newCategoryExists)
                throw new InvalidOperationException($"Category with ID {request.NewCategoryId.Value} not found.");

            product.ChangeCategory(request.NewCategoryId.ToDomain());
            var newRowVersion = await _productRepository.UpdateAsync(product, request.RowVersion, cancellationToken);

            return new ChangeProductCategoryResponse(
                product.ProductId.ToDto(),
                product.CategoryId.ToDto(),
                newRowVersion);
        }
    }
}
