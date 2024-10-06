using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Netherite.API.Migrations
{
    /// <inheritdoc />
    public partial class Name_to_symbol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "CurrencyPairs");

            migrationBuilder.DropColumn(
                name: "NameTwo",
                table: "CurrencyPairs");

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                table: "CurrencyPairs",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Symbol",
                table: "CurrencyPairs",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SymbolTwo",
                table: "CurrencyPairs",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Symbol",
                table: "CurrencyPairs");

            migrationBuilder.DropColumn(
                name: "SymbolTwo",
                table: "CurrencyPairs");

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                table: "CurrencyPairs",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "CurrencyPairs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameTwo",
                table: "CurrencyPairs",
                type: "text",
                nullable: true);
        }
    }
}
