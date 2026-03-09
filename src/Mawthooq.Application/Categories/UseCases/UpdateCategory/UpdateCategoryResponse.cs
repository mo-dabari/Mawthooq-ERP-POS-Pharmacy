using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Application.Categories.UseCases.UpdateCategory
{
    public record class UpdateCategoryResponse (int CategoryId, string Name, string Description, bool IsActive)
    {

    }
}
