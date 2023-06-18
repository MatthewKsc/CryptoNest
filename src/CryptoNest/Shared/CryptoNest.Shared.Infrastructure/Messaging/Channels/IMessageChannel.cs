using System.Threading.Channels;
using CryptoNest.Shared.Abstractions.Messaging;

namespace CryptoNest.Shared.Infrastructure.Messaging.Channels;

public interface IMessageChannel
{
    ChannelReader<IMessage> Reader { get; }
    ChannelWriter<IMessage> Writer { get; }
}