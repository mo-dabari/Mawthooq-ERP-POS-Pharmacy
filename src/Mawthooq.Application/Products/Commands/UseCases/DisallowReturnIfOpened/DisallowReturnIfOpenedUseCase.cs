using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Commands.Mappers;
using Mawthooq.Domain.Products;

namespace Mawthooq.Application.Products.Commands.UseCases.DisallowReturnIfOpened
{
    public class DisallowReturnIfOpenedUseCase
    {
        private readonly IProductRepository _productRepository;

        public DisallowReturnIfOpenedUseCase(IProductRepository productRepository)
        {
            ArgumentNullException.ThrowIfNull(productRepository, nameof(productRepository));
            _productRepository = productRepository;
        }

        public DisallowReturnIfOpenedResponse Execute(DisallowReturnIfOpenedRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            var product = _productRepository.GetById(request.ProductIdDto.ToDomain());

            if (product is null)
                throw new InvalidOperationException($"Product with ID {request.ProductIdDto.Value} not found.");

            product.DisallowReturnIfOpened();

            var newRowVersion = _productRepository.Update(product, request.RowVersion);

            return new DisallowReturnIfOpenedResponse(
                product.ProductId.ToDto(),
                product.CanBeReturnedIfOpened,
                newRowVersion);
        }
    }
}
