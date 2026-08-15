using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Categories.Enums;

namespace Mawthooq.Application.Categories.Commands.UseCases.CreateCategory
{
    public record class CreateCategoryRequest(ProductCategory Name, string Description)
    {

    }
}
