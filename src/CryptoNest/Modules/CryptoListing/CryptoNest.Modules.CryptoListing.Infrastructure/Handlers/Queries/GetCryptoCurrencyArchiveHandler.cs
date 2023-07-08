using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CryptoNest.Modules.CryptoListing.Application.DTO;
using CryptoNest.Modules.CryptoListing.Application.Exceptions;
using CryptoNest.Modules.CryptoListing.Application.Queries;
using CryptoNest.Modules.CryptoListing.Domain.Entities;
using CryptoNest.Modules.CryptoListing.Domain.Repositories;
using CryptoNest.Shared.Abstractions.Queries;

namespace CryptoNest.Modules.CryptoListing.Infrastructure.Handlers.Queries;

internal sealed class GetCryptoCurrencyArchiveHandler : IQueryHandler<GetCryptoCurrencyArchive, CryptoCurrencyArchiveDto>
{
    private readonly IMapper mapper;
    private readonly ICryptoCurrencyArchiveRepository currencyArchiveRepository;

    public GetCryptoCurrencyArchiveHandler(IMapper mapper, ICryptoCurrencyArchiveRepository currencyArchiveRepository)
    {
        this.mapper = mapper;
        this.currencyArchiveRepository = currencyArchiveRepository;
    }
    
    public async Task<CryptoCurrencyArchiveDto> HandleAsync(GetCryptoCurrencyArchive query)
    {
        if (string.IsNullOrWhiteSpace(query.Symbol)) 
        {
            throw new CryptoCurrencySymbolEmptyException();
        }

        if (query.PriceHistoricalEndDate is null || query.PriceHistoricalEndDate.Value.Date > DateTime.UtcNow.Date)
        {
            throw new CryptoCurrencyArchiveInvalidData(query.Symbol);
        }
        
        IReadOnlyCollection<CryptoCurrencyArchive> archives = 
            await currencyArchiveRepository.GetHistoricalCurrencyDataAsync(query.Symbol, query.PriceHistoricalEndDate.Value.Date);

        if (!archives.Any())
        {
            return null;
        }

        Dictionary<DateTime, decimal> dateToPriceAscending = archives
            .ToDictionary(archive => archive.TimeOfRecord, archive => archive.OldMarketPrice);

        var latestCryptoCurrencyArchive = mapper.Map<CryptoCurrencyArchiveDto>(archives.First());
        latestCryptoCurrencyArchive.MarketPriceData = dateToPriceAscending;

        return latestCryptoCurrencyArchive;
    }
}