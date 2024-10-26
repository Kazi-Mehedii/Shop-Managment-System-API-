using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopManagment.Data.Migrations
{
    /// <inheritdoc />
    public partial class addPropertiesonsaleModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "Sale",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Sale");
        }
    }
}
