using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ecommerce_NetCore_API.Migrations
{
    public partial class BillandPDFversion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "billscollections",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Billnumber = table.Column<int>(nullable: false),
                    Billamount = table.Column<int>(nullable: false),
                    Deduction = table.Column<int>(nullable: false),
                    Payableamount = table.Column<int>(nullable: false),
                    Billbytearray = table.Column<byte[]>(nullable: true),
                    Billdate = table.Column<DateTime>(nullable: false),
                    Ispaid = table.Column<bool>(nullable: false),
                    Customerid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_billscollections", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "billscollections");
        }
    }
}
