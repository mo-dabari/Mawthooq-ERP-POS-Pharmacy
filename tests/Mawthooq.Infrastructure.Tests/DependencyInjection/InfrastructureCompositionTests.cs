using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mawthooq.Application.DependencyInjection;
using Mawthooq.Application.Products.Queries.Common.Enums;
using Mawthooq.Application.Products.Queries.UseCases.SearchProducts;
using Mawthooq.Infrastructure.AdoNet.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mawthooq.Infrastructure.Tests.DependencyInjection
{
    public class InfrastructureCompositionTests
    {
        private static IConfiguration Configuration(string? connectionString) =>
            new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                { ["ConnectionStrings:DefaultConnection"] = connectionString })
                .Build();

        [Fact]
        public async Task Should_ResolveEveryUseCaseAndHandler_When_ApplicationAndInfrastructureAreRegistered()
        {
            var services = new ServiceCollection();
            services.AddApplication();
            services.AddInfrastructure(Configuration("Server=localhost;Database=MawthooqPos;Trusted_Connection=True;TrustServerCertificate=True"));

            using var provider = services.BuildServiceProvider(
                new ServiceProviderOptions { ValidateScopes = true, ValidateOnBuild = true });
            await using var scope = provider.CreateAsyncScope();

            var types = typeof(ApplicationServiceCollectionExtensions).Assembly.GetTypes()
                .Where(t => t is { IsClass: true, IsAbstract: false, IsPublic: true }
                            && (t.Name.EndsWith("UseCase") || t.Name.EndsWith("Handler")));

            foreach (Type type in types)
                scope.ServiceProvider.GetRequiredService(type).Should().NotBeNull(type.Name);
        }

        [Fact]
        public void Should_FailAtStartup_When_ConnectionStringIsMissing()
        {
            var services = new ServiceCollection();

            Action act = () => services.AddInfrastructure(Configuration(null));

            act.Should().Throw<InvalidOperationException>().WithMessage("*DefaultConnection*");
        }

        [Fact]
        public async Task Should_HaveExactlyOneSearchRepository_ForEveryProductSearchBy()
        {
            var services = new ServiceCollection();
            services.AddApplication();
            services.AddInfrastructure(Configuration("Server=localhost;Database=MawthooqPos;Trusted_Connection=True;TrustServerCertificate=True"));

            using var provider = services.BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true, ValidateOnBuild = true });
            await using  var scope = provider.CreateAsyncScope();

            var registered = scope.ServiceProvider.GetServices<ISearchProductsQueryRepository>()
                .Select(repository => repository.SearchBy)
                .ToList();

            registered.Should().BeEquivalentTo(Enum.GetValues<ProductSearchBy>());
        }
    }
}
