using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LabelPadCoreApi.Migrations
{
    public partial class zatparkadd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSentToZatPark",
                table: "VehicleRegistrationTimeSlots",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Request",
                table: "VehicleRegistrationTimeSlots",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Response",
                table: "VehicleRegistrationTimeSlots",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SentToZatparkDateTime",
                table: "VehicleRegistrationTimeSlots",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ZatparkResponse",
                table: "VehicleRegistrationTimeSlots",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSentToZatPark",
                table: "VehicleRegistrations",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Request",
                table: "VehicleRegistrations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Response",
                table: "VehicleRegistrations",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SentToZatparkDateTime",
                table: "VehicleRegistrations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ZatparkResponse",
                table: "VehicleRegistrations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSentToZatPark",
                table: "VehicleRegistrationTimeSlots");

            migrationBuilder.DropColumn(
                name: "Request",
                table: "VehicleRegistrationTimeSlots");

            migrationBuilder.DropColumn(
                name: "Response",
                table: "VehicleRegistrationTimeSlots");

            migrationBuilder.DropColumn(
                name: "SentToZatparkDateTime",
                table: "VehicleRegistrationTimeSlots");

            migrationBuilder.DropColumn(
                name: "ZatparkResponse",
                table: "VehicleRegistrationTimeSlots");

            migrationBuilder.DropColumn(
                name: "IsSentToZatPark",
                table: "VehicleRegistrations");

            migrationBuilder.DropColumn(
                name: "Request",
                table: "VehicleRegistrations");

            migrationBuilder.DropColumn(
                name: "Response",
                table: "VehicleRegistrations");

            migrationBuilder.DropColumn(
                name: "SentToZatparkDateTime",
                table: "VehicleRegistrations");

            migrationBuilder.DropColumn(
                name: "ZatparkResponse",
                table: "VehicleRegistrations");
        }
    }
}
