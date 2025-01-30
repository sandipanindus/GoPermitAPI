using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LabelPadCoreApi.Migrations
{
    public partial class fromdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FromDate",
                table: "VehicleRegistrationTimeSlots",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ToDate",
                table: "VehicleRegistrationTimeSlots",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "VehicleRegistrationId",
                table: "VehicleRegistrationTimeSlots",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromDate",
                table: "VehicleRegistrationTimeSlots");

            migrationBuilder.DropColumn(
                name: "ToDate",
                table: "VehicleRegistrationTimeSlots");

            migrationBuilder.DropColumn(
                name: "VehicleRegistrationId",
                table: "VehicleRegistrationTimeSlots");
        }
    }
}
