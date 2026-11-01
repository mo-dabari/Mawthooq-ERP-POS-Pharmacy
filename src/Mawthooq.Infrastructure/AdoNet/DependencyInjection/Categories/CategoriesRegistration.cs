using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Categories;
using Mawthooq.Infrastructure.AdoNet.Repositories.Categories;
using Microsoft.Extensions.DependencyInjection;

namespace Mawthooq.Infrastructure.AdoNet.DependencyInjection.Categories
{
    internal static class CategoriesRegistration
    {
        internal static IServiceCollection AddCategoriesPersistence(this IServiceCollection services)
        {
            services.AddScoped<ICategoryRepository, CategoryRepository>();

            return services;
        }
    }
}
