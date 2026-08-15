using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Commands.Mappers;
using Mawthooq.Domain.Products;

namespace Mawthooq.Application.Products.Commands.UseCases.ActivateProduct
{
    public class ActivateProductUseCase
    {
        private readonly IProductRepository _productRepository;

        public ActivateProductUseCase(IProductRepository productRepository)
        {
            ArgumentNullException.ThrowIfNull(productRepository, nameof(productRepository));

            _productRepository = productRepository;
        }

        public ActivateProductResponse Execute(ActivateProductRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            var product = _productRepository.GetById(request.ProductId.ToDomain());

            if (product is null)
                throw new InvalidOperationException($"Product with ID {request.ProductId.Value} not found.");

            product.Activate();

            var newRowVersion = _productRepository.Update(product, request.RowVersion);

            return new ActivateProductResponse(
                product.ProductId.ToDto(),
                product.TradeName,
                product.Status.ToString(),
                newRowVersion);
        }
    }
}
