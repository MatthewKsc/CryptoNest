using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CryptoNest.Shared.Abstractions.Events;
using CryptoNest.Shared.Abstractions.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoNest.Shared.Infrastructure.Modules;

public static class ModulesExtensions
{
    internal static WebApplicationBuilder ConfigureModules(this WebApplicationBuilder builder)
    {
        foreach (string setting in GetSettings("*", builder))
        {
            builder.Configuration.AddJsonFile(setting);
        }
        
        foreach (string setting in GetSettings($"*.{builder.Environment.EnvironmentName}", builder))
        {
            builder.Configuration.AddJsonFile(setting);
        }

        return builder;
    }

    internal static IServiceCollection AddModuleRequests(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        services.AddModuleRegistry(assemblies);
        services.AddSingleton<IModuleClient, ModuleClient>();

        return services;
    }

    private static IServiceCollection AddModuleRegistry(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        ModuleRegistry moduleRegistry = new();

        Type[] types = assemblies.SelectMany(x => x.GetTypes())
            .ToArray();
        Type[] eventTypes = types.Where(type => type.IsClass && typeof(IIntegrationEvent).IsAssignableFrom(type))
            .ToArray();

        services.AddSingleton<IModuleRegistry>(serviceProvider =>
        {
            var eventDispatcher = serviceProvider.GetRequiredService<IIntegrationEventDispatcher>();
            Type eventDispatcherType = eventDispatcher.GetType();

            foreach (Type eventType in eventTypes)
            {
                moduleRegistry.AddBroadcastAction(eventType, @event => 
                    (Task) eventDispatcherType.GetMethod(nameof(eventDispatcher.PublishAsync))
                        ?.MakeGenericMethod(eventType)
                        .Invoke(eventDispatcher, new []{ @event })
                    );
            }

            return moduleRegistry;
        });
        
        return services;
    }
    
    private static IEnumerable<string> GetSettings(string pattern, WebApplicationBuilder  builder) =>
        Directory.EnumerateFiles(builder.Environment.ContentRootPath, $"module.{pattern}.json", SearchOption.AllDirectories);
}