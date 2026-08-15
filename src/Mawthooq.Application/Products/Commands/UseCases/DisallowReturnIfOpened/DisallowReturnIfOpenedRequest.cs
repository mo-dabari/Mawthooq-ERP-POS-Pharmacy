using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Commands.DTOs;
using Mawthooq.Domain.Products.ValueObjects;

namespace Mawthooq.Application.Products.Commands.UseCases.DisallowReturnIfOpened
{
    public record class DisallowReturnIfOpenedRequest(
        ProductIdDto ProductIdDto,
        byte[] RowVersion);
}
