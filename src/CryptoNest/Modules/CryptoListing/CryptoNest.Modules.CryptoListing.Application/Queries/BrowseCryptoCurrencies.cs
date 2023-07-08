using System.Collections.Generic;
using CryptoNest.Modules.CryptoListing.Application.DTO;
using CryptoNest.Shared.Abstractions.Queries;

namespace CryptoNest.Modules.CryptoListing.Application.Queries;

public class BrowseCryptoCurrencies : IQuery<PageResult<CryptoCurrencyDto>>
{
    public string SortBy { get; set; }
    public bool IsAscending { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}