using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ecommerce_NetCore_API.Migrations
{
    public partial class txhistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "billspending",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pendingamount = table.Column<int>(nullable: false),
                    Billnumber = table.Column<int>(nullable: false),
                    Iscompleted = table.Column<bool>(nullable: false),
                    Customerid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_billspending", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "customertxhistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Paidamount = table.Column<int>(nullable: false),
                    Billnumber = table.Column<int>(nullable: false),
                    paiddate = table.Column<DateTime>(nullable: false),
                    Customerid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customertxhistory", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "billspending");

            migrationBuilder.DropTable(
                name: "customertxhistory");
        }
    }
}
