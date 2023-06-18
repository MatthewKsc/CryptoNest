using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoNest.Shared.Abstractions.Messaging;
using CryptoNest.Shared.Infrastructure.Messaging.Dispatchers;

namespace CryptoNest.Shared.Infrastructure.Messaging.Brokers;

internal sealed class MessageBroker : IMessageBroker
{
    private readonly IAsyncMessageDispatcher asyncMessageDispatcher;

    public MessageBroker(IAsyncMessageDispatcher asyncMessageDispatcher)
    {
        this.asyncMessageDispatcher = asyncMessageDispatcher;
    }

    public async Task PublishAsync(params IMessage[] messages)
    {
        messages = messages?.Where(message => message is not null)
            .ToArray();

        if (!messages.Any())
        {
            return;
        }

        List<Task> tasks = new();

        foreach (IMessage message in messages)
        {
            tasks.Add(asyncMessageDispatcher.PublishAsync(message));
        }

        await Task.WhenAll(tasks);
    }
}