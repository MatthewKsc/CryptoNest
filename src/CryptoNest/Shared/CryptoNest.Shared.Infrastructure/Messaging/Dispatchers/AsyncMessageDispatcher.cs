using System.Threading.Tasks;
using CryptoNest.Shared.Abstractions.Messaging;
using CryptoNest.Shared.Infrastructure.Messaging.Channels;

namespace CryptoNest.Shared.Infrastructure.Messaging.Dispatchers;

internal sealed class AsyncMessageDispatcher : IAsyncMessageDispatcher
{
    private readonly IMessageChannel messageChannel;

    public AsyncMessageDispatcher(IMessageChannel messageChannel)
    {
        this.messageChannel = messageChannel;
    }

    public async Task PublishAsync<TMessage>(TMessage message) where TMessage : class, IMessage
        => await messageChannel.Writer.WriteAsync(message);
}