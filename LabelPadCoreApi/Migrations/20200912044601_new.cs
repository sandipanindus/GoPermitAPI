using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LabelPadCoreApi.Migrations
{
    public partial class @new : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "VisitorParkings");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "VisitorParkings");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "VisitorBayNos");

            migrationBuilder.DropColumn(
                name: "MaxVehiclesPerBay",
                table: "VisitorBayNos");

            migrationBuilder.DropColumn(
                name: "RegisterUserId",
                table: "VisitorBayNos");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "VisitorBayNos");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EndTime",
                table: "VisitorParkings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StartTime",
                table: "VisitorParkings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "VisitorBayNos",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxVehiclesPerBay",
                table: "VisitorBayNos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RegisterUserId",
                table: "VisitorBayNos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "VisitorBayNos",
                type: "datetime2",
                nullable: true);
        }
    }
}
