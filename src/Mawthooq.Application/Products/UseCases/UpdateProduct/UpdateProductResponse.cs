using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Application.Products.UseCases.UpdateProduct
{
    public record class UpdateProductResponse(int ProductId, string Name, bool IsActive)
    {
    }
}
