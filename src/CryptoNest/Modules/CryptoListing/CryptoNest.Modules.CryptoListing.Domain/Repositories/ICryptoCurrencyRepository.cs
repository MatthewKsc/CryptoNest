using CryptoNest.Modules.CryptoListing.Domain.Entities;

namespace CryptoNest.Modules.CryptoListing.Domain.Repositories;

public interface ICryptoCurrencyRepository
{
    Task<CryptoCurrency> GetBySymbolAsync(string symbol);
    Task<IEnumerable<CryptoCurrency>> GetAllAsync();
    Task AddRangeAsync(IEnumerable<CryptoCurrency> currencies);
    Task DeleteAllDataAsync();
}