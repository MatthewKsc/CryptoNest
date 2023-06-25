using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoNest.Modules.CryptoListing.Application.DTO;
using CryptoNest.Modules.CryptoListing.Application.Queries;
using CryptoNest.Shared.Abstractions.Queries;
using Microsoft.AspNetCore.Mvc;

namespace CryptoNest.Modules.CryptoListing.Api.Controllers;

[Route(CryptoListingModule.BasePath + "/crypto-currencies")]
internal class CryptoCurrencyController : BaseController
{
    private readonly IQueryDispatcher queryDispatcher;

    public CryptoCurrencyController(IQueryDispatcher queryDispatcher)
    {
        this.queryDispatcher = queryDispatcher;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CryptoCurrencyDto>>> BrowseCryptoCurrenciesAsync(BrowseCryptoCurrencies browseCryptoCurrencies) =>
        OkOrNotFound(await queryDispatcher.QueryAsync(browseCryptoCurrencies));

    [HttpGet("currency/{symbol}")]
    public async Task<ActionResult<CryptoCurrencyDto>> GetCryptoCurrencyAsync(string symbol) =>
        OkOrNotFound(await queryDispatcher.QueryAsync(new GetCryptoCurrency { Symbol = symbol }));
}