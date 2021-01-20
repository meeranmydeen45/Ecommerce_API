using Microsoft.EntityFrameworkCore.Migrations;

namespace Ecommerce_NetCore_API.Migrations
{
    public partial class TableRecreationF1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PorductName",
                table: "productWithCategoryIds");

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "productWithCategoryIds",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "productWithCategoryIds");

            migrationBuilder.AddColumn<string>(
                name: "PorductName",
                table: "productWithCategoryIds",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
