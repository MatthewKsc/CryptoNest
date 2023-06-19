using System.Collections.Generic;
using CryptoNest.Shared.Abstractions.Events;

namespace CryptoNest.Shared.Infrastructure.Events;

internal record CryptoCurrenciesFetched(List<CryptoCurrencyFetched> CryptoCurrencies) : IIntegrationEvent;
internal record CryptoCurrencyFetched(string Name, string Symbol, string Slug, int Rank, decimal Price) : IIntegrationEvent;