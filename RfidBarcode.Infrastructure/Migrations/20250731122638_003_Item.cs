using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RfidBarcode.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _003_Item : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdentitasBenang",
                table: "Items",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "R",
                table: "Items",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdentitasBenang",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "R",
                table: "Items");
        }
    }
}
