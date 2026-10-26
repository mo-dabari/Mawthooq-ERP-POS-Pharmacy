using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.Exceptions;
using Mawthooq.Application.Common.Validation;

namespace Mawthooq.Application.Products.Queries.UseCases.GetProductDetails
{
    public static class GetProductDetailsQueryValidator
    {
        public static void Validate(GetProductDetailsQuery query)
        {
            List<string> errors = new();

            if (RequiredValidator.ValidateAndCollect(query.ProductId, nameof(query.ProductId), errors))
                GuidValidator.ValidateAndCollect(query.ProductId.Value, nameof(query.ProductId.Value), errors);

            if (errors.Count > 0) throw new ValidationException(errors);
        }
    }
}
