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

            var (product, _) = await _productRepository.GetByIdAsync(request.ProductId.ToDomain(), cancellationToken);

            if (product is null)
                throw new ProductNotFoundException(request.ProductId.Value);

            var barcode = request.Barcode.ToDomain();
            var existsInProduct = product.PackagingVersions
                .SelectMany(v => v.Units)
                .SelectMany(u => u.Barcodes)
                .Any(b => b.Value == barcode.Value);
            if (!existsInProduct)
                throw new BarcodeNotFoundException(request.Barcode.Value);

            product.RemoveCompanyBarcodeFromVersion(
                request.PackagingVersionId.ToDomain(),
                request.Unit,
                request.Barcode.ToDomain());

            byte[] newRowVersion = await _productRepository.UpdateAsync(product, request.RowVersion, cancellationToken);

            var PackagingVersion = product.PackagingVersions
                .Single(pv => pv.PackagingVersionId.Value == request.PackagingVersionId.Value);

            var unit = PackagingVersion
                .Units.Single(u => u.Unit == request.Unit);

            return new RemoveCompanyBarcodeToUnitResponse(
                product.ProductId.ToDto(),
                PackagingVersion.PackagingVersionId.ToDto(),
                request.Unit,
                unit.Barcodes
                .Select(ub => ub.ToDto())
                .ToList(),
                newRowVersion);
        }
    }
}
