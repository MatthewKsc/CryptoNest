using System.Threading.Tasks;
using CryptoNest.Shared.Abstractions.Messaging;

namespace CryptoNest.Shared.Infrastructure.Messaging.Dispatchers;

internal interface IAsyncMessageDispatcher
{
    Task PublishAsync<TMessage>(TMessage message) where TMessage : class, IMessage;
}