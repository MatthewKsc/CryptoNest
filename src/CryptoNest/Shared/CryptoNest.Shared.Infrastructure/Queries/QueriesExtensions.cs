using System.Collections.Generic;
using System.Reflection;
using CryptoNest.Shared.Abstractions.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoNest.Shared.Infrastructure.Queries;

internal static class QueriesExtensions
{
    public static IServiceCollection AddQueries(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        services.AddSingleton<IQueryDispatcher, QueryDispatcher>();
        services.Scan(s => s.FromAssemblies(assemblies)
            .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }
}