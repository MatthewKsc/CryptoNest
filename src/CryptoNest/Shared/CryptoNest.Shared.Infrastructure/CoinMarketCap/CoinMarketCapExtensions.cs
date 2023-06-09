using CryptoNest.Shared.Infrastructure.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoNest.Shared.Infrastructure.CoinMarketCap;

internal static class CoinMarketCapExtensions
{
    internal static WebApplicationBuilder AddCoinMarketCapConfiguration(this WebApplicationBuilder builder)
    {
        IConfiguration coinMarketCapConfiguration = builder.Configuration.GetSection("CoinMarketCapApi");
        builder.Services.Configure<CoinMarketCapOptions>(coinMarketCapConfiguration);

        return builder;
    }
    
    internal static IServiceCollection AddCoinMarketCap(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddHostedService<CryptoInfoSync>();
        
        return services;
    }
}