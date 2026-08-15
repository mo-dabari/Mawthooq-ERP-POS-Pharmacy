using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Infrastructure.ADO.Persistence.Connections;
using Mawthooq.Infrastructure.AdoNet.Persistence.Abstractions;
using Mawthooq.Infrastructure.AdoNet.Persistence.Connections;
using Mawthooq.Infrastructure.AdoNet.Persistence.Transactions;

namespace Mawthooq.Infrastructure.AdoNet.DependencyInjection
{
    public static class InfrastructureServiceCollectionExtensions
    {

        public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
        {
            services.AddSingleton<IDbConnectionFactory, SqlConnectionFactory>();
            services.AddScoped<ITransactionManager, SqlTransactionManager>();

            return services;
        }

    }
}
