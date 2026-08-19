using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Commands.Mappers;
using Mawthooq.Domain.Products;

namespace Mawthooq.Application.Products.Commands.UseCases.DeactivatePackagingVersion
{
    public class DeactivatePackagingVersionUseCase
    {
        private readonly IProductRepository _productRepository;

        public DeactivatePackagingVersionUseCase(IProductRepository productRepository)
        {
            ArgumentNullException.ThrowIfNull(productRepository, nameof(productRepository));

            _productRepository = productRepository;
        }

        public async Task<DeactivatePackagingVersionResponse> ExecuteAsync(DeactivatePackagingVersionRequest request, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            var (product, _) = await _productRepository.GetByIdAsync(request.ProductIdDto.ToDomain(), cancellationToken);

            if (product is null)
                throw new InvalidOperationException($"Product with ID {request.ProductIdDto.Value} not found.");

            product.DeactivatePackagingVersions(request.packagingVersionId.ToDomain());

            var newRowVersion = await _productRepository.UpdateAsync(product, request.RowVersion, cancellationToken);

            return new DeactivatePackagingVersionResponse(
                product.ProductId.ToDto(),
                request.packagingVersionId,
                product.PackagingVersions.Single(
                    pv => pv.PackagingVersionId.Value == request.packagingVersionId.ToDomain().Value).Status,
                newRowVersion);
        }
    }
}
