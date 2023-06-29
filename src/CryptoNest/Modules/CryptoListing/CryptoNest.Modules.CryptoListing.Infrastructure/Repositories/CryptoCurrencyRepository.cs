using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoNest.Modules.CryptoListing.Domain.Entities;
using CryptoNest.Modules.CryptoListing.Domain.Repositories;
using CryptoNest.Modules.CryptoListing.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace CryptoNest.Modules.CryptoListing.Infrastructure.Repositories;

internal sealed class CryptoCurrencyRepository : ICryptoCurrencyRepository
{
    private readonly CryptoListingDbContext dbContext;
    private readonly DbSet<CryptoCurrency> currencies;

    public CryptoCurrencyRepository(CryptoListingDbContext dbContext)
    {
        this.dbContext = dbContext;
        this.currencies = this.dbContext.CryptoCurrencies;
    }

    public async Task<CryptoCurrency> GetByIdAsync(int id) =>
        await currencies.SingleOrDefaultAsync(currency => currency.Id == id);

    public async Task<CryptoCurrency> GetBySymbolAsync(string symbol) =>
        await currencies.SingleOrDefaultAsync(currency => currency.Symbol == symbol);

    public async Task<IEnumerable<CryptoCurrency>> GetAllAsync() => await currencies.ToListAsync();

    public async Task AddAsync(CryptoCurrency currency)
    {
        await currencies.AddAsync(currency);
        await dbContext.SaveChangesAsync();
    }

    public async Task AddRangeAsync(IEnumerable<CryptoCurrency> currencies)
    {
        await this.currencies.AddRangeAsync(currencies);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAllDataAsync() => await currencies.ExecuteDeleteAsync();
}