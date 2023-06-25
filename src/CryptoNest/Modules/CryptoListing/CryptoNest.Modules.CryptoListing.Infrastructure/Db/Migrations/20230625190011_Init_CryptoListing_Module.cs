using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoNest.Modules.CryptoListing.Infrastructure.Db.Migrations
{
    /// <inheritdoc />
    public partial class Init_CryptoListing_Module : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "cryptolisting");

            migrationBuilder.CreateTable(
                name: "CryptoCurrencies",
                schema: "cryptolisting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrencyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MarketRank = table.Column<int>(type: "int", nullable: false),
                    MarketPrice = table.Column<decimal>(type: "decimal(38,19)", nullable: false),
                    TimeOfRecord = table.Column<DateTime>(type: "datetime2(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CryptoCurrencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CryptoCurrencyArchives",
                schema: "cryptolisting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrencyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OldMarketPrice = table.Column<decimal>(type: "decimal(38,19)", nullable: false),
                    TimeOfRecord = table.Column<DateTime>(type: "datetime2(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CryptoCurrencyArchives", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CryptoCurrencies_Symbol",
                schema: "cryptolisting",
                table: "CryptoCurrencies",
                column: "Symbol",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CryptoCurrencies",
                schema: "cryptolisting");

            migrationBuilder.DropTable(
                name: "CryptoCurrencyArchives",
                schema: "cryptolisting");
        }
    }
}
