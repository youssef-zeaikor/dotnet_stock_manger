using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryM.Migrations
{
    /// <inheritdoc />
    public partial class init_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "category",
                table: "Categories",
                newName: "Category_Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Category_Name",
                table: "Categories",
                newName: "category");
        }
    }
}
