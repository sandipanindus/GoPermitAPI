using Microsoft.EntityFrameworkCore.Migrations;

namespace LabelPadCoreApi.Migrations
{
    public partial class add3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EndTime",
                table: "VisitorParkings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StartTime",
                table: "VisitorParkings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "VisitorParkings");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "VisitorParkings");
        }
    }
}
