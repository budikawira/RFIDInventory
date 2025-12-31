using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RfidBarcode.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _005_Role : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsReturn",
                table: "SuratJalans",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AccessMenus",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Description", "LastUpdateBy", "LastUpdateDate" },
                values: new object[,]
                {
                    { "CI", "system", new DateTime(2025, 12, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Buat Surat Jalan Inbound", "system", new DateTime(2025, 12, 30, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { "CO", "system", new DateTime(2025, 12, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Buat Surat Jalan Outbond", "system", new DateTime(2025, 12, 30, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { "FI", "system", new DateTime(2025, 12, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Konfirmasi Surat Jalan Inbound", "system", new DateTime(2025, 12, 30, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { "FO", "system", new DateTime(2025, 12, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Konfirmasi Surat Jalan Outbond", "system", new DateTime(2025, 12, 30, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { "FOR", "system", new DateTime(2025, 12, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Konfirmasi Surat Jalan Outbond Retur", "system", new DateTime(2025, 12, 30, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AccessMenus",
                keyColumn: "Id",
                keyValue: "CI");

            migrationBuilder.DeleteData(
                table: "AccessMenus",
                keyColumn: "Id",
                keyValue: "CO");

            migrationBuilder.DeleteData(
                table: "AccessMenus",
                keyColumn: "Id",
                keyValue: "FI");

            migrationBuilder.DeleteData(
                table: "AccessMenus",
                keyColumn: "Id",
                keyValue: "FO");

            migrationBuilder.DeleteData(
                table: "AccessMenus",
                keyColumn: "Id",
                keyValue: "FOR");

            migrationBuilder.DropColumn(
                name: "IsReturn",
                table: "SuratJalans");
        }
    }
}
