using System;
using System.Collections.Generic;

namespace CryptoNest.Modules.CryptoListing.Application.DTO;

public class CryptoCurrencyArchiveDto
{
    public string CurrencyName { get; set; }
    public string Symbol { get; set; }
    public string Slug { get; set; }
    public Dictionary<DateTime, decimal> MarketPriceData { get; set; }
}