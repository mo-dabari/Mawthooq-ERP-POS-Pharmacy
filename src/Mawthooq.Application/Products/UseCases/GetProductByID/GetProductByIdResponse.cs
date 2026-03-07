using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Application.Products.UseCases.GetProductByID
{
    public record class GetProductByIdResponse (int ProductId, string ProductName, string CategoryName, string SKU, bool IsActive, string? Barcode)
    {
    }
}
