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

            var (product, _) = await _productRepository.GetByIdAsync(request.ProductIdDto.ToDomain(), cancellationToken);

            if (product is null)
                throw new ProductNotFoundException(request.ProductIdDto.Value);

            if (await _productRepository.ExistsByBarcodeAsync(request.BarcodeDto.ToDomain(), cancellationToken))
                throw new DuplicateBarcodeException(request.BarcodeDto.Value);

            product.AddCompanyBarcodeToVersion(
                request.PackagingVersionIdDto.ToDomain(),
                request.UnitName,
                request.BarcodeDto.ToDomain());

            byte[] newRowVersion = await _productRepository.UpdateAsync(product, request.RowVersion, cancellationToken);

            var unit = product.PackagingVersions
                .Single(pv => pv.PackagingVersionId.Value == request.PackagingVersionIdDto.Value)
                .Units.Single(u => u.Unit == request.UnitName);

            return new AddingNewBarcodeToUnitResponse(
                product.ProductId.ToDto(),
                request.UnitName,
                unit.Barcodes
                .Select(ub => ub.ToDto())
                .ToList(),
                newRowVersion);
        }
    }
}
