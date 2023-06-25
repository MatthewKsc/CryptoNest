using CryptoNest.Modules.CryptoListing.Domain.Entities;

namespace CryptoNest.Modules.CryptoListing.Domain.Repositories;

public interface ICryptoCurrencyRepository
{
    Task<CryptoCurrency> GetByIdAsync(int id);
    Task<CryptoCurrency> GetBySymbolAsync(string symbol);
    Task<IEnumerable<CryptoCurrency>> GetAllAsync();
    Task AddAsync(CryptoCurrency currency);
    Task AddRangeAsync(IEnumerable<CryptoCurrency> currencies);
}