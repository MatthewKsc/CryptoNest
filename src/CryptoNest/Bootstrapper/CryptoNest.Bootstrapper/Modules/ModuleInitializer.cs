using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CryptoNest.Shared.Abstractions.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

namespace CryptoNest.Bootstrapper.Modules;

internal static class ModuleInitializer
{
    private static IReadOnlyCollection<IModule> modules;

    public static WebApplicationBuilder RegisterModules(this WebApplicationBuilder builder, IEnumerable<Assembly> assemblies)
    {
        modules = ModuleLoader.LoadModules(assemblies);

        foreach (IModule module in modules)
        {
            module.Register(builder.Services, builder.Configuration);
        }

        return builder;
    }

    public static WebApplication UseModules(this WebApplication app, ILogger logger)
    {
        logger.LogInformation($"Modules: {string.Join(", ", modules.Select(x => x.Name))}");
        
        foreach (IModule module in modules)
        {
            module.Use(app);
        }

        return app;
    }
}