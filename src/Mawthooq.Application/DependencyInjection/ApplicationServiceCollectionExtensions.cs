using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Categories.DependencyInjection;
using Mawthooq.Application.Products.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Mawthooq.Application.DependencyInjection
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddProductsFeature();
            services.AddCategoriesFeature();

            return services;
        }
    }
}
