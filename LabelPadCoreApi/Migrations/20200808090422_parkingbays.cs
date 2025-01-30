using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LabelPadCoreApi.Migrations
{
    public partial class parkingbays : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NoOfTotalBay",
                table: "Sites",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SectionsOrFloors",
                table: "Sites",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Seperator",
                table: "Sites",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ParkingBays",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    Prefix = table.Column<string>(nullable: true),
                    From = table.Column<int>(nullable: false),
                    To = table.Column<int>(nullable: false),
                    count = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingBays", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParkingBays");

            migrationBuilder.DropColumn(
                name: "NoOfTotalBay",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "SectionsOrFloors",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "Seperator",
                table: "Sites");
        }
    }
}
