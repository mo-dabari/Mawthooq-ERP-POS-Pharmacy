using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Application.Categories.UseCases.GetCategoryByID
{
    public record class GetCategoryByIdResponse(int CategoryId, string Name, string Description, bool IsActive)
    {

    }
}
