using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Application.Products.Queries.DTOs
{
    public record class ProductSelectionDto(
        Guid ProductId,
        string DisplayName,
        string Manufacturer,
        string Category
    );

}
