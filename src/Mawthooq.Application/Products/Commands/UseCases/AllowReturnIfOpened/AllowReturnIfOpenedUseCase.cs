using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Commands.Mappers;
using Mawthooq.Application.Products.Exceptions;
using Mawthooq.Domain.Products;

namespace Mawthooq.Application.Products.Commands.UseCases.AllowReturnIfOpened
{
    public class AllowReturnIfOpenedUseCase
    {
        private readonly IProductRepository _productRepository;

        public AllowReturnIfOpenedUseCase(IProductRepository productRepository)
        {
            ArgumentNullException.ThrowIfNull(productRepository, nameof(productRepository));

            _productRepository = productRepository;
        }

        public async Task<AllowReturnIfOpenedResponse> ExecuteAsync(AllowReturnIfOpenedRequest request, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            AllowReturnIfOpenedValidator.Validate(request);

            var (product, _) = await _productRepository.GetByIdAsync(request.ProductIdDto.ToDomain(), cancellationToken);

            if (product is null)
                throw new ProductNotFoundException(request.ProductIdDto.Value);

            product.AllowReturnIfOpened();

            var newRowVersion = await _productRepository.UpdateAsync(product, request.RowVersion, cancellationToken);

            return new AllowReturnIfOpenedResponse(
                product.ProductId.ToDto(),
                product.TradeName,
                product.CanBeReturnedIfOpened,
                newRowVersion);


        }
    }
}
