using Microsoft.EntityFrameworkCore.Migrations;

namespace LabelPadCoreApi.Migrations
{
    public partial class cctome : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CCtome",
                table: "VisitorParkings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Surname",
                table: "VisitorParkings",
                maxLength: 200,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CCtome",
                table: "VisitorParkings");

            migrationBuilder.DropColumn(
                name: "Surname",
                table: "VisitorParkings");
        }
    }
}
