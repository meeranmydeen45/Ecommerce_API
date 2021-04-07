using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ecommerce_NetCore_API.Migrations
{
    public partial class SaleTE : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CutomerId",
                table: "customers");

            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "customers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "saleswithCustomerIds",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Productname = table.Column<string>(nullable: true),
                    Prodsize = table.Column<string>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    Unitprice = table.Column<int>(nullable: false),
                    TotalCost = table.Column<int>(nullable: false),
                    Purchasedate = table.Column<DateTime>(nullable: false),
                    Productid = table.Column<int>(nullable: false),
                    Custid = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_saleswithCustomerIds", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "saleswithCustomerIds");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "customers");

            migrationBuilder.AddColumn<string>(
                name: "CutomerId",
                table: "customers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
