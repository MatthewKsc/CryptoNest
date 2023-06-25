using System;

namespace CryptoNest.Modules.CryptoListing.Application.Exceptions;

public class GetCryptoCurrencyArchiveException : Exception
{
    public GetCryptoCurrencyArchiveException(string message) : base(message) { }
}