using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using CryptoNest.Shared.Abstractions.Messaging;
using CryptoNest.Shared.Infrastructure.Events;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace CryptoNest.Shared.Infrastructure.CoinMarketCap;

// TODO: Move to separate module
internal class CryptoInfoSync : BackgroundService
{
    private readonly HttpClient httpClient;
    private readonly ILogger<CryptoInfoSync> logger;
    private readonly CoinMarketCapOptions cmcOptions;
    private readonly IMessageBroker messageBroker;

    public CryptoInfoSync(
        HttpClient httpClient,
        ILogger<CryptoInfoSync> logger,
        IOptions<CoinMarketCapOptions> cmcOptions,
        IMessageBroker messageBroker)
    {
        this.httpClient = httpClient;
        this.logger = logger;
        this.cmcOptions = cmcOptions.Value;
        this.messageBroker = messageBroker;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Started fetching crypto currencies job.....");
            
            try
            {
                UriBuilder uriBuilder = new($"{cmcOptions.ApiBaseUrl}{cmcOptions.ApiEndpoint}");
                NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
                
                query["start"] = "1";
                query["limit"] = Convert.ToString(cmcOptions.ListingLimit);
                query["sort"] = cmcOptions.SortBy;
                
                uriBuilder.Query = query.ToString();
                string requestApiUrl = uriBuilder.ToString();
                
                HttpRequestMessage request = new(HttpMethod.Get, requestApiUrl);
                
                request.Headers.Add("X-CMC_PRO_API_KEY", cmcOptions.ApiKey);
                request.Headers.Add("Accept", "application/json");

                HttpResponseMessage response = await httpClient.SendAsync(request, stoppingToken);

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync(stoppingToken);
               
                JObject jObject = JObject.Parse(responseBody);
                var data = (JArray)jObject["data"]!;
                
                List<CryptoCurrency> cryptoCurrencies = new();

                foreach (JToken crypto in data)
                {
                    decimal cryptoCurrencyPrice = crypto.SelectToken("quote.USD.price")!
                        .Value<decimal>();

                    var cryptoCurrency = crypto.ToObject<CryptoCurrency>();
                    cryptoCurrency!.Price = cryptoCurrencyPrice;
                    
                    cryptoCurrencies.Add(cryptoCurrency);
                }
                
                List<CryptoCurrencyFetched> cryptocurrenciesFetched = cryptoCurrencies
                    .Select(currency => new CryptoCurrencyFetched(
                        currency.Name,
                        currency.Symbol,
                        currency.Slug,
                        currency.Cmc_rank,
                        currency.Price))
                    .ToList();

                await messageBroker.PublishAsync(new CryptoCurrenciesFetched(cryptocurrenciesFetched));
                logger.LogInformation("End of fetching crypto currencies job, fetched: {count}", cryptocurrenciesFetched.Count);
                
                await Task.Delay(TimeSpan.FromMinutes(cmcOptions.BackgroundServiceIntervalMinutes), stoppingToken);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Exception Occured while fetching crypto currencies from CoinMarketCap");
            }
        }
    }
}