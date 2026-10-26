using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Categories.Commands.DTOs;
using Mawthooq.Application.Common.DTOs;
using Mawthooq.Application.Products.Commands.DTOs;
using Mawthooq.Domain.Products.Enums;
using Mawthooq.Domain.SharedKernel.Enums;

namespace Mawthooq.Application.Products.Commands.UseCases.UpdateProductInformation
{
    public record class UpdateProductInformationRequest(
        ProductIdDto ProductId,
        string TradeName,
        CategoryIdDto CategoryId,
        ImageDto Image,
        bool CanBeReturnedIfOpened,
        int NearExpiryThresholdInDays,
        byte[] RowVersion);
}
