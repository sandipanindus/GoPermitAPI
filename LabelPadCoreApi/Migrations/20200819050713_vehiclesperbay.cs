using Microsoft.EntityFrameworkCore.Migrations;

namespace LabelPadCoreApi.Migrations
{
    public partial class vehiclesperbay : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VehiclesPerBay",
                table: "Sites",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VehiclesPerBay",
                table: "Sites");
        }
    }
}
