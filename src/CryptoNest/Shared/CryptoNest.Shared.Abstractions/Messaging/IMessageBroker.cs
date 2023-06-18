using System.Threading.Tasks;

namespace CryptoNest.Shared.Abstractions.Messaging;

public interface IMessageBroker
{
    Task PublishAsync(params IMessage[] messages);
}