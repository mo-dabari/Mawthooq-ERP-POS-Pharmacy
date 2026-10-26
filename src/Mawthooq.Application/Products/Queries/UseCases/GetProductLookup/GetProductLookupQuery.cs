using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Queries.Common.Pagination;

namespace Mawthooq.Application.Products.Queries.UseCases.GetProductLookup
{
    public sealed record GetProductLookupQuery(string? SearchText = null, int MaxResults = PagingDefaults.DefaultMaxResults);

}
