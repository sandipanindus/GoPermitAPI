using Microsoft.EntityFrameworkCore.Migrations;

namespace LabelPadCoreApi.Migrations
{
    public partial class issavecount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IsSaveCount",
                table: "VehicleRegistrationTimeSlots",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IsSaveCount",
                table: "VehicleRegistrations",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSaveCount",
                table: "VehicleRegistrationTimeSlots");

            migrationBuilder.DropColumn(
                name: "IsSaveCount",
                table: "VehicleRegistrations");
        }
    }
}
