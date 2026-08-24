using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Categories.Commands.DTOs;

namespace Mawthooq.Application.Categories.Commands.UseCases.UpdateCategory
{
    public record class UpdateCategoryResponse (
        CategoryIdDto CategoryId,
        string Name,
        string Description,
        byte[] NewRowVersion);
}
