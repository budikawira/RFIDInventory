using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace RfidBarcode.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _004_Location : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Locations_LocationId",
                table: "Items");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Locations",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Locations",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastUpdateBy",
                table: "Locations",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdateDate",
                table: "Locations",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Gates",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Gates",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastUpdateBy",
                table: "Gates",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdateDate",
                table: "Gates",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "GateMaps",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "GateMaps",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastUpdateBy",
                table: "GateMaps",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdateDate",
                table: "GateMaps",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TagLocations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Epc = table.Column<string>(type: "longtext", nullable: false),
                    ItemId = table.Column<long>(type: "bigint", nullable: true),
                    LocationId = table.Column<long>(type: "bigint", nullable: true),
                    StartScanned = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EndScanned = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastScanned = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastUpdateBy = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TagLocations_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TagLocations_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AccessMenuRoles",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2025, 3, 13, 13, 51, 10, 433, DateTimeKind.Local).AddTicks(1838), new DateTime(2025, 3, 13, 13, 51, 10, 433, DateTimeKind.Local).AddTicks(1842) });

            migrationBuilder.UpdateData(
                table: "AccessMenus",
                keyColumn: "Id",
                keyValue: "UM",
                columns: new[] { "CreatedDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2025, 3, 13, 13, 51, 10, 433, DateTimeKind.Local).AddTicks(1409), new DateTime(2025, 3, 13, 13, 51, 10, 433, DateTimeKind.Local).AddTicks(1442) });

            migrationBuilder.CreateIndex(
                name: "IX_TagLocations_ItemId",
                table: "TagLocations",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TagLocations_LocationId",
                table: "TagLocations",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Locations_LocationId",
                table: "Items",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Locations_LocationId",
                table: "Items");

            migrationBuilder.DropTable(
                name: "TagLocations");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "LastUpdateBy",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "LastUpdateDate",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Gates");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Gates");

            migrationBuilder.DropColumn(
                name: "LastUpdateBy",
                table: "Gates");

            migrationBuilder.DropColumn(
                name: "LastUpdateDate",
                table: "Gates");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "GateMaps");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "GateMaps");

            migrationBuilder.DropColumn(
                name: "LastUpdateBy",
                table: "GateMaps");

            migrationBuilder.DropColumn(
                name: "LastUpdateDate",
                table: "GateMaps");

            migrationBuilder.UpdateData(
                table: "AccessMenuRoles",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2025, 3, 12, 11, 14, 8, 970, DateTimeKind.Local).AddTicks(8046), new DateTime(2025, 3, 12, 11, 14, 8, 970, DateTimeKind.Local).AddTicks(8047) });

            migrationBuilder.UpdateData(
                table: "AccessMenus",
                keyColumn: "Id",
                keyValue: "UM",
                columns: new[] { "CreatedDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2025, 3, 12, 11, 14, 8, 970, DateTimeKind.Local).AddTicks(7847), new DateTime(2025, 3, 12, 11, 14, 8, 970, DateTimeKind.Local).AddTicks(7874) });

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Locations_LocationId",
                table: "Items",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id");
        }
    }
}
