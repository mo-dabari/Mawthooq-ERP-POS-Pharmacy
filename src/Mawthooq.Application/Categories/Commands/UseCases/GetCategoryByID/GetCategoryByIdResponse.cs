using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Categories.Commands.DTOs;
using Mawthooq.Domain.SharedKernel.Enums;

namespace Mawthooq.Application.Categories.UseCases.GetCategoryByID
{
    public record class GetCategoryByIdResponse(
        CategoryIdDto CategoryIdDto,
        string Name,
        string Description,
        Status Status);
}
