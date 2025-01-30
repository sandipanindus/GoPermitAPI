using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LabelPadCoreApi.Migrations
{
    public partial class startdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "VisitorBayNos",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "VisitorBayNos",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "ParkingBayNos",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "ParkingBayNos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "VisitorBayNos");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "VisitorBayNos");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "ParkingBayNos");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "ParkingBayNos");
        }
    }
}
