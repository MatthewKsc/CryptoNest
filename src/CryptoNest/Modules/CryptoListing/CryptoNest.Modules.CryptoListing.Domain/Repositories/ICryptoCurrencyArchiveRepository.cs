using CryptoNest.Modules.CryptoListing.Domain.Entities;

namespace CryptoNest.Modules.CryptoListing.Domain.Repositories;

public interface ICryptoCurrencyArchiveRepository
{
    Task AddRangeAsync(IEnumerable<CryptoCurrencyArchive> currencies);
}