using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;

namespace CryptoNest.Shared.Infrastructure.Logger;

internal static class NLogExtensions
{
    internal static WebApplicationBuilder AddNlogConfiguration(this WebApplicationBuilder builder)
    {
        LogManager.Setup()
            .LoadConfigurationFromAppSettings()
            .GetCurrentClassLogger();
        
        builder.Logging.ClearProviders();
        builder.Host.UseNLog();

        return builder;
    }
}