using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CryptoNest.Modules.CryptoListing.Application.DTO;
using CryptoNest.Modules.CryptoListing.Application.Exceptions;
using CryptoNest.Modules.CryptoListing.Application.Queries;
using CryptoNest.Modules.CryptoListing.Domain.Entities;
using CryptoNest.Modules.CryptoListing.Domain.Repositories;
using CryptoNest.Shared.Abstractions.Queries;

namespace CryptoNest.Modules.CryptoListing.Infrastructure.Handlers.Queries;

internal sealed class BrowseCryptoCurrenciesHandler : IQueryHandler<BrowseCryptoCurrencies, PageResult<CryptoCurrencyDto>>
{
    private readonly IMapper mapper;
    private readonly ICryptoCurrencyRepository currencyRepository;

    public BrowseCryptoCurrenciesHandler(IMapper mapper, ICryptoCurrencyRepository currencyRepository)
    {
        this.mapper = mapper;
        this.currencyRepository = currencyRepository;
    }
    
    public async Task<PageResult<CryptoCurrencyDto>> HandleAsync(BrowseCryptoCurrencies query)
    {
        if (string.IsNullOrWhiteSpace(query.SortBy) || typeof(CryptoCurrency).GetProperty(query.SortBy) is null)
        {
            throw new BrowseCryptoCurrenciesSortByEmptyException();
        }

        int numberOfItemToSkip = GetItemsToSkip(query);

        long countOfAllCurrencies = await currencyRepository.GetAllCountAsync();
        
        IReadOnlyCollection<CryptoCurrency> currencies = await currencyRepository
            .GetPaginatedDataAsync(query.SortBy, query.IsAscending, numberOfItemToSkip, query.PageSize);

        return new PageResult<CryptoCurrencyDto>(
            mapper.Map<CryptoCurrencyDto[]>(currencies),
            countOfAllCurrencies,
            query.PageSize,
            query.PageNumber);
    }

    private static int GetItemsToSkip(BrowseCryptoCurrencies query) 
        => query.PageNumber > 0 ? (query.PageNumber - 1) * query.PageSize : 0;
}