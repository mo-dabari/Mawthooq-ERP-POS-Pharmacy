using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Application.Products.Queries.Common.Pagination
{
    public sealed record PagedResult<T>
    (
        IReadOnlyList<T> items,

        int TotalCount,

        int PageNumber,

        int PageSize
    );
}
