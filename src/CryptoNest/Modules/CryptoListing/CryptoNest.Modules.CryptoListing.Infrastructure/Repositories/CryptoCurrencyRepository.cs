using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using CryptoNest.Modules.CryptoListing.Domain.Entities;
using CryptoNest.Modules.CryptoListing.Domain.Repositories;
using CryptoNest.Modules.CryptoListing.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace CryptoNest.Modules.CryptoListing.Infrastructure.Repositories;

internal sealed class CryptoCurrencyRepository : ICryptoCurrencyRepository
{
    private const string DynamicLinqDescendingExpression = "descending";
    
    private readonly CryptoListingDbContext dbContext;
    private readonly DbSet<CryptoCurrency> currencies;

    public CryptoCurrencyRepository(CryptoListingDbContext dbContext)
    {
        this.dbContext = dbContext;
        this.currencies = this.dbContext.CryptoCurrencies;
    }

    public async Task<CryptoCurrency> GetBySymbolAsync(string symbol)
        => await currencies.SingleOrDefaultAsync(currency => currency.Symbol == symbol);

    public async Task<IEnumerable<CryptoCurrency>> GetAllAsync() => await currencies.ToListAsync();
    public async Task<long> GetAllCountAsync() => await currencies.LongCountAsync();

    public async Task<IReadOnlyCollection<CryptoCurrency>> GetPaginatedDataAsync(
        string orderBy,
        bool isAscending,
        int skipItems,
        int takeItems)
    {
        IOrderedQueryable<CryptoCurrency> orderedQueryable = isAscending
            ? currencies.OrderBy(orderBy)
            : currencies.OrderBy($"{orderBy} {DynamicLinqDescendingExpression}");
        
        return await orderedQueryable
            .Skip(skipItems)
            .Take(takeItems)
            .ToArrayAsync();
    }

    public async Task AddRangeAsync(IEnumerable<CryptoCurrency> currenciesToAdd)
    {
        await this.currencies.AddRangeAsync(currenciesToAdd);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAllDataAsync() => await currencies.ExecuteDeleteAsync();
}