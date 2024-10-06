using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Netherite.API.Migrations
{
    /// <inheritdoc />
    public partial class Moveinterestratetointerval : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InterestRate",
                table: "CurrencyPairs");

            migrationBuilder.AddColumn<decimal>(
                name: "InterestRate",
                table: "Intervals",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InterestRate",
                table: "Intervals");

            migrationBuilder.AddColumn<decimal>(
                name: "InterestRate",
                table: "CurrencyPairs",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
