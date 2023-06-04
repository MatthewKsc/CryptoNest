using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

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
    
    private static IEnumerable<string> GetSettings(string pattern, WebApplicationBuilder  builder) =>
        Directory.EnumerateFiles(builder.Environment.ContentRootPath, $"module.{pattern}.json", SearchOption.AllDirectories);
}