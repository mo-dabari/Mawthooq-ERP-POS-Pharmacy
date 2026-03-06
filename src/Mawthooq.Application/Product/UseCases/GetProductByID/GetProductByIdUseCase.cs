using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Category;
using Mawthooq.Domain.Product;

namespace Mawthooq.Application.Product.UseCases.GetProductByID
{
    public class GetProductByIdUseCase
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        public GetProductByIdUseCase(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public GetProductByIdResponse Execute(int productId)
        {
            var product = _productRepository.GetById(productId);

            if (product == null)
                throw new InvalidOperationException($"Product with id {productId} not found.");

            var category = _categoryRepository.GetById(product.CategoryId);

            if (category == null)
                throw new InvalidOperationException($"Category with id {product.CategoryId} not found.");

            return new GetProductByIdResponse(product.ProductId, product.Name, category.Name, product.SKU, product.IsActive, product.Barcode);
        }
    }
}
