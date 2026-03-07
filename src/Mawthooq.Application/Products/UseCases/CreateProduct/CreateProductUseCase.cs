using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Categories;
using Mawthooq.Domain.Products;

namespace Mawthooq.Application.Products.UseCases.CreateProduct
{
    public class CreateProductUseCase
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        public CreateProductUseCase(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public CreateProductResponse Execute(CreateProductRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            Category? category = _categoryRepository.GetById(request.CategoryId);

            if (category == null)
                throw new InvalidOperationException($"Category with id {request.CategoryId} not found.");

            var product = Product.Create(request.Name, request.SKU, request.Barcode, request.ImageUrl, request.CategoryId);

            var savedProduct = _productRepository.Add(product);

            return new CreateProductResponse(savedProduct.ProductId, category.Name, savedProduct.Name, savedProduct.SKU);
        }
    }
}
