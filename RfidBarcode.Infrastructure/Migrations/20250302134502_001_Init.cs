using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RfidBarcode.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _001_Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AccessMenus",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastUpdateBy = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessMenus", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: true),
                    SecurityStamp = table.Column<string>(type: "longtext", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true),
                    PhoneNumber = table.Column<string>(type: "longtext", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastUpdateBy = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TrackingItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Merk = table.Column<string>(type: "longtext", nullable: true),
                    Kp = table.Column<string>(type: "longtext", nullable: true),
                    Ib = table.Column<string>(type: "longtext", nullable: true),
                    Kode = table.Column<string>(type: "longtext", nullable: true),
                    Kode1 = table.Column<string>(type: "longtext", nullable: true),
                    Kode2 = table.Column<string>(type: "longtext", nullable: true),
                    Kode3 = table.Column<string>(type: "longtext", nullable: true),
                    Kode4 = table.Column<string>(type: "longtext", nullable: true),
                    Oz = table.Column<string>(type: "longtext", nullable: true),
                    Grade = table.Column<string>(type: "longtext", nullable: true),
                    Point = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Yard = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Kg = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Lebar = table.Column<double>(type: "double", nullable: true),
                    SusutLusi = table.Column<string>(type: "longtext", nullable: true),
                    SerialNumber = table.Column<string>(type: "longtext", nullable: true),
                    Inisial = table.Column<string>(type: "longtext", nullable: true),
                    EncodeTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    TrolleyId = table.Column<long>(type: "bigint", nullable: true),
                    MeterWeaving = table.Column<float>(type: "float", nullable: true),
                    MeterGreige = table.Column<float>(type: "float", nullable: true),
                    MeterBBSF = table.Column<float>(type: "float", nullable: true),
                    WeavingMachineNo = table.Column<string>(type: "longtext", nullable: true),
                    ProductionDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FormId = table.Column<string>(type: "longtext", nullable: true),
                    NoBeamIndigo = table.Column<string>(type: "longtext", nullable: true),
                    StockOutDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ImportTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    StartProcess = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    EndProcess = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastUpdateBy = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackingItems", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AccessMenuRoles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    AccessMenuId = table.Column<string>(type: "varchar(255)", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastUpdateBy = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessMenuRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccessMenuRoles_AccessMenus_AccessMenuId",
                        column: x => x.AccessMenuId,
                        principalTable: "AccessMenus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccessMenuRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    ClaimType = table.Column<string>(type: "longtext", nullable: true),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ClaimType = table.Column<string>(type: "longtext", nullable: true),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: true),
                    ApplicationUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "varchar(255)", nullable: false),
                    ProviderKey = table.Column<string>(type: "varchar(255)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "longtext", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ApplicationUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    LoginProvider = table.Column<string>(type: "varchar(255)", nullable: false),
                    Name = table.Column<string>(type: "varchar(255)", nullable: false),
                    Value = table.Column<string>(type: "longtext", nullable: true),
                    ApplicationUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Merk = table.Column<string>(type: "longtext", nullable: false),
                    Kp = table.Column<string>(type: "longtext", nullable: false),
                    Kode = table.Column<string>(type: "longtext", nullable: true),
                    Kode1 = table.Column<string>(type: "longtext", nullable: true),
                    Kode2 = table.Column<string>(type: "longtext", nullable: true),
                    Kode3 = table.Column<string>(type: "longtext", nullable: true),
                    Kode4 = table.Column<string>(type: "longtext", nullable: true),
                    Oz = table.Column<string>(type: "longtext", nullable: true),
                    Grade = table.Column<string>(type: "longtext", nullable: true),
                    Point = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Yard = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Kg = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Lebar = table.Column<string>(type: "longtext", nullable: true),
                    K = table.Column<string>(type: "longtext", nullable: true),
                    SusutLusi = table.Column<string>(type: "longtext", nullable: true),
                    SerialNumber = table.Column<string>(type: "longtext", nullable: true),
                    K3l = table.Column<string>(type: "longtext", nullable: true),
                    Inisial = table.Column<string>(type: "longtext", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    SuratJalanId = table.Column<long>(type: "bigint", nullable: true),
                    QcFinishUserId = table.Column<long>(type: "bigint", nullable: true),
                    QcFinish = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    TanggalBuatBarcode = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SuratJalanP1Id = table.Column<long>(type: "bigint", nullable: true),
                    ScanP1UserId = table.Column<long>(type: "bigint", nullable: true),
                    ScanP1 = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    TrackingItemId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastUpdateBy = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_TrackingItems_TrackingItemId",
                        column: x => x.TrackingItemId,
                        principalTable: "TrackingItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ItemPrintLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ItemId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastUpdateBy = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemPrintLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemPrintLogs_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.InsertData(
                table: "AccessMenus",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Description", "LastUpdateBy", "LastUpdateDate" },
                values: new object[] { "UM", "system", new DateTime(2025, 3, 2, 20, 45, 1, 822, DateTimeKind.Local).AddTicks(2880), "User Management", "system", new DateTime(2025, 3, 2, 20, 45, 1, 822, DateTimeKind.Local).AddTicks(2914) });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { 1L, null, "Administrator", "ADMINISTRATOR" },
                    { 2L, null, "Admin Finish", "ADMIN FINISH" },
                    { 3L, null, "QC Finish", "QC FINISH" },
                    { 4L, null, "Gudang Kain", "GUDANG KAIN" },
                    { 5L, null, "QC Gudang Kain", "QC GUDANG KAIN" },
                    { 6L, null, "Admin Gudang Kain", "ADMIN GUDANG KAIN" },
                    { 7L, null, "Gudang Jakarta", "GUDANG JAKARTA" },
                    { 8L, null, "Admin Gudang Jakarta", "ADMIN GUDANG JAKARTA" }
                });

            migrationBuilder.InsertData(
                table: "AccessMenuRoles",
                columns: new[] { "Id", "AccessMenuId", "CreatedBy", "CreatedDate", "LastUpdateBy", "LastUpdateDate", "RoleId" },
                values: new object[] { 1L, "UM", "system", new DateTime(2025, 3, 2, 20, 45, 1, 822, DateTimeKind.Local).AddTicks(3056), "system", new DateTime(2025, 3, 2, 20, 45, 1, 822, DateTimeKind.Local).AddTicks(3057), 1L });

            migrationBuilder.CreateIndex(
                name: "IX_AccessMenuRoles_AccessMenuId",
                table: "AccessMenuRoles",
                column: "AccessMenuId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessMenuRoles_RoleId",
                table: "AccessMenuRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_ApplicationUserId",
                table: "AspNetUserClaims",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_ApplicationUserId",
                table: "AspNetUserLogins",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserTokens_ApplicationUserId",
                table: "AspNetUserTokens",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemPrintLogs_ItemId",
                table: "ItemPrintLogs",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_TrackingItemId",
                table: "Items",
                column: "TrackingItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessMenuRoles");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ItemPrintLogs");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropTable(
                name: "AccessMenus");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "TrackingItems");
        }
    }
}
