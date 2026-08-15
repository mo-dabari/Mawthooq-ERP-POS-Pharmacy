using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Application.Products.Queries.UseCases.SearchProducts
{
    public sealed record SearchProductsQuery(string SearchText, int MaxResults = 20);
}
