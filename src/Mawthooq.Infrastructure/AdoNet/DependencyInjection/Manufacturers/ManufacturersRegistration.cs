using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Manufacturers;
using Mawthooq.Infrastructure.AdoNet.Repositories.Manufacturers;
using Microsoft.Extensions.DependencyInjection;

namespace Mawthooq.Infrastructure.AdoNet.DependencyInjection.Manufacturers
{
    internal static class ManufacturersRegistration
    {
        internal static IServiceCollection AddManufacturersPersistence(this IServiceCollection services)
        {
            services.AddScoped<IManufacturerRepository, ManufacturerRepository>();

            return services;
        }
    }
}
