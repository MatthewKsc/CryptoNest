using System.Threading.Tasks;

namespace CryptoNest.Shared.Abstractions.Events;

public interface IIntegrationEventHandler<in TEvent> where TEvent : class, IIntegrationEvent
{
    Task HandleAsync(TEvent @event);
}