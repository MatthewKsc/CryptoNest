using CryptoNest.Modules.CryptoListing.Application.DTO;
using CryptoNest.Shared.Abstractions.Queries;

namespace CryptoNest.Modules.CryptoListing.Application.Queries;

public class GetCryptoCurrency : IQuery<CryptoCurrencyDto>
{
    public string Symbol { get; set; }
}