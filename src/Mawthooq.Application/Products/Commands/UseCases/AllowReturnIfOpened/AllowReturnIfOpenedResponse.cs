using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Commands.DTOs;

namespace Mawthooq.Application.Products.Commands.UseCases.AllowReturnIfOpened
{
    public record class AllowReturnIfOpenedResponse(
        ProductIdDto ProductId,
        string TradeName,
        bool CanBeReturnedIfOpened,
        byte[] RowVersion);
}
