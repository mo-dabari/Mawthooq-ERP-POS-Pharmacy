using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.Exceptions;
using Mawthooq.Application.Common.Validation;
using Mawthooq.Application.Products.Queries.Common.Pagination;

namespace Mawthooq.Application.Products.Queries.UseCases.GetProducts
{
    public static class GetProductsQueryValidator
    {
        public static void Validate(GetProductsQuery query)
        {
            List<string> errors = new();

            NumberValidator.ValidatePositive(query.PageNumber, nameof(query.PageNumber), errors);
            RangeValidator.ValidateRange(query.PageSize, 1, PagingDefaults.MaxPageSize, nameof(query.PageSize), errors);
            EnumValidator.ValidateAndCollect(query.FilterByStatus, nameof(query.FilterByStatus), errors);
            EnumValidator.ValidateAndCollect(query.SortBy, nameof(query.SortBy), errors);
            EnumValidator.ValidateAndCollect(query.SortDirection, nameof(query.SortDirection), errors);

            if (errors.Count > 0)
                throw new ValidationException(errors);
        }
    }
}
