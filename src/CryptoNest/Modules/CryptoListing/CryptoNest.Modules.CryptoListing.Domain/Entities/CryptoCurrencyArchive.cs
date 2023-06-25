namespace CryptoNest.Modules.CryptoListing.Domain.Entities;

public class CryptoCurrencyArchive
{
    public int Id { get; set; }
    public string CurrencyName { get; set; }
    public string Symbol { get; set; }
    public string Slug { get; set; }
    public decimal OldMarketPrice { get; set; }
    public DateTime TimeOfRecord { get; set; }
}