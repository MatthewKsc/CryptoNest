using System;

namespace CryptoNest.Modules.CryptoListing.Application.DTO;

public class CryptoCurrencyDto
{
    public string CurrencyName { get; set; }
    public string Symbol { get; set; }
    public string Slug { get; set; }
    public int MarketRank { get; set; }
    public decimal MarketPrice { get; set; }
    public DateTime TimeOfRecord { get; set; }
}