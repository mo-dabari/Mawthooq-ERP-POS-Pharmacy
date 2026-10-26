using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.Exceptions;
using Mawthooq.Application.Common.Validation;
using Mawthooq.Application.Products.Queries.Common.Pagination;

namespace Mawthooq.Application.Products.Queries.UseCases.SearchProducts
{
    public static class SearchProductsQueryValidator
    {
        public static void Validate(SearchProductsQuery query)
        {
            List<string> errors = new();

            StringValidator.ValidateAndCollect(query.SearchText, nameof(query.SearchText), errors);

            RangeValidator.ValidateRange(query.MaxResults, 1, PagingDefaults.MaxResultsLimit, nameof(query.MaxResults), errors);

            if (errors.Count > 0) throw new ValidationException(errors);
        }
    }
}
