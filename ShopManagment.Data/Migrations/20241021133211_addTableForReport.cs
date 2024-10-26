using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopManagment.Data.Migrations
{
    /// <inheritdoc />
    public partial class addTableForReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuantitySold = table.Column<int>(type: "int", nullable: false),
                    QuantityPurchased = table.Column<int>(type: "int", nullable: false),
                    TotalSalse = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPurchases = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Revenue = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reports");
        }
    }
}
