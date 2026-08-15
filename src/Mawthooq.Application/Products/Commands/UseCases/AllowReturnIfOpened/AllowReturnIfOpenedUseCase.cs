using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Commands.Mappers;
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

        public AllowReturnIfOpenedResponse Execute(AllowReturnIfOpenedRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            var product = _productRepository.GetById(request.ProductIdDto.ToDomain());

            if (product is null)
                throw new InvalidOperationException($"Product with ID {request.ProductIdDto.Value} not found.");

            product.AllowReturnIfOpened();
            
            var newRowVersion =_productRepository.Update(product,request.RowVersion);

            return new AllowReturnIfOpenedResponse(
                product.ProductId.ToDto(),
                product.TradeName,
                product.CanBeReturnedIfOpened,
                newRowVersion);

        }
    }
}
