using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using AutoMapper;
using CryptoNest.Modules.CryptoListing.Application.DTO;
using CryptoNest.Modules.CryptoListing.Application.Exceptions;
using CryptoNest.Modules.CryptoListing.Application.Queries;
using CryptoNest.Modules.CryptoListing.Domain.Entities;
using CryptoNest.Modules.CryptoListing.Infrastructure.Db;
using CryptoNest.Shared.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;

namespace CryptoNest.Modules.CryptoListing.Infrastructure.Handlers.Queries;

internal sealed class BrowseCryptoCurrenciesHandler : IQueryHandler<BrowseCryptoCurrencies, IEnumerable<CryptoCurrencyDto>>
{
    private const string DynamicLinqDescendingExpression = "descending";
    private readonly IMapper mapper;
    private readonly DbSet<CryptoCurrency> cryptoCurrencies;

    public BrowseCryptoCurrenciesHandler(IMapper mapper, CryptoListingDbContext dbContext)
    {
        this.mapper = mapper;
        this.cryptoCurrencies = dbContext.CryptoCurrencies;
    }
    
    public async Task<IEnumerable<CryptoCurrencyDto>> HandleAsync(BrowseCryptoCurrencies query)
    {
        if (string.IsNullOrWhiteSpace(query.SortBy) || typeof(CryptoCurrency).GetProperty(query.SortBy) is null)
        {
            throw new BrowseCryptoCurrenciesSortByEmptyException();
        }

        int numberOfItemToSkip = GetItemsToSkip(query);

        IOrderedQueryable<CryptoCurrency> orderedQueryable = query.IsAscending
            ? cryptoCurrencies.OrderBy(query.SortBy)
            : cryptoCurrencies.OrderBy($"{query.SortBy} {DynamicLinqDescendingExpression}");
        
        return await orderedQueryable
            .Skip(numberOfItemToSkip)
            .Take(query.PageSize)
            .Select(cryptoCurrency => mapper.Map<CryptoCurrencyDto>(cryptoCurrency))
            .ToListAsync();
    }

    private static int GetItemsToSkip(BrowseCryptoCurrencies query) 
        => query.PageNumber > 0 ? (query.PageNumber - 1) * query.PageSize : 0;
}