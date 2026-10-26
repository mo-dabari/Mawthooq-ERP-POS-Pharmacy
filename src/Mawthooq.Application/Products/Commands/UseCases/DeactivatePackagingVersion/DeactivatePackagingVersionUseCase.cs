using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Commands.Mappers;
using Mawthooq.Application.Products.Exceptions;
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

            DeactivatePackagingVersionValidator.Validate(request);

            var (product, _) = await _productRepository.GetByIdAsync(request.ProductId.ToDomain(), cancellationToken);

            if (product is null)
                throw new ProductNotFoundException(request.ProductId.Value);

            product.DeactivatePackagingVersions(request.PackagingVersionId.ToDomain());

            var newRowVersion = await _productRepository.UpdateAsync(product, request.RowVersion, cancellationToken);

            return new DeactivatePackagingVersionResponse(
                product.ProductId.ToDto(),
                request.PackagingVersionId,
                product.PackagingVersions.Single(
                    pv => pv.PackagingVersionId.Value == request.PackagingVersionId.ToDomain().Value).Status,
                newRowVersion);
        }
    }
}
