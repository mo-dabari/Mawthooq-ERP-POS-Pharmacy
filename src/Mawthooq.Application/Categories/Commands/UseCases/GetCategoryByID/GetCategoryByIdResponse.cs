using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Categories.Commands.DTOs;
using Mawthooq.Domain.Categories.Enums;

namespace Mawthooq.Application.Categories.UseCases.GetCategoryByID
{
    public record class GetCategoryByIdResponse(
        CategoryIdDto CategoryId,
        string Name,
        string Description,
        bool Status);
}
