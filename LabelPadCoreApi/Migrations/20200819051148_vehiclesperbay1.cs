using Microsoft.EntityFrameworkCore.Migrations;

namespace LabelPadCoreApi.Migrations
{
    public partial class vehiclesperbay1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VehiclesPerBay",
                table: "VisitorBayNos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VehiclesPerBay",
                table: "ParkingBayNos",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VehiclesPerBay",
                table: "VisitorBayNos");

            migrationBuilder.DropColumn(
                name: "VehiclesPerBay",
                table: "ParkingBayNos");
        }
    }
}
