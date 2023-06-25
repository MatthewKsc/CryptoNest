using System;
using CryptoNest.Modules.CryptoListing.Application.DTO;
using CryptoNest.Shared.Abstractions.Queries;

namespace CryptoNest.Modules.CryptoListing.Application.Queries;

public class GetCryptoCurrencyArchive : IQuery<CryptoCurrencyArchiveDto>
{
    public string Symbol { get; set; }
    public DateTime? PriceHistoricalEndDate { get; set; }
}