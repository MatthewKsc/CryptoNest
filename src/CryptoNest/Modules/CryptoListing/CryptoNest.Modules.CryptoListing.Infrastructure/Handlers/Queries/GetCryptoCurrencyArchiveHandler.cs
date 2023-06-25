using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    private readonly DbSet<CryptoCurrencyArchive> cryptoCurrencyArchives;

    public GetCryptoCurrencyArchiveHandler(CryptoListingDbContext dbContext)
    {
        this.cryptoCurrencyArchives = dbContext.CryptoCurrencyArchives;
    }
    
    public async Task<CryptoCurrencyArchiveDto> HandleAsync(GetCryptoCurrencyArchive query)
    {
        if (string.IsNullOrWhiteSpace(query.Symbol)) 
        {
            throw new GetCryptoCurrencyArchiveException("Crypto currency archive symbol is not provided");
        }

        if (query.PriceHistoricalEndDate is null || query.PriceHistoricalEndDate.Value.Date > DateTime.UtcNow.Date)
        {
            throw new GetCryptoCurrencyArchiveException("Crypto currency archive historical price end date is invalid");
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

        CryptoCurrencyArchive latestCryptoCurrencyArchive = archives.First();

        return new CryptoCurrencyArchiveDto
        {
            CurrencyName = latestCryptoCurrencyArchive.CurrencyName,
            Symbol = latestCryptoCurrencyArchive.Symbol,
            Slug = latestCryptoCurrencyArchive.Slug,
            MarketPriceData = dateToPriceAscending
        };
    }
}