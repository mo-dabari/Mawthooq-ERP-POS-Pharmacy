using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Infrastructure.AdoNet.DependencyInjection.Categories;
using Mawthooq.Infrastructure.AdoNet.DependencyInjection.Products;
using Mawthooq.Infrastructure.AdoNet.Persistence.Connections.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mawthooq.Infrastructure.AdoNet.DependencyInjection
{
    public static class InfrastructureServicesRegistration
    {
        public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
        {
            services.AddDatabaseContext(configuration);
            services.AddProductsPersistence();
            services.AddCategoriesPersistence();

            return services;
        }
    }
}
