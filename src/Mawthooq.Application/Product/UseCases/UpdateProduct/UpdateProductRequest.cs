using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Application.Product.UseCases.UpdateProduct
{
    public record class UpdateProductRequest(int ProductId, string Name, string SKU, string Barcode, string ImageUrl, bool IsActive, int CategoryId)
    {
    }
}
