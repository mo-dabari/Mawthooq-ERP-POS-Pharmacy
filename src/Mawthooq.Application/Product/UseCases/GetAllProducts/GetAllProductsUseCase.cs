using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Product;

namespace Mawthooq.Application.Product.UseCases.GetAllProducts
{
    public class GetAllProductsUseCase
    {

        private readonly IProductRepository _productRepository;
        public GetAllProductsUseCase(IProductRepository ProductRepository)
        {
            _productRepository = ProductRepository;
        }

        public IEnumerable<GetAllProductsResponse> Execute()
        {
            var products = _productRepository.GetAll();

            if (products is null || !products.Any())
                return Enumerable.Empty<GetAllProductsResponse>();

            return products.Select(product => new GetAllProductsResponse(product.ProductId, product.Name, product.IsActive, product?.Barcode));
        }
    }
}
