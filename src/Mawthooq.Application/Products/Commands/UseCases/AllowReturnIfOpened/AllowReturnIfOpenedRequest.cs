using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Commands.DTOs;

namespace Mawthooq.Application.Products.Commands.UseCases.AllowReturnIfOpened
{
    public record class AllowReturnIfOpenedRequest (
        ProductIdDto ProductIdDto,
        byte[] RowVersion);
}
