using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RfidBarcode.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _004_Role : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 8L);

            migrationBuilder.InsertData(
                table: "AccessMenus",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Description", "LastUpdateBy", "LastUpdateDate" },
                values: new object[,]
                {
                    { "IB", "system", new DateTime(2025, 12, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Input Barcode", "system", new DateTime(2025, 12, 30, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { "SJI", "system", new DateTime(2025, 12, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Surat Jalan Inbound", "system", new DateTime(2025, 12, 30, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { "SJO", "system", new DateTime(2025, 12, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Surat Jalan Outbond", "system", new DateTime(2025, 12, 30, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Adm Barcode", "ADM BARCODE" });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Adm Finish", "ADM FINISH" });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Adm Gudang", "ADM GUDANG" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AccessMenus",
                keyColumn: "Id",
                keyValue: "IB");

            migrationBuilder.DeleteData(
                table: "AccessMenus",
                keyColumn: "Id",
                keyValue: "SJI");

            migrationBuilder.DeleteData(
                table: "AccessMenus",
                keyColumn: "Id",
                keyValue: "SJO");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Admin Finish", "ADMIN FINISH" });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "QC Finish", "QC FINISH" });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Gudang Kain", "GUDANG KAIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { 5L, null, "QC Gudang Kain", "QC GUDANG KAIN" },
                    { 6L, null, "Admin Gudang Kain", "ADMIN GUDANG KAIN" },
                    { 7L, null, "Gudang Jakarta", "GUDANG JAKARTA" },
                    { 8L, null, "Admin Gudang Jakarta", "ADMIN GUDANG JAKARTA" }
                });
        }
    }
}
