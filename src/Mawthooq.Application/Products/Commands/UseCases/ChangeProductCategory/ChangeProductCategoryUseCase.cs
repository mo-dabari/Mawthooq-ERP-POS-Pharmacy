using System;
using System.Collections.Generic;
using System.Linq;
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

        public ChangeProductCategoryResponse Execute(ChangeProductCategoryRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            var product = _productRepository.GetById(request.ProductId.ToDomain());

            if (product is null)
                throw new InvalidOperationException($"Product with ID {request.ProductId.Value} not found.");

            var newCategoryExists = _categoryRepository.ExistsAndActive(request.NewCategoryId.ToDomain());

            if (!newCategoryExists)
                throw new InvalidOperationException($"Category with ID {request.NewCategoryId.Value} not found.");

            product.ChangeCategory(request.NewCategoryId.ToDomain());
            var newRowVersion = _productRepository.Update(product, request.RowVersion);

            return new ChangeProductCategoryResponse(
                product.ProductId.ToDto(),
                product.CategoryId.ToDto(),
                newRowVersion);
        }
    }
}
