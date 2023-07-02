using CryptoNest.Shared.Abstractions.Exceptions;

namespace CryptoNest.Modules.CryptoListing.Application.Exceptions;

public class CryptoCurrencyArchiveInvalidData : CryptoNestBaseException
{
    public CryptoCurrencyArchiveInvalidData(string symbol) 
        : base($"Crypto currency archive historical price end date is invalid for symbol {symbol}") { }
}