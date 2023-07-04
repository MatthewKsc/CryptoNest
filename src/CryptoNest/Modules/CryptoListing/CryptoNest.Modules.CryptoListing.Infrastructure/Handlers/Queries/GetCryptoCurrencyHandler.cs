using System.Threading.Tasks;
using AutoMapper;
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
    private readonly IMapper mapper;
    private readonly DbSet<CryptoCurrency> cryptoCurrencies;

    public GetCryptoCurrencyHandler(IMapper mapper, CryptoListingDbContext dbContext)
    {
        this.mapper = mapper;
        this.cryptoCurrencies = dbContext.CryptoCurrencies;
    }
    
    public async Task<CryptoCurrencyDto> HandleAsync(GetCryptoCurrency query)
    {
        if (string.IsNullOrWhiteSpace(query.Symbol))
        {
            throw new CryptoCurrencySymbolEmptyException();
        }

        CryptoCurrency cryptoCurrency = await cryptoCurrencies
            .SingleOrDefaultAsync(currency => currency.Symbol == query.Symbol);

        if (cryptoCurrency is null)
        {
            return null;
        }

        return mapper.Map<CryptoCurrencyDto>(cryptoCurrency);
    }
}