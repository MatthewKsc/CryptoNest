using System.Threading.Tasks;
using CryptoNest.Modules.CryptoListing.Application.DTO;
using CryptoNest.Modules.CryptoListing.Application.Exceptions;
using CryptoNest.Modules.CryptoListing.Application.Queries;
using CryptoNest.Modules.CryptoListing.Domain.Entities;
using CryptoNest.Modules.CryptoListing.Infrastructure.Db;
using CryptoNest.Shared.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;

namespace CryptoNest.Modules.CryptoListing.Infrastructure.Handlers.Queries;

internal sealed class GetCryptoCurrencyHandler : IQueryHandler<GetCryptoCurrency, CryptoCurrencyDto>
{
    private readonly DbSet<CryptoCurrency> cryptoCurrencies;

    public GetCryptoCurrencyHandler(CryptoListingDbContext dbContext)
    {
        this.cryptoCurrencies = dbContext.CryptoCurrencies;
    }
    
    public async Task<CryptoCurrencyDto> HandleAsync(GetCryptoCurrency query)
    {
        if (string.IsNullOrWhiteSpace(query.Symbol))
        {
            throw new GetCryptoCurrencyException("Crypto currency symbol is not provided");
        }

        CryptoCurrency cryptoCurrency = await cryptoCurrencies
            .SingleOrDefaultAsync(currency => currency.Symbol == query.Symbol);

        return new CryptoCurrencyDto
        {
            CurrencyName = cryptoCurrency.CurrencyName,
            Symbol = cryptoCurrency.Symbol,
            Slug = cryptoCurrency.Slug,
            MarketRank = cryptoCurrency.MarketRank,
            MarketPrice = cryptoCurrency.MarketPrice,
            TimeOfRecord = cryptoCurrency.TimeOfRecord,
        };
    }
}