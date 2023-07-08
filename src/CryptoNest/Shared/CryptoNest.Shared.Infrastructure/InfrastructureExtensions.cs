using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using CryptoNest.Shared.Infrastructure.Api;
using CryptoNest.Shared.Infrastructure.CoinMarketCap;
using CryptoNest.Shared.Infrastructure.Events;
using CryptoNest.Shared.Infrastructure.Exceptions;
using CryptoNest.Shared.Infrastructure.Logger;
using CryptoNest.Shared.Infrastructure.Messaging;
using CryptoNest.Shared.Infrastructure.Modules;
using CryptoNest.Shared.Infrastructure.Queries;
using CryptoNest.Shared.Infrastructure.Services;
using CryptoNest.Shared.Infrastructure.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

[assembly:InternalsVisibleTo("CryptoNest.Bootstrapper")]
namespace CryptoNest.Shared.Infrastructure;

internal static class InfrastructureExtensions
{
    public static WebApplicationBuilder ConfigureInfrastructure(this WebApplicationBuilder builder)
    {
        builder.AddNlogConfiguration();
        builder.AddCoinMarketCapConfiguration();
        builder.AddSqlServerConfiguration();

        return builder;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, List<Assembly> assemblies)
    {
        List<string> disabledModules = GetDisabledModules(services);
        
        services.AddHostedService<AppInitializer>();
        services.AddErrorHandling();
        services.AddCoinMarketCap();
        services.AddQueries(assemblies);
        services.AddEvents(assemblies);
        services.AddModuleRequests(assemblies);
        services.AddMessaging();

        services.AddControllers()
            .ConfigureApplicationPartManager(manager =>
            {
                List<ApplicationPart> applicationPartsToRemove = new();

                foreach (string disabledModule in disabledModules)
                {
                    IEnumerable<ApplicationPart> parts = manager.ApplicationParts.Where(x =>
                        x.Name.Contains(disabledModule, StringComparison.InvariantCultureIgnoreCase));

                    applicationPartsToRemove.AddRange(parts);
                }

                foreach (ApplicationPart part in applicationPartsToRemove)
                {
                    manager.ApplicationParts.Remove(part);
                }

                manager.FeatureProviders.Add(new InternalControllerFeatureProvider());
            });

        services.AddAutoMapper(assemblies);

        return services;
    }

    public static IApplicationBuilder UseInfrastructure(this WebApplication app)
    {
        app.UseErrorHandling();

        if (app.Environment.IsDevelopment())
        {
            app.UseCors(corsPolicyBuilder =>
            {
                corsPolicyBuilder.AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed(origin => true)
                    .WithExposedHeaders("Content-Disposition");
            });
        }
        
        return app;
    }

    private static List<string> GetDisabledModules(IServiceCollection services)
    {
        List<string> disabledModules = new();
        using ServiceProvider serviceProvider = services.BuildServiceProvider();

        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        
        foreach ((string key, string value) in configuration.AsEnumerable())
        {
            if (!key.Contains(":module:enabled"))
            {
                continue;
            }

            if (!bool.Parse(value))
            {
                disabledModules.Add(key.Split(":")[0]);
            }
        }

        return disabledModules;
    }
}