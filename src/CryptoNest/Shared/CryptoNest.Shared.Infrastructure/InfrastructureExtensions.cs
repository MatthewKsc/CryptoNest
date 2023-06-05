using System.Runtime.CompilerServices;
using CryptoNest.Shared.Infrastructure.CoinMarketCap;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly:InternalsVisibleTo("CryptoNest.Bootstrapper")]
namespace CryptoNest.Shared.Infrastructure;

internal static class InfrastructureExtensions
{
    public static WebApplicationBuilder ConfigureInfrastructure(this WebApplicationBuilder builder)
    {
        IConfiguration coinMarketCapConfiguration = builder.Configuration.GetSection("CoinMarketCapApi");
        builder.Services.Configure<CoinMarketCapOptions>(coinMarketCapConfiguration);

        return builder;
    }
    
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddHostedService<CryptoInfoSync>();
        
        return services;
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
    {
        return app;
    }
}