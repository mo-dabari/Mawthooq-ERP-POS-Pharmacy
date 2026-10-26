using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mawthooq.Application.DependencyInjection;
using Mawthooq.Application.Products.Queries.UseCases.GetProductDetails;
using Mawthooq.Application.Products.Queries.UseCases.GetProductLookup;
using Mawthooq.Application.Products.Queries.UseCases.GetProducts;
using Mawthooq.Application.Products.Queries.UseCases.ScanProductBarcode;
using Mawthooq.Application.Products.Queries.UseCases.SearchProducts;
using Mawthooq.Domain.Categories;
using Mawthooq.Domain.Products;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Mawthooq.Application.Tests.DependencyInjection
{
    public class ApplicationServiceCollectionExtensionsTests
{
    [Fact]
    public void Should_ResolveEveryUseCaseAndHandler_When_ApplicationIsRegistered()
    {
        var services = new ServiceCollection();
        services.AddApplication();

        services.AddScoped(_ => Mock.Of<IProductRepository>());
        services.AddScoped(_ => Mock.Of<ICategoryRepository>());
        services.AddScoped(_ => Mock.Of<IInternalBarcodeSequenceGenerator>());
        services.AddScoped(_ => Mock.Of<IGetProductDetailsQueryRepository>());
        services.AddScoped(_ => Mock.Of<IGetProductsQueryRepository>());
        services.AddScoped(_ => Mock.Of<IGetProductLookupQueryRepository>());
        services.AddScoped(_ => Mock.Of<IScanProductBarcodeQueryRepository>());
        services.AddScoped(_ => Mock.Of<ISearchProductsQueryRepository>());

        using var provider = services.BuildServiceProvider(
            new ServiceProviderOptions { ValidateScopes = true, ValidateOnBuild = true });
        using var scope = provider.CreateScope();

        var types = typeof(ApplicationServiceCollectionExtensions).Assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false, IsPublic: true }
                        && (t.Name.EndsWith("UseCase") || t.Name.EndsWith("Handler")))
            .ToList();

        types.Should().HaveCount(21);
        
        foreach (Type type in types)
            scope.ServiceProvider.GetRequiredService(type).Should().NotBeNull(type.Name);
    }
}
}
