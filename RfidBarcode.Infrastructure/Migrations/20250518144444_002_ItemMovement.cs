using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace RfidBarcode.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _002_ItemMovement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItemMovements",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ItemId = table.Column<long>(type: "bigint", nullable: false),
                    PrevLocationId = table.Column<long>(type: "bigint", nullable: true),
                    LocationId = table.Column<long>(type: "bigint", nullable: true),
                    PrevLocationName = table.Column<string>(type: "longtext", nullable: true),
                    LocationName = table.Column<string>(type: "longtext", nullable: true),
                    Note = table.Column<string>(type: "longtext", nullable: true),
                    Source = table.Column<string>(type: "longtext", nullable: true),
                    TagLocationId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastUpdateBy = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemMovements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemMovements_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemMovements_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ItemMovements_Locations_PrevLocationId",
                        column: x => x.PrevLocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ItemMovements_ItemId",
                table: "ItemMovements",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemMovements_LocationId",
                table: "ItemMovements",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemMovements_PrevLocationId",
                table: "ItemMovements",
                column: "PrevLocationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemMovements");
        }
    }
}
