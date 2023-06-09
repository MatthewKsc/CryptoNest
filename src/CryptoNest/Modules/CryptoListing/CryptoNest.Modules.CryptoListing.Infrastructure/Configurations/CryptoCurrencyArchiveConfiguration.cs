﻿using CryptoNest.Modules.CryptoListing.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoNest.Modules.CryptoListing.Infrastructure.Configurations;

public class CryptoCurrencyArchiveConfiguration : IEntityTypeConfiguration<CryptoCurrencyArchive>
{
    public void Configure(EntityTypeBuilder<CryptoCurrencyArchive> builder)
    {
        builder.HasKey(currencyArchive => currencyArchive.Id);

        builder.Property(currencyArchive => currencyArchive.Symbol)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(currencyArchive => currencyArchive.OldMarketPrice)
            .IsRequired()
            .HasColumnType("decimal(38,19)");
        
        builder.Property(currencyArchive => currencyArchive.Symbol)
            .IsRequired();
        
        builder.Property(b => b.TimeOfRecord)
            .IsRequired()
            .HasColumnType("datetime2(7)");
    }
}