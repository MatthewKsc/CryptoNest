namespace CryptoNest.Shared.Infrastructure.CoinMarketCap;

internal class CoinMarketCapOptions
{
    public string ApiKey { get; set; }
    public string ApiBaseUrl { get; set; }
    public string ApiEndpoint { get; set; }
    public int ListingLimit { get; set; }
    public string SortBy { get; set; }
    public int BackgroundServiceIntervalMinutes { get; set; }
}