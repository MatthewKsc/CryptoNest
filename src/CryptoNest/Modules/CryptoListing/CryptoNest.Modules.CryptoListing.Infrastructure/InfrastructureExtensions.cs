using CryptoNest.Modules.CryptoListing.Domain.Repositories;
using CryptoNest.Modules.CryptoListing.Infrastructure.Db;
using CryptoNest.Modules.CryptoListing.Infrastructure.Repositories;
using CryptoNest.Shared.Infrastructure.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoNest.Modules.CryptoListing.Infrastructure;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) =>
        services
            .AddScoped<ICryptoCurrencyRepository, CryptoCurrencyRepository>()
            .AddScoped<ICryptoCurrencyArchiveRepository, CryptoCurrencyArchiveRepository>()
            .AddSqlServerDbContext<CryptoListingDbContext>(configuration);
}