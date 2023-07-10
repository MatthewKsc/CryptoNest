using System.Threading.Tasks;
using CryptoNest.Modules.CryptoListing.Application.DTO;
using CryptoNest.Modules.CryptoListing.Application.Queries;
using CryptoNest.Shared.Abstractions.Queries;
using Microsoft.AspNetCore.Mvc;

namespace CryptoNest.Modules.CryptoListing.Api.Controllers;

[Route(CryptoListingModule.BasePath + "/crypto-currencies-archives")]
internal class CryptoCurrencyArchiveController : BaseController
{
    private readonly IQueryDispatcher queryDispatcher;

    public CryptoCurrencyArchiveController(IQueryDispatcher queryDispatcher)
    {
        this.queryDispatcher = queryDispatcher;
    }

    [HttpGet("currency")]
    public async Task<ActionResult<CryptoCurrencyArchiveDto>> GetCryptoCurrencyArchive([FromQuery]GetCryptoCurrencyArchive archive)
        => OkOrNotFound(await queryDispatcher.QueryAsync(archive));
}