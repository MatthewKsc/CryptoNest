using AutoMapper;
using CryptoNest.Modules.CryptoListing.Domain.Entities;
using CryptoNest.Modules.CryptoListing.Domain.Repositories;
using CryptoNest.Shared.Abstractions.Events;
using Microsoft.Extensions.Logging;

namespace CryptoNest.Modules.CryptoListing.Domain.Events.External.Handlers;

internal sealed class CryptoCurrenciesFetchedHandler : IIntegrationEventHandler<CryptoCurrenciesFetched>
{
    private readonly IMapper mapper;
    private readonly ILogger<CryptoCurrenciesFetched> logger;
    private readonly ICryptoCurrencyRepository cryptoCurrencyRepository;
    private readonly ICryptoCurrencyArchiveRepository cryptoCurrencyArchiveRepository;

    public CryptoCurrenciesFetchedHandler(
        IMapper mapper,
        ILogger<CryptoCurrenciesFetched> logger,
        ICryptoCurrencyRepository cryptoCurrencyRepository,
        ICryptoCurrencyArchiveRepository cryptoCurrencyArchiveRepository)
    {
        this.mapper = mapper;
        this.logger = logger;
        this.cryptoCurrencyRepository = cryptoCurrencyRepository;
        this.cryptoCurrencyArchiveRepository = cryptoCurrencyArchiveRepository;
    }

    public async Task HandleAsync(CryptoCurrenciesFetched @event)
    {
        if (!@event.CryptoCurrencies.Any())
        {
            return;
        }
        
        IReadOnlyCollection<CryptoCurrency> cryptoCurrencies = @event.CryptoCurrencies
            .Where(currency => !string.IsNullOrWhiteSpace(currency.Symbol) && currency.Price > 0)
            .Select(currency => mapper.Map<CryptoCurrency>(currency))
            .ToArray();

        IEnumerable<CryptoCurrency> currenciesToArchive = await cryptoCurrencyRepository.GetAllAsync();

        IReadOnlyCollection<CryptoCurrencyArchive> cryptoCurrencyArchives = currenciesToArchive
            .Select(currencyToArchive => mapper.Map<CryptoCurrencyArchive>(currencyToArchive))
            .ToArray();
        
        await cryptoCurrencyArchiveRepository.AddRangeAsync(cryptoCurrencyArchives);
        logger.LogInformation("Archived crypto currencies: {count}", cryptoCurrencyArchives.Count);
        
        await cryptoCurrencyRepository.DeleteAllDataAsync();
        await cryptoCurrencyRepository.AddRangeAsync(cryptoCurrencies);
        logger.LogInformation("Re-uploaded crypto currencies: {count}", cryptoCurrencies.Count);
    }
}