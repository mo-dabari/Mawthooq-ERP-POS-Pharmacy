using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Commands.Mappers;
using Mawthooq.Domain.Products;
using Mawthooq.Domain.Products.Enums;

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
        public ActivatePackagingVersionResponse Execute(ActivatePackagingVersionRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            var product = _productRepository.GetById(request.ProductIdDto.ToDomain());

            if (product is null)
                throw new InvalidOperationException($"Product with ID {request.ProductIdDto.Value} not found.");

            product.ActivatePackagingVersions(request.PackagingVersionId.ToDomain());

            var newRowVersion = _productRepository.Update(product, request.RowVersion);

            return new ActivatePackagingVersionResponse(
                product.ProductId.ToDto(),
                request.PackagingVersionId,
                product.PackagingVersions.Single(pv => pv.PackagingVersionId.Value == request.PackagingVersionId.ToDomain().Value).Status,
                newRowVersion);
        }
    }
}
