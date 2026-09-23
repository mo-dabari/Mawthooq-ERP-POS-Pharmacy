using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mawthooq.Application.Categories.Commands.Mappers;
using Mawthooq.Application.Categories.Exceptions;
using Mawthooq.Application.Products.Commands.Mappers;
using Mawthooq.Application.Products.Exceptions;
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

            var (product, _) = await _productRepository.GetByIdAsync(request.ProductIdDto.ToDomain(), cancellationToken);

            if (product is null)
                throw new ProductNotFoundException(request.ProductIdDto.Value);

            var newCategoryExists = await _categoryRepository.ExistsAndActiveAsync(request.NewCategoryId.ToDomain(), cancellationToken);

            if (!newCategoryExists)
                throw new CategoryNotFoundException(request.NewCategoryId.Value);

            product.ChangeCategory(request.NewCategoryId.ToDomain());
            var newRowVersion = await _productRepository.UpdateAsync(product, request.RowVersion, cancellationToken);

            return new ChangeProductCategoryResponse(
                product.ProductId.ToDto(),
                product.CategoryId.ToDto(),
                newRowVersion);
        }
    }
}
