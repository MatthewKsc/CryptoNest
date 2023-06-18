using CryptoNest.Shared.Abstractions.Messaging;
using CryptoNest.Shared.Infrastructure.Messaging.Brokers;
using CryptoNest.Shared.Infrastructure.Messaging.Channels;
using CryptoNest.Shared.Infrastructure.Messaging.Dispatchers;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoNest.Shared.Infrastructure.Messaging;

internal static class MessagingExtensions
{
    internal static IServiceCollection AddMessaging(this IServiceCollection services)
    {
        services.AddSingleton<IMessageBroker, MessageBroker>();
        services.AddSingleton<IMessageChannel, MessageChannel>();
        services.AddSingleton<IAsyncMessageDispatcher, AsyncMessageDispatcher>();

        services.AddHostedService<BackgroundDispatcher>();
        
        return services;
    }
}