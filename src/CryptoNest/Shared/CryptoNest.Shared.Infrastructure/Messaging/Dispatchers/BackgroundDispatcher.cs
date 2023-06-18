using System;
using System.Threading;
using System.Threading.Tasks;
using CryptoNest.Shared.Abstractions.Messaging;
using CryptoNest.Shared.Abstractions.Modules;
using CryptoNest.Shared.Infrastructure.Messaging.Channels;
using Microsoft.Extensions.Hosting;

namespace CryptoNest.Shared.Infrastructure.Messaging.Dispatchers;

internal sealed class BackgroundDispatcher : BackgroundService
{
    //TODO Add logger in up coming PR
    private readonly IMessageChannel messageChannel;
    private readonly IModuleClient moduleClient;

    public BackgroundDispatcher(IMessageChannel messageChannel, IModuleClient moduleClient)
    {
        this.messageChannel = messageChannel;
        this.moduleClient = moduleClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (IMessage message in messageChannel.Reader.ReadAllAsync(stoppingToken))
        {
            try
            {
                await moduleClient.PublishAsync(messageChannel);
            }
            catch (Exception exception)
            {
                //
                Console.WriteLine(exception.Message);
            }
        }
    }
}