using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoNest.Modules.CryptoListing.Domain.Entities;
using CryptoNest.Modules.CryptoListing.Domain.Repositories;
using CryptoNest.Modules.CryptoListing.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace CryptoNest.Modules.CryptoListing.Infrastructure.Repositories;

internal sealed class CryptoCurrencyArchiveRepository : ICryptoCurrencyArchiveRepository
{
    private readonly CryptoListingDbContext dbContext;
    private readonly DbSet<CryptoCurrencyArchive> currencyArchives;

    public CryptoCurrencyArchiveRepository(CryptoListingDbContext dbContext)
    {
        this.dbContext = dbContext;
        this.currencyArchives = dbContext.CryptoCurrencyArchives;
    }

    public async Task<CryptoCurrencyArchive> GetByIdAsync(int id) =>
        await currencyArchives.SingleOrDefaultAsync(currency => currency.Id == id);

    public async Task<CryptoCurrencyArchive> GetBySymbolAsync(string symbol) =>
        await currencyArchives.SingleOrDefaultAsync(currency => currency.Symbol == symbol);

    public async Task<IEnumerable<CryptoCurrencyArchive>> GetAllAsync() => await currencyArchives.ToListAsync();

    public async Task AddAsync(CryptoCurrencyArchive currency)
    {
        await currencyArchives.AddAsync(currency);
        await dbContext.SaveChangesAsync();
    }

    public async Task AddRangeAsync(IEnumerable<CryptoCurrencyArchive> currencies)
    {
        await currencyArchives.AddRangeAsync(currencies);
        await dbContext.SaveChangesAsync();
    }
}