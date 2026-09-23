using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Commands.Mappers;
using Mawthooq.Application.Products.Exceptions;
using Mawthooq.Domain.Products;

namespace Mawthooq.Application.Products.Commands.UseCases.ActivatePackagingVersion
{
    public class ActivatePackagingVersionUseCase
    {
        private readonly IProductRepository _productRepository;

        public ActivatePackagingVersionUseCase(IProductRepository productRepository)
        {
            ArgumentNullException.ThrowIfNull(productRepository, nameof(productRepository));

            _productRepository = productRepository;
        }
        public async Task<ActivatePackagingVersionResponse> ExecuteAsync(ActivatePackagingVersionRequest request, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            var (product, _) = await _productRepository.GetByIdAsync(request.ProductIdDto.ToDomain(), cancellationToken);

            if (product is null)
                throw new ProductNotFoundException(request.ProductIdDto.Value);

            product.ActivatePackagingVersions(request.PackagingVersionId.ToDomain());

            var newRowVersion = await _productRepository.UpdateAsync(product, request.RowVersion, cancellationToken);

            return new ActivatePackagingVersionResponse(
                product.ProductId.ToDto(),
                request.PackagingVersionId,
                product.PackagingVersions.Single(
                    pv => pv.PackagingVersionId.Value == request.PackagingVersionId.ToDomain().Value).Status,
                newRowVersion);
        }
    }
}
