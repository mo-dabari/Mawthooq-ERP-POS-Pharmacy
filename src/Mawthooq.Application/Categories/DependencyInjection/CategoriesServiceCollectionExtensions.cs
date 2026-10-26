using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Categories.Commands.UseCases.CreateCategory;
using Mawthooq.Application.Categories.Commands.UseCases.GetCategoryByID;
using Mawthooq.Application.Categories.Commands.UseCases.UpdateCategory;
using Microsoft.Extensions.DependencyInjection;

namespace Mawthooq.Application.Categories.DependencyInjection
{
    internal static class CategoriesServiceCollectionExtensions
    {
        internal static IServiceCollection AddCategoriesFeature(this IServiceCollection services)
        {
            services.AddScoped<CreateCategoryUseCase>();
            services.AddScoped<GetCategoryByIdUseCase>();
            services.AddScoped<UpdateCategoryUseCase>();

            return services;
        }
    }
}
