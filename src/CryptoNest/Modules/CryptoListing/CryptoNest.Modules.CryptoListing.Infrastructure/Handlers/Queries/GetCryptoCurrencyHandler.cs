using System.Threading.Tasks;
using AutoMapper;
using CryptoNest.Modules.CryptoListing.Application.DTO;
using CryptoNest.Modules.CryptoListing.Application.Exceptions;
using CryptoNest.Modules.CryptoListing.Application.Queries;
using CryptoNest.Modules.CryptoListing.Domain.Entities;
using CryptoNest.Modules.CryptoListing.Domain.Repositories;
using CryptoNest.Shared.Abstractions.Queries;

namespace CryptoNest.Modules.CryptoListing.Infrastructure.Handlers.Queries;

internal sealed class GetCryptoCurrencyHandler : IQueryHandler<GetCryptoCurrency, CryptoCurrencyDto>
{
    private readonly IMapper mapper;
    private readonly ICryptoCurrencyRepository currencyRepository;

    public GetCryptoCurrencyHandler(IMapper mapper, ICryptoCurrencyRepository currencyRepository)
    {
        this.mapper = mapper;
        this.currencyRepository = currencyRepository;
    }
    
    public async Task<CryptoCurrencyDto> HandleAsync(GetCryptoCurrency query)
    {
        if (string.IsNullOrWhiteSpace(query.Symbol))
        {
            throw new CryptoCurrencySymbolEmptyException();
        }

        CryptoCurrency cryptoCurrency = await currencyRepository.GetBySymbolAsync(query.Symbol);

        return mapper.Map<CryptoCurrencyDto>(cryptoCurrency);
    }
}