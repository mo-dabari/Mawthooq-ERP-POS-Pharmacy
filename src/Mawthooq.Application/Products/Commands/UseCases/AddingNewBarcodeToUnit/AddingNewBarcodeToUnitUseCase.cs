using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Commands.DTOs;
using Mawthooq.Application.Products.Commands.Mappers;
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

        public AddingNewBarcodeToUnitResponse Execute(AddingNewBarcodeToUnitRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            var product = _productRepository.GetById(request.ProductIdDto.ToDomain());

            if (product is null)
                throw new InvalidOperationException($"Product with ID {request.ProductIdDto.Value} not found.");

            if (_productRepository.ExistsByBarcode(request.BarcodeDto.ToDomain()))
                throw new InvalidOperationException($"cannot adding barcode : {request.BarcodeDto.Value} duplicate");

            product.AddCompanyBarcodeToVersion(
                request.PackagingVersionIdDto.ToDomain(),
                request.UnitName,
                request.BarcodeDto.ToDomain());

            byte[] newRowVersion = _productRepository.Update(product, request.RowVersion);

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
