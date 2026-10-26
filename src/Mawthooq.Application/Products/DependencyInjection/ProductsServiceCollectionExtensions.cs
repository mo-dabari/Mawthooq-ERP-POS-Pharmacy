using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Commands.UseCases.ActivatePackagingVersion;
using Mawthooq.Application.Products.Commands.UseCases.ActivateProduct;
using Mawthooq.Application.Products.Commands.UseCases.AddingNewBarcodeToUnit;
using Mawthooq.Application.Products.Commands.UseCases.AddPackagingVersion;
using Mawthooq.Application.Products.Commands.UseCases.AllowReturnIfOpened;
using Mawthooq.Application.Products.Commands.UseCases.ChangeNearExpiryThreshold;
using Mawthooq.Application.Products.Commands.UseCases.ChangeProductCategory;
using Mawthooq.Application.Products.Commands.UseCases.CreateProduct;
using Mawthooq.Application.Products.Commands.UseCases.DeactivatePackagingVersion;
using Mawthooq.Application.Products.Commands.UseCases.DisallowReturnIfOpened;
using Mawthooq.Application.Products.Commands.UseCases.DiscontinueProduct;
using Mawthooq.Application.Products.Commands.UseCases.RemoveCompanyBarcodeToUnit;
using Mawthooq.Application.Products.Commands.UseCases.UpdateProductInformation;
using Mawthooq.Application.Products.Queries.UseCases.GetProductDetails;
using Mawthooq.Application.Products.Queries.UseCases.GetProductLookup;
using Mawthooq.Application.Products.Queries.UseCases.GetProducts;
using Mawthooq.Application.Products.Queries.UseCases.ScanProductBarcode;
using Mawthooq.Application.Products.Queries.UseCases.SearchProducts;
using Mawthooq.Domain.Products.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Mawthooq.Application.Products.DependencyInjection
{
    internal static class ProductsServiceCollectionExtensions
    {
        internal static IServiceCollection AddProductsFeature(this IServiceCollection services)
        {
            services.AddScoped<ProductFactory>();

            services.AddScoped<CreateProductUseCase>();
            services.AddScoped<UpdateProductInformationUseCase>();
            services.AddScoped<ChangeProductCategoryUseCase>();
            services.AddScoped<ChangeNearExpiryThresholdUseCase>();
            services.AddScoped<ActivateProductUseCase>();
            services.AddScoped<DiscontinueProductUseCase>();
            services.AddScoped<AllowReturnIfOpenedUseCase>();
            services.AddScoped<DisallowReturnIfOpenedUseCase>();
            services.AddScoped<AddPackagingVersionUseCase>();
            services.AddScoped<ActivatePackagingVersionUseCase>();
            services.AddScoped<DeactivatePackagingVersionUseCase>();
            services.AddScoped<AddingNewBarcodeToUnitUseCase>();
            services.AddScoped<RemoveCompanyBarcodeToUnitUseCase>();

            services.AddScoped<GetProductDetailsHandler>();
            services.AddScoped<GetProductListHandler>();
            services.AddScoped<GetProductLookupHandler>();
            services.AddScoped<ScanProductBarcodeHandler>();
            services.AddScoped<SearchProductsHandler>();

            return services;
        }
    }
}
