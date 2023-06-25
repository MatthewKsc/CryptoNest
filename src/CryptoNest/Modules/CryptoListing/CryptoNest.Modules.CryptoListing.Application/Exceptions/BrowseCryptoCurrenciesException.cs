using System;

namespace CryptoNest.Modules.CryptoListing.Application.Exceptions;

public class BrowseCryptoCurrenciesException : Exception
{
    public BrowseCryptoCurrenciesException(string message) : base(message) { }
}