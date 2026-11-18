using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.LocalAgents;
using Mawthooq.Infrastructure.AdoNet.Repositories.LocalAgents;
using Microsoft.Extensions.DependencyInjection;

namespace Mawthooq.Infrastructure.AdoNet.DependencyInjection.LocalAgents
{
    internal static class LocalAgentsRegistration
    {
        internal static IServiceCollection AddLocalAgentsPersistence(this IServiceCollection services)
        {
            services.AddScoped<ILocalAgentRepository, LocalAgentRepository>();

            return services;
        }
    }
}
