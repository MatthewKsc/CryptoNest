using System;
using AutoMapper;
using CryptoNest.Modules.CryptoListing.Application.DTO;
using CryptoNest.Modules.CryptoListing.Domain.Entities;
using CryptoNest.Modules.CryptoListing.Domain.Events.External;

namespace CryptoNest.Modules.CryptoListing.Infrastructure.Mappings;

internal sealed class CryptoListingMappingProfile : Profile
{
    public CryptoListingMappingProfile()
    {
        CreateMap<CryptoCurrency, CryptoCurrencyDto>();
        CreateMap<CryptoCurrencyArchive, CryptoCurrencyArchiveDto>();

        CreateMap<CryptoCurrencyFetched, CryptoCurrency>()
            .ForMember(dest => dest.CurrencyName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.MarketRank, opt => opt.MapFrom(src => src.Rank))
            .ForMember(dest => dest.MarketPrice, opt => opt.MapFrom(src => Math.Round(src.Price, 18)))
            .ForMember(dest => dest.TimeOfRecord, opt => opt.MapFrom(src => DateTime.UtcNow));

        CreateMap<CryptoCurrency, CryptoCurrencyArchive>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.OldMarketPrice, opt => opt.MapFrom(src => src.MarketPrice));
    }
}