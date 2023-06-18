using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using CryptoNest.Shared.Infrastructure.CoinMarketCap;
using CryptoNest.Shared.Infrastructure.Events;
using CryptoNest.Shared.Infrastructure.Messaging;
using CryptoNest.Shared.Infrastructure.Modules;
using CryptoNest.Shared.Infrastructure.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

[assembly:InternalsVisibleTo("CryptoNest.Bootstrapper")]
namespace CryptoNest.Shared.Infrastructure;

internal static class InfrastructureExtensions
{
    public static WebApplicationBuilder ConfigureInfrastructure(this WebApplicationBuilder builder)
    {
        builder.AddCoinMarketCapConfiguration();
        builder.AddSqlServerConfiguration();

        return builder;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, List<Assembly> assemblies)
    {
        services.AddCoinMarketCap();
        services.AddEvents(assemblies);
        services.AddModuleRequests(assemblies);
        services.AddMessaging();

        return services;
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
    {
        return app;
    }
}