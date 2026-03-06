using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Application.Product.UseCases.UpdateProduct
{
    public record class UpdateProductResponse(int ProductId, string Name, bool IsActive)
    {
    }
}
