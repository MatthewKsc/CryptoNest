using CryptoNest.Modules.CryptoListing.Domain.Entities;

namespace CryptoNest.Modules.CryptoListing.Domain.Repositories;

public interface ICryptoCurrencyArchiveRepository
{
    Task<IReadOnlyCollection<CryptoCurrencyArchive>> GetHistoricalCurrencyDataAsync(string symbol, DateTime priceHistoricalEndDate);
    Task AddRangeAsync(IEnumerable<CryptoCurrencyArchive> currencies);
}