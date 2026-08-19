using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Commands.Mappers;
using Mawthooq.Domain.Products;

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

        public async Task<DiscontinueProductResponse> ExecuteAsync(DiscontinueProductRequest request, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            var (product, _) = await _productRepository.GetByIdAsync(request.ProductId.ToDomain(), cancellationToken);

            if (product is null)
                throw new InvalidOperationException($"Product with ID {request.ProductId.Value} not found.");

            product.Deactivate();

            var newRowVersion = await _productRepository.UpdateAsync(product, request.RowVersion, cancellationToken);

            return new DiscontinueProductResponse(
                product.ProductId.ToDto(),
                product.TradeName,
                product.Status.ToString(),
                newRowVersion);
        }
    }
}
