using Microsoft.EntityFrameworkCore.Migrations;

namespace LabelPadCoreApi.Migrations
{
    public partial class endtimesession : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeSlot",
                table: "VisitorParkings");

            migrationBuilder.AddColumn<string>(
                name: "EndTime",
                table: "VisitorParkings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StartTime",
                table: "VisitorParkings",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VisitorBayNoId",
                table: "VisitorParkings",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "VisitorParkings");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "VisitorParkings");

            migrationBuilder.DropColumn(
                name: "VisitorBayNoId",
                table: "VisitorParkings");

            migrationBuilder.AddColumn<string>(
                name: "TimeSlot",
                table: "VisitorParkings",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
