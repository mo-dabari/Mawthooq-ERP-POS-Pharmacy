using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Commands.Mappers;
using Mawthooq.Application.Products.Exceptions;
using Mawthooq.Domain.Products;
using Mawthooq.Domain.Products.Entities;
using Mawthooq.Domain.Products.Services;
using Mawthooq.Domain.Products.ValueObjects.Packaging;

namespace Mawthooq.Application.Products.Commands.UseCases.AddPackagingVersion
{
    public sealed class AddPackagingVersionUseCase
    {
        private readonly IProductRepository _productRepository;
        private readonly ProductFactory _productFactory;

        public AddPackagingVersionUseCase(
            IProductRepository productRepository,
            ProductFactory productFactory)
        {
            ArgumentNullException.ThrowIfNull(productRepository, nameof(productRepository));
            ArgumentNullException.ThrowIfNull(productFactory, nameof(productFactory));

            _productRepository = productRepository;
            _productFactory = productFactory;
        }

        public async Task<AddPackagingVersionResponse> ExecuteAsync(
            AddPackagingVersionRequest request,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

             AddPackagingVersionValidator.Validate(request);

            var (product, _) =
                await _productRepository.GetByIdAsync(
                    request.ProductId.ToDomain(),
                    cancellationToken);

            if (product is null)
                throw new ProductNotFoundException(request.ProductId.Value);

            ProductPackaging packaging =
                request.Packaging.ToDomain(request.Template);

            await _productFactory.AddPackagingVersionAsync(product, packaging, cancellationToken);

            byte[] newRowVersion =
                await _productRepository.UpdateAsync(
                    product,
                    request.RowVersion,
                    cancellationToken);

            PackagingVersion packagingVersion =
                product.PackagingVersions
                    .Single(p => p.Version ==
                                 product.PackagingVersions.Max(v => v.Version));

            return new AddPackagingVersionResponse(
                product.ProductId.ToDto(),
                packagingVersion.ToDto(),
                newRowVersion);
        }
    }
}
