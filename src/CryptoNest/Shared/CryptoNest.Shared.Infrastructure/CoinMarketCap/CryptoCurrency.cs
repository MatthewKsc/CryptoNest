using Newtonsoft.Json;

namespace CryptoNest.Shared.Infrastructure.CoinMarketCap;

internal class CryptoCurrency
{
    public string Name { get; set; }
    public string Symbol { get; set; }
    public string Slug { get; set; }
    public int Cmc_rank { get; set; }
    [JsonIgnore]
    public decimal Price { get; set; }
}