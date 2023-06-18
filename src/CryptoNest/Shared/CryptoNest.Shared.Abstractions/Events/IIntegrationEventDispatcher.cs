using System.Threading.Tasks;

namespace CryptoNest.Shared.Abstractions.Events;

public interface IIntegrationEventDispatcher
{
    Task PublishAsync<TEvent>(TEvent @event) where TEvent : class, IIntegrationEvent;
}