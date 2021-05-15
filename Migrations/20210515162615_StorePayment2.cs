using Microsoft.EntityFrameworkCore.Migrations;

namespace Ecommerce_NetCore_API.Migrations
{
    public partial class StorePayment2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "paiddate",
                table: "customertxhistory",
                newName: "Paiddate");

            migrationBuilder.AddColumn<string>(
                name: "Paymentmode",
                table: "customertxhistory",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Paymentmode",
                table: "customertxhistory");

            migrationBuilder.RenameColumn(
                name: "Paiddate",
                table: "customertxhistory",
                newName: "paiddate");
        }
    }
}
