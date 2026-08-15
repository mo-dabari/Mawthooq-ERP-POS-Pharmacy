using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Queries.Common.Enums;
using Mawthooq.Application.Products.Queries.Common.Pagination;
using Mawthooq.Application.Products.Queries.UseCases.GetProducts.Enums;

namespace Mawthooq.Application.Products.Queries.UseCases.GetProducts
{
    public sealed record GetProductsQuery
    (
        string? SearchText = null,

        Guid? FilterByCategoryId = null,
        
        Guid? FilterByManufacturerId = null,

        ProductStatusFilter FilterByStatus = ProductStatusFilter.ActiveOnly,

        ProductSortBy SortBy = ProductSortBy.TradeName,

        SortDirection SortDirection = SortDirection.Asc,

        int PageNumber = PagingDefaults.FirstPage,

        int PageSize = PagingDefaults.DefaultPageSize
    );
}
