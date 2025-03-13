using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace RfidBarcode.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _003_Location : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "LocationId",
                table: "Items",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Qr",
                table: "Items",
                type: "longtext",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Gates",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    ClientId = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gates", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true),
                    Type = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    SkipStockOpname = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "GateMaps",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    GateId = table.Column<long>(type: "bigint", nullable: false),
                    Antenna = table.Column<string>(type: "longtext", nullable: true),
                    PrevLocationId = table.Column<long>(type: "bigint", nullable: true),
                    NextLocationId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GateMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GateMaps_Gates_GateId",
                        column: x => x.GateId,
                        principalTable: "Gates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GateMaps_Locations_NextLocationId",
                        column: x => x.NextLocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GateMaps_Locations_PrevLocationId",
                        column: x => x.PrevLocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

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

            migrationBuilder.CreateIndex(
                name: "IX_Items_LocationId",
                table: "Items",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_GateMaps_GateId",
                table: "GateMaps",
                column: "GateId");

            migrationBuilder.CreateIndex(
                name: "IX_GateMaps_NextLocationId",
                table: "GateMaps",
                column: "NextLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_GateMaps_PrevLocationId",
                table: "GateMaps",
                column: "PrevLocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Locations_LocationId",
                table: "Items",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Locations_LocationId",
                table: "Items");

            migrationBuilder.DropTable(
                name: "GateMaps");

            migrationBuilder.DropTable(
                name: "Gates");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Items_LocationId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Qr",
                table: "Items");

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
    }
}
