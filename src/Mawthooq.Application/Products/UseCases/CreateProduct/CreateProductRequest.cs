using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Application.Products.UseCases.CreateProduct
{
    public record class CreateProductRequest(int CategoryId, string Name, string SKU, string ImageUrl, string Barcode)
    {
    }
}
