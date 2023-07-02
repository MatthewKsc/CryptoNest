using System;
using CryptoNest.Shared.Abstractions.Exceptions;

namespace CryptoNest.Modules.CryptoListing.Application.Exceptions;

public class BrowseCryptoCurrenciesSortByEmptyException : CryptoNestBaseException
{
    public BrowseCryptoCurrenciesSortByEmptyException() : base("SortBy data is not provided") { }
}