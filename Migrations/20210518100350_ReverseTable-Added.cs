using Microsoft.EntityFrameworkCore.Migrations;

namespace Ecommerce_NetCore_API.Migrations
{
    public partial class ReverseTableAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Isbillmodified",
                table: "billscollections",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "reverseentrydata",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Billnumber = table.Column<string>(nullable: true),
                    Productid = table.Column<int>(nullable: false),
                    Size = table.Column<string>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    Saleprice = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reverseentrydata", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "reverseentrydata");

            migrationBuilder.DropColumn(
                name: "Isbillmodified",
                table: "billscollections");
        }
    }
}
