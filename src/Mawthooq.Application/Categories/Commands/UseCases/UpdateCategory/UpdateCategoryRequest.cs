using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Categories.Commands.DTOs;
using Mawthooq.Domain.SharedKernel.Enums;

namespace Mawthooq.Application.Categories.Commands.UseCases.UpdateCategory
{
    public record class UpdateCategoryRequest (
        CategoryIdDto CategoryId,
        string Name,
        string Description,
        byte[] RowVersion);
}
