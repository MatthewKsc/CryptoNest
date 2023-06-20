using CryptoNest.Modules.CryptoListing.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryptoNest.Modules.CryptoListing.Infrastructure.Db;

internal class CryptoListingDbContext : DbContext
{
    public DbSet<CryptoCurrency> CryptoCurrencies { get; set; }
    public DbSet<CryptoCurrencyArchive> CryptoCurrencyArchives { get; set; }

    public CryptoListingDbContext(DbContextOptions<CryptoListingDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("cryptolisting");
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}