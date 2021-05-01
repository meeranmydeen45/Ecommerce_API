using Microsoft.EntityFrameworkCore.Migrations;

namespace Ecommerce_NetCore_API.Migrations
{
    public partial class BillProfitField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Billprofit",
                table: "billscollections",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Billprofit",
                table: "billscollections");
        }
    }
}
