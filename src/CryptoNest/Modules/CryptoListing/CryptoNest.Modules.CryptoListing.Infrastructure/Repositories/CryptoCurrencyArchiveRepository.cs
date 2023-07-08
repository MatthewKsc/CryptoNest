using System;
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

    public async Task<IReadOnlyCollection<CryptoCurrencyArchive>> GetHistoricalCurrencyDataAsync(string symbol, DateTime priceHistoricalEndDate) 
        => await currencyArchives
            .Where(currency => currency.Symbol == symbol && currency.TimeOfRecord.Date <= priceHistoricalEndDate)
            .OrderBy(archive => archive.TimeOfRecord)
            .ToArrayAsync();

    public async Task AddRangeAsync(IEnumerable<CryptoCurrencyArchive> currencies)
    {
        await currencyArchives.AddRangeAsync(currencies);
        await dbContext.SaveChangesAsync();
    }
}