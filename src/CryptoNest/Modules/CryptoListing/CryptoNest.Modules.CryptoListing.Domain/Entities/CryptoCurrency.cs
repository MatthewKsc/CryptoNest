namespace CryptoNest.Modules.CryptoListing.Domain.Entities;

public class CryptoCurrency
{
    public int Id { get; set; }
    public string CurrencyName { get; set; }
    public string Symbol { get; set; }
    public string Slug { get; set; }
    public int MarketRank { get; set; }
    public decimal MarketPrice { get; set; }
    public DateTime TimeOfRecord { get; set; }
}