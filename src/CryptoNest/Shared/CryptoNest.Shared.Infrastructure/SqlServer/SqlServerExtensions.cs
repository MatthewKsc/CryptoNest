using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoNest.Shared.Infrastructure.SqlServer;

public static class SqlServerExtensions
{
    private const string SqlServerSectionName = "sqlserver";
    
    internal static WebApplicationBuilder AddSqlServerConfiguration(this WebApplicationBuilder builder)
    {
        IConfiguration sqlServerConfiguration = builder.Configuration.GetSection(SqlServerSectionName);
        builder.Services.Configure<SqlServerOptions>(sqlServerConfiguration);

        return builder;
    }

    public static IServiceCollection AddSqlServerDbContext<T>(this IServiceCollection services, IConfiguration configuration)
        where T : DbContext
    {
        SqlServerOptions sqlServerConfiguration = new();
        configuration.GetSection(SqlServerSectionName).Bind(sqlServerConfiguration);

        services.AddDbContext<T>(x => x.UseSqlServer(sqlServerConfiguration.ConnectionString));

        return services;
    }
}