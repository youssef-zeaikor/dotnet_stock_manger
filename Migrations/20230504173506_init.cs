using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryM.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vendor",
                columns: table => new
                {
                    VendorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VendorName = table.Column<string>(type: "varchar(50)", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    ContactPerson = table.Column<string>(type: "varchar(50)", nullable: false),
                    Telephone = table.Column<string>(type: "varchar(10)", nullable: false),
                    Email = table.Column<string>(type: "varchar(50)", nullable: false),
                    Fax = table.Column<string>(type: "varchar(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendor", x => x.VendorId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vendor");
        }
    }
}
