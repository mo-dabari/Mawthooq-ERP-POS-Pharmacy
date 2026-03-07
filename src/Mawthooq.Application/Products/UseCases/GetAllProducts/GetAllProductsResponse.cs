using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Application.Products.UseCases.GetAllProducts
{
    public record class GetAllProductsResponse(int ProductId, string Name, bool IsActive, string? Barcode)
    {

    }
}
