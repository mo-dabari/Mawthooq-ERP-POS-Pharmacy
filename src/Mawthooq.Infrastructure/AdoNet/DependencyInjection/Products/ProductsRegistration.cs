using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Queries.UseCases.GetProductDetails;
using Mawthooq.Application.Products.Queries.UseCases.GetProductLookup;
using Mawthooq.Application.Products.Queries.UseCases.GetProducts;
using Mawthooq.Application.Products.Queries.UseCases.ScanProductBarcode;
using Mawthooq.Application.Products.Queries.UseCases.SearchProducts;
using Mawthooq.Domain.Products;
using Mawthooq.Infrastructure.AdoNet.Queries.Products.GetProductDetails;
using Mawthooq.Infrastructure.AdoNet.Queries.Products.GetProductLookup;
using Mawthooq.Infrastructure.AdoNet.Queries.Products.GetProducts;
using Mawthooq.Infrastructure.AdoNet.Queries.Products.ScanProductBarcode;
using Mawthooq.Infrastructure.AdoNet.Queries.Products.SearchProducts;
using Mawthooq.Infrastructure.AdoNet.Repositories.Products;
using Mawthooq.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Mawthooq.Infrastructure.AdoNet.DependencyInjection.Products
{
    internal static class ProductsRegistration
    {
        internal static IServiceCollection AddProductsPersistence(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IInternalBarcodeSequenceGenerator, SqlInternalBarcodeSequenceGenerator>();
            services.AddScoped<IGetProductsQueryRepository, GetProductsQueryRepository>();
            services.AddScoped<IGetProductLookupQueryRepository, GetProductLookupQueryRepository>();
            services.AddScoped<IScanProductBarcodeQueryRepository, ScanProductBarcodeQueryRepository>();

            services.AddScoped<IGetProductDetailsQueryRepository, GetProductDetailsQueryRepository>();
            
            services.AddScoped<ISearchProductsQueryRepository, SearchProductsByNameQueryRepository>();
            services.AddScoped<ISearchProductsQueryRepository, SearchProductsBySkuQueryRepository>();
            services.AddScoped<ISearchProductsQueryRepository, SearchProductsByBarcodeQueryRepository>();

            return services;
        }
    }
}
