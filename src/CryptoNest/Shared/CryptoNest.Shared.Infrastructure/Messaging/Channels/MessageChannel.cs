using System.Threading.Channels;
using CryptoNest.Shared.Abstractions.Messaging;

namespace CryptoNest.Shared.Infrastructure.Messaging.Channels;

internal sealed class MessageChannel : IMessageChannel
{
    private static Channel<IMessage> Messages => Channel.CreateUnbounded<IMessage>();

    public ChannelReader<IMessage> Reader => Messages.Reader;
    public ChannelWriter<IMessage> Writer => Messages.Writer;
}