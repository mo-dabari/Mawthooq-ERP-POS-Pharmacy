using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Commands.DTOs;

namespace Mawthooq.Application.Products.Commands.UseCases.ChangeNearExpiryThreshold
{
    public record class ChangeNearExpiryThresholdRequest(
        ProductIdDto ProductId,
        int NewNearExpiryThresholdInDays,
        byte[] RowVersion);
}
