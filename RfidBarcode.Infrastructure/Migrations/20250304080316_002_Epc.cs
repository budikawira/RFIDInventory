using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RfidBarcode.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _002_Epc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Epc",
                table: "Items",
                type: "longtext",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AccessMenuRoles",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2025, 3, 4, 15, 3, 15, 786, DateTimeKind.Local).AddTicks(3323), new DateTime(2025, 3, 4, 15, 3, 15, 786, DateTimeKind.Local).AddTicks(3324) });

            migrationBuilder.UpdateData(
                table: "AccessMenus",
                keyColumn: "Id",
                keyValue: "UM",
                columns: new[] { "CreatedDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2025, 3, 4, 15, 3, 15, 786, DateTimeKind.Local).AddTicks(3130), new DateTime(2025, 3, 4, 15, 3, 15, 786, DateTimeKind.Local).AddTicks(3151) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Epc",
                table: "Items");

            migrationBuilder.UpdateData(
                table: "AccessMenuRoles",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2025, 3, 2, 20, 45, 1, 822, DateTimeKind.Local).AddTicks(3056), new DateTime(2025, 3, 2, 20, 45, 1, 822, DateTimeKind.Local).AddTicks(3057) });

            migrationBuilder.UpdateData(
                table: "AccessMenus",
                keyColumn: "Id",
                keyValue: "UM",
                columns: new[] { "CreatedDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2025, 3, 2, 20, 45, 1, 822, DateTimeKind.Local).AddTicks(2880), new DateTime(2025, 3, 2, 20, 45, 1, 822, DateTimeKind.Local).AddTicks(2914) });
        }
    }
}
