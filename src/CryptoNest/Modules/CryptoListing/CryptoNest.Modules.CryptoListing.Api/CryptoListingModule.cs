using CryptoNest.Modules.CryptoListing.Infrastructure;
using CryptoNest.Shared.Abstractions.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoNest.Modules.CryptoListing.Api;

internal class CryptoListingModule : IModule
{
    public const string BasePath = "api/crypto-listing-module";

    public string Name { get; } = "CryptoListing";
    public string Path => BasePath;

    public void Register(IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);
    }

    public void Use(IApplicationBuilder applicationBuilder)
    {
    }
}