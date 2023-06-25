using System;

namespace CryptoNest.Modules.CryptoListing.Application.Exceptions;

public class GetCryptoCurrencyException : Exception
{
    public GetCryptoCurrencyException(string message) : base(message) { }
}