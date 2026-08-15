using System;
using System.Collections.Generic;
using System.Linq;
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

        public DeactivatePackagingVersionResponse Execute(DeactivatePackagingVersionRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            var product = _productRepository.GetById(request.ProductIdDto.ToDomain());

            if (product is null)
                throw new InvalidOperationException($"Product with ID {request.ProductIdDto.Value} not found.");

            product.DeactivatePackagingVersions(request.packagingVersionId.ToDomain());

            var newRowVersion = _productRepository.Update(product, request.RowVersion);

            return new DeactivatePackagingVersionResponse(
                product.ProductId.ToDto(),
                request.packagingVersionId,
                product.PackagingVersions.Single(pv => pv.PackagingVersionId.Value == request.packagingVersionId.ToDomain().Value).Status,
                newRowVersion);
        }
    }
}
