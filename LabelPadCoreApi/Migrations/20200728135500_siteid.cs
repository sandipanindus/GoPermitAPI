using Microsoft.EntityFrameworkCore.Migrations;

namespace LabelPadCoreApi.Migrations
{
    public partial class siteid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ParkingBay",
                table: "RegisterUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SiteId",
                table: "RegisterUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParkingBay",
                table: "RegisterUsers");

            migrationBuilder.DropColumn(
                name: "SiteId",
                table: "RegisterUsers");
        }
    }
}
