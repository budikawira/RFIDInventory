using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RfidBarcode.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _003_AccessMenu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AccessMenus",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Description", "LastUpdateBy", "LastUpdateDate" },
                values: new object[] { "RM", "system", new DateTime(2025, 12, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Role Management", "system", new DateTime(2025, 12, 30, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "AccessMenuRoles",
                columns: new[] { "Id", "AccessMenuId", "CreatedBy", "CreatedDate", "LastUpdateBy", "LastUpdateDate", "RoleId" },
                values: new object[] { 2L, "RM", "system", new DateTime(2025, 12, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", new DateTime(2025, 12, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 1L });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AccessMenuRoles",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "AccessMenus",
                keyColumn: "Id",
                keyValue: "RM");
        }
    }
}
