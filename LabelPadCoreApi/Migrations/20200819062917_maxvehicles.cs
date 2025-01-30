using Microsoft.EntityFrameworkCore.Migrations;

namespace LabelPadCoreApi.Migrations
{
    public partial class maxvehicles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VehiclesPerBay",
                table: "VisitorBayNos");

            migrationBuilder.DropColumn(
                name: "VehiclesPerBay",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "VehiclesPerBay",
                table: "ParkingBayNos");

            migrationBuilder.AddColumn<int>(
                name: "MaxVehiclesPerBay",
                table: "VisitorBayNos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxVehiclesPerBay",
                table: "Sites",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxVehiclesPerBay",
                table: "ParkingBayNos",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxVehiclesPerBay",
                table: "VisitorBayNos");

            migrationBuilder.DropColumn(
                name: "MaxVehiclesPerBay",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "MaxVehiclesPerBay",
                table: "ParkingBayNos");

            migrationBuilder.AddColumn<int>(
                name: "VehiclesPerBay",
                table: "VisitorBayNos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VehiclesPerBay",
                table: "Sites",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VehiclesPerBay",
                table: "ParkingBayNos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
