using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Application.Products.Queries.DTOs
{
    public record class ProductLookupDto(
        Guid ProductId,
        string DisplayName,
        int InternalBarcod
    );
}
