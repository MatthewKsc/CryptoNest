using CryptoNest.Shared.Abstractions.Events;

namespace CryptoNest.Modules.CryptoListing.Domain.Events.External;

public record CryptoCurrenciesFetched(List<CryptoCurrencyFetched> CryptoCurrencies) : IIntegrationEvent;
public record CryptoCurrencyFetched(string Name, string Symbol, string Slug, int Rank, decimal Price) : IIntegrationEvent;