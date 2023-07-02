using CryptoNest.Shared.Abstractions.Exceptions;

namespace CryptoNest.Modules.CryptoListing.Application.Exceptions;

public class CryptoCurrencySymbolEmptyException : CryptoNestBaseException
{
    public CryptoCurrencySymbolEmptyException() : base("Crypto currency symbol is not provided") { }
}