using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Commands.Mappers;
using Mawthooq.Application.Products.Exceptions;
using Mawthooq.Domain.Products;

namespace Mawthooq.Application.Products.Commands.UseCases.AddingNewBarcodeToUnit
{
    public class AddingNewBarcodeToUnitUseCase
    {
        private readonly IProductRepository _productRepository;

        public AddingNewBarcodeToUnitUseCase(IProductRepository productRepository)
        {
            ArgumentNullException.ThrowIfNull(productRepository, nameof(productRepository));

            _productRepository = productRepository;
        }

        public async Task<AddingNewBarcodeToUnitResponse> ExecuteAsync(AddingNewBarcodeToUnitRequest request, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            AddingNewBarcodeToUnitValidator.Validate(request);

            var (product, _) = await _productRepository.GetByIdAsync(request.ProductId.ToDomain(), cancellationToken);

            if (product is null)
                throw new ProductNotFoundException(request.ProductId.Value);

            if (await _productRepository.ExistsByBarcodeAsync(request.Barcode.ToDomain(), cancellationToken))
                throw new DuplicateBarcodeException(request.Barcode.Value);

            product.AddCompanyBarcodeToVersion(
                request.PackagingVersionId.ToDomain(),
                request.Unit,
                request.Barcode.ToDomain());

            byte[] newRowVersion = await _productRepository.UpdateAsync(product, request.RowVersion, cancellationToken);

            var packagingVersion = product.PackagingVersions
                .Single(pv => pv.PackagingVersionId.Value == request.PackagingVersionId.Value);

            var unit = packagingVersion
                .Units.Single(u => u.Unit == request.Unit);

            return new AddingNewBarcodeToUnitResponse(
                product.ProductId.ToDto(),
                packagingVersion.PackagingVersionId.ToDto(),
                unit.Unit,
                unit.Barcodes
                .Select(ub => ub.ToDto())
                .ToList(),
                newRowVersion);
        }
    }
}
