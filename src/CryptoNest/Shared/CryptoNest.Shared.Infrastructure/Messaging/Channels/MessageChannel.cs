using System.Threading.Channels;
using CryptoNest.Shared.Abstractions.Messaging;

namespace CryptoNest.Shared.Infrastructure.Messaging.Channels;

internal sealed class MessageChannel : IMessageChannel
{
    private readonly Channel<IMessage> messages;

    public MessageChannel()
    {
        messages = Channel.CreateUnbounded<IMessage>();
    }

    public ChannelReader<IMessage> Reader => messages.Reader;
    public ChannelWriter<IMessage> Writer => messages.Writer;
}