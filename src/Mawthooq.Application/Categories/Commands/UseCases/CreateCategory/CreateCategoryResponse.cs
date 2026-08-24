using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Categories.Commands.DTOs;

namespace Mawthooq.Application.Categories.Commands.UseCases.CreateCategory
{
    public record class CreateCategoryResponse(
        CategoryIdDto CategoryId,
        string Name,
        string Description,
        bool Status,
        byte[] RowVersion);
}
