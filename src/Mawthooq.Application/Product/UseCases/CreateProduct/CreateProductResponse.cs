using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Application.Product.UseCases.CreateProduct
{
    public record class CreateProductResponse (int ProductId, string CategoryName, string Name, string SKU)
    {
    }
}
