using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Infrastructure.AdoNet.Persistence.Connections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mawthooq.Infrastructure.AdoNet.Persistence.Connections.DependencyInjection
{
    internal static class DatabaseRegistration
    {
        public static IServiceCollection AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is missing.");

            services.AddScoped<IDbConnectionProvider>(_ => new DbConnectionProvider(connectionString));

            return services;
        }
    }
}
