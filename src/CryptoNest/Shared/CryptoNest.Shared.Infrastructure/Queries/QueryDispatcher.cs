using System;
using System.Threading.Tasks;
using CryptoNest.Shared.Abstractions.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoNest.Shared.Infrastructure.Queries;

internal sealed class QueryDispatcher : IQueryDispatcher
{
    private readonly IServiceProvider serviceProvider;

    public QueryDispatcher(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }
    
    public async Task<TResult> QueryAsync<TResult>(IQuery<TResult> query)
    {
        using IServiceScope scope = serviceProvider.CreateScope();
        Type handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
        object handler = scope.ServiceProvider.GetRequiredService(handlerType);

        return await (Task<TResult>) handlerType
            .GetMethod(nameof(IQueryHandler<IQuery<TResult>, TResult>.HandleAsync))
            ?.Invoke(handler, new[] { query });
    }
}