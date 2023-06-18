using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoNest.Shared.Abstractions.Events;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoNest.Shared.Infrastructure.Events;

internal sealed class IntegrationEventDispatcher : IIntegrationEventDispatcher
{
    private readonly IServiceProvider serviceProvider;

    public IntegrationEventDispatcher(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : class, IIntegrationEvent
    {
        using IServiceScope scope = serviceProvider.CreateScope();
        IEnumerable<IIntegrationEventHandler<TEvent>> handlers = scope.ServiceProvider
            .GetServices<IIntegrationEventHandler<TEvent>>();

        IEnumerable<Task> handlerTasks = handlers.Select(handler => handler.HandleAsync(@event));

        await Task.WhenAll(handlerTasks);
    } 
}