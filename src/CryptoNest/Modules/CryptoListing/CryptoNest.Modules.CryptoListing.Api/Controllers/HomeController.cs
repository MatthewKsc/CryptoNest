using Microsoft.AspNetCore.Mvc;

namespace CryptoNest.Modules.CryptoListing.Api.Controllers;

[Route(CryptoListingModule.BasePath)]
internal class HomeController : BaseController
{
    [HttpGet]
    public ActionResult<string> Get() => "Crypto Listing API";
}