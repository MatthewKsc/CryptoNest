using CryptoNest.Shared.Abstractions.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoNest.Shared.Infrastructure.Exceptions;

internal static class ExceptionsExtensions
{
    public static IServiceCollection AddErrorHandling(this IServiceCollection services) 
        => services.AddScoped<ExceptionMiddleware>()
            .AddSingleton<IExceptionToResponseMapper, ExceptionToResponseMapper>()
            .AddSingleton<IExceptionCompositionRoot, ExceptionCompositionRoot>();

    public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder app) 
        => app.UseMiddleware<ExceptionMiddleware>();
}