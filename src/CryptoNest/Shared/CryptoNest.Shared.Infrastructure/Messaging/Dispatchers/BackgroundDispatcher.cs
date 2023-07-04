using System;
using System.Threading;
using System.Threading.Tasks;
using CryptoNest.Shared.Abstractions.Messaging;
using CryptoNest.Shared.Abstractions.Modules;
using CryptoNest.Shared.Infrastructure.Messaging.Channels;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CryptoNest.Shared.Infrastructure.Messaging.Dispatchers;

internal sealed class BackgroundDispatcher : BackgroundService
{
    private readonly ILogger<BackgroundDispatcher> logger;
    private readonly IMessageChannel messageChannel;
    private readonly IModuleClient moduleClient;

    public BackgroundDispatcher(ILogger<BackgroundDispatcher> logger, IMessageChannel messageChannel, IModuleClient moduleClient)
    {
        this.logger = logger;
        this.messageChannel = messageChannel;
        this.moduleClient = moduleClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (IMessage message in messageChannel.Reader.ReadAllAsync(stoppingToken))
        {
            try
            {
                await moduleClient.PublishAsync(message);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Exception while publishing message: {exMsg}", exception.Message);
            }
        }
    }
}