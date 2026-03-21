using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Returns.DTOs;
using Mawthooq.Domain.Returns.Entity;

namespace Mawthooq.Application.Returns.Mappers
{
    public static class ReturnedProductMapper
    {
        public static ReturnLine ToDomain(this ReturnedProductDto returnedProductDto)
        {
            ArgumentNullException.ThrowIfNull(returnedProductDto, nameof(returnedProductDto));

            return new ReturnLine(
                returnedProductDto.ProductId,
                returnedProductDto.Quantity,
                returnedProductDto.Reason,
                returnedProductDto.Status,
                returnedProductDto.ExpiryDate);
        }

        public static ReturnedProductDto ToDto(this ReturnLine returnLine)
        {
            ArgumentNullException.ThrowIfNull(returnLine, nameof(returnLine));

            return new ReturnedProductDto(
                returnLine.ProductId,
                returnLine.Quantity,
                returnLine.Reason,
                returnLine.Status,
                returnLine.ExpiryDate);
        }
    }
}
