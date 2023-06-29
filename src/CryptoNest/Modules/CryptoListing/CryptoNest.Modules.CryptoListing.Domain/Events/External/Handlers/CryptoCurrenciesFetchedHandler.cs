using CryptoNest.Modules.CryptoListing.Domain.Entities;
using CryptoNest.Modules.CryptoListing.Domain.Repositories;
using CryptoNest.Shared.Abstractions.Events;

namespace CryptoNest.Modules.CryptoListing.Domain.Events.External.Handlers;

internal sealed class CryptoCurrenciesFetchedHandler : IIntegrationEventHandler<CryptoCurrenciesFetched>
{
    private readonly ICryptoCurrencyRepository cryptoCurrencyRepository;
    private readonly ICryptoCurrencyArchiveRepository cryptoCurrencyArchiveRepository;

    public CryptoCurrenciesFetchedHandler(
        ICryptoCurrencyRepository cryptoCurrencyRepository,
        ICryptoCurrencyArchiveRepository cryptoCurrencyArchiveRepository)
    {
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
            .Select(currency => new CryptoCurrency()
            {
                CurrencyName = currency.Name,
                Symbol = currency.Symbol,
                Slug = currency.Slug,
                MarketRank = currency.Rank,
                MarketPrice = Math.Round(currency.Price, 18),
                TimeOfRecord = DateTime.UtcNow,
            })
            .ToArray();

        IEnumerable<CryptoCurrency> currenciesToArchive = await cryptoCurrencyRepository.GetAllAsync();

        IReadOnlyCollection<CryptoCurrencyArchive> cryptoCurrencyArchives = currenciesToArchive
            .Select(currencyToArchive => new CryptoCurrencyArchive()
            { 
                CurrencyName = currencyToArchive.CurrencyName,
                Symbol = currencyToArchive.Symbol,
                Slug = currencyToArchive.Slug,
                OldMarketPrice = currencyToArchive.MarketPrice,
                TimeOfRecord = currencyToArchive.TimeOfRecord
            })
            .ToArray();

        await cryptoCurrencyArchiveRepository.AddRangeAsync(cryptoCurrencyArchives);
        
        await cryptoCurrencyRepository.DeleteAllDataAsync();
        await cryptoCurrencyRepository.AddRangeAsync(cryptoCurrencies);
    }
}