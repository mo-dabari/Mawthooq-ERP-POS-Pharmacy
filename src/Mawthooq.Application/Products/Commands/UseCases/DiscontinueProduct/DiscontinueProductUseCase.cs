using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Commands.Mappers;
using Mawthooq.Domain.Products;
using Mawthooq.Domain.Products.Enums;

namespace Mawthooq.Application.Products.Commands.UseCases.DiscontinueProduct
{
    public class DiscontinueProductUseCase
    {
        private readonly IProductRepository _productRepository;
        public DiscontinueProductUseCase(IProductRepository productRepository)
        {
            ArgumentNullException.ThrowIfNull(productRepository, nameof(productRepository));
            _productRepository = productRepository;
        }

        public DiscontinueProductResponse Execute(DiscontinueProductRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            var product = _productRepository.GetById(request.ProductId.ToDomain());

            if (product is null)
                throw new InvalidOperationException($"Product with ID {request.ProductId.Value} not found.");

            product.Deactivate();

            var newRowVersion = _productRepository.Update(product, request.RowVersion);

            return new DiscontinueProductResponse(
                product.ProductId.ToDto(),
                product.TradeName,
                product.Status.ToString(),
                newRowVersion);
        }
    }
}
