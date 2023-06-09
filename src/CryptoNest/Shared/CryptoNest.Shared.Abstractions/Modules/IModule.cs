using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoNest.Shared.Abstractions.Modules;

public interface IModule
{
    public string Name { get; }
    public string Path { get; }
    void Register(IServiceCollection services, IConfiguration configuration);
    void Use(IApplicationBuilder applicationBuilder);
}