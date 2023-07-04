using System;
using System.Collections.Generic;
using System.Linq;
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

internal sealed class GetCryptoCurrencyArchiveHandler : IQueryHandler<GetCryptoCurrencyArchive, CryptoCurrencyArchiveDto>
{
    private readonly IMapper mapper;
    private readonly DbSet<CryptoCurrencyArchive> cryptoCurrencyArchives;

    public GetCryptoCurrencyArchiveHandler(IMapper mapper, CryptoListingDbContext dbContext)
    {
        this.mapper = mapper;
        this.cryptoCurrencyArchives = dbContext.CryptoCurrencyArchives;
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

        CryptoCurrencyArchive[] archives = await cryptoCurrencyArchives
            .Where(currency => currency.Symbol == query.Symbol && currency.TimeOfRecord.Date <= DateTime.UtcNow.Date)
            .OrderBy(archive => archive.TimeOfRecord)
            .ToArrayAsync();

        if (!archives.Any())
        {
            return null;
        }

        Dictionary<DateTime, decimal> dateToPriceAscending = archives
            .ToDictionary(archive => archive.TimeOfRecord, archive => archive.OldMarketPrice);

        CryptoCurrencyArchiveDto latestCryptoCurrencyArchive = mapper.Map<CryptoCurrencyArchiveDto>(archives.First());
        latestCryptoCurrencyArchive.MarketPriceData = dateToPriceAscending;

        return latestCryptoCurrencyArchive;
    }
}