using CryptoNest.Modules.CryptoListing.Domain.Entities;

namespace CryptoNest.Modules.CryptoListing.Domain.Repositories;

public interface ICryptoCurrencyArchiveRepository
{
    Task<CryptoCurrencyArchive> GetByIdAsync(int id);
    Task<CryptoCurrencyArchive> GetBySymbolAsync(string symbol);
    Task<IEnumerable<CryptoCurrencyArchive>> GetAllAsync();
    Task AddAsync(CryptoCurrencyArchive currency);
    Task AddRangeAsync(IEnumerable<CryptoCurrencyArchive> currencies);
}