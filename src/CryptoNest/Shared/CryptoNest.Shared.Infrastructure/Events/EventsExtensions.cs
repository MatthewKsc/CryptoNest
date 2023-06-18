using System.Collections.Generic;
using System.Reflection;
using CryptoNest.Shared.Abstractions.Events;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoNest.Shared.Infrastructure.Events;

internal static class EventsExtensions
{
    public static IServiceCollection AddEvents(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        services.AddSingleton<IIntegrationEventDispatcher, IntegrationEventDispatcher>();
        
        services.Scan(s => s.FromAssemblies(assemblies)
            .AddClasses(c => c.AssignableTo(typeof(IIntegrationEventHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        
        return services;
    }
}