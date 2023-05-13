using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryM.Migrations
{
    /// <inheritdoc />
    public partial class init_4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Stockins",
                columns: table => new
                {
                    StockInId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Reference = table.Column<string>(type: "varchar(50)", nullable: false),
                    Quantite = table.Column<string>(type: "varchar(50)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    stockeby = table.Column<string>(type: "varchar(50)", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", nullable: true),
                    VendorId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stockins", x => x.StockInId);
                    table.ForeignKey(
                        name: "FK_Stockins_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Stockins_Vendor_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendor",
                        principalColumn: "VendorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stockins_ProductId",
                table: "Stockins",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Stockins_VendorId",
                table: "Stockins",
                column: "VendorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Stockins");
        }
    }
}
