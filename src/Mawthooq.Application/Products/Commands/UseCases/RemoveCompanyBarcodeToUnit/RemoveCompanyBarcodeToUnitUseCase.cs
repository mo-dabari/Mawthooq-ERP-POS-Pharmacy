using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Commands.Mappers;
using Mawthooq.Application.Products.Exceptions;
using Mawthooq.Domain.Products;

namespace Mawthooq.Application.Products.Commands.UseCases.RemoveCompanyBarcodeToUnit
{
    public class RemoveCompanyBarcodeToUnitUseCase
    {
        private readonly IProductRepository _productRepository;

        public RemoveCompanyBarcodeToUnitUseCase(IProductRepository productRepository)
        {
            ArgumentNullException.ThrowIfNull(productRepository, nameof(productRepository));

            _productRepository = productRepository;
        }

        public async Task<RemoveCompanyBarcodeToUnitResponse> ExecuteAsync(RemoveCompanyBarcodeToUnitRequest request, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            RemoveCompanyBarcodeToUnitValidator.Validate(request);

            var (product, _) = await _productRepository.GetByIdAsync(request.ProductIdDto.ToDomain(), cancellationToken);

            if (product is null)
                throw new ProductNotFoundException(request.ProductIdDto.Value);

            if (!await _productRepository.ExistsByBarcodeAsync(request.BarcodeDto.ToDomain(), cancellationToken))
                throw new BarcodeNotFoundException(request.BarcodeDto.Value);

            product.RemoveCompanyBarcodeFromVersion(
                request.PackagingVersionIdDto.ToDomain(),
                request.UnitName,
                request.BarcodeDto.ToDomain());

            byte[] newRowVersion = await _productRepository.UpdateAsync(product, request.RowVersion, cancellationToken);

            var unit = product.PackagingVersions
                .Single(pv => pv.PackagingVersionId.Value == request.PackagingVersionIdDto.Value)
                .Units.Single(u => u.Unit == request.UnitName);

            return new RemoveCompanyBarcodeToUnitResponse(
                product.ProductId.ToDto(),
                request.UnitName,
                unit.Barcodes
                .Select(ub => ub.ToDto())
                .ToList(),
                newRowVersion);
        }
    }
}
