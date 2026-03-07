using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Products.UseCases.UpdateProduct;
using Mawthooq.Domain.Categories;
using Mawthooq.Domain.Products;

namespace Mawthooq.Application.Products.UseCases.UpdateProduct
{
    public class UpdateProductUseCase
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        public UpdateProductUseCase(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public UpdateProductResponse Execute(UpdateProductRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            var product = _productRepository.GetById(request.ProductId);

            if (product is null)
                throw new InvalidOperationException($"Product with id {request.ProductId} not found.");

            var category = _categoryRepository.GetById(request.CategoryId);

            if (category is null)
                throw new InvalidOperationException($"Category with id {request.CategoryId} not found.");

            product.ChangeProductName(request.Name);
            product.ChangeCategory(request.CategoryId);
            product.ChangeSKU(request.SKU);
            product.SetBarcode(request.Barcode);

            if (request.IsActive)
                product.Activate();
            else
                product.Deactivate();

            _productRepository.Update(product);

            return new UpdateProductResponse(
                product.ProductId,
                product.Name,
                product.IsActive);
        }
    }
}
