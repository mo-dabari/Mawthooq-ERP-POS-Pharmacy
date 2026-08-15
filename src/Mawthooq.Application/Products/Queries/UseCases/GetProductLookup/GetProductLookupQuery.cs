using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Application.Products.Queries.UseCases.GetProductLookup
{
    public sealed record GetProductLookupQuery(string? SearchText = null, int MaxResults = 20);

}
