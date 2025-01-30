using Microsoft.EntityFrameworkCore.Migrations;

namespace LabelPadCoreApi.Migrations
{
    public partial class Label : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScreenUrl",
                table: "Screens");

            migrationBuilder.AddColumn<string>(
                name: "Label",
                table: "Screens",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "To",
                table: "Screens",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Label",
                table: "Modules",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "To",
                table: "Modules",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Label",
                table: "Screens");

            migrationBuilder.DropColumn(
                name: "To",
                table: "Screens");

            migrationBuilder.DropColumn(
                name: "Label",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "To",
                table: "Modules");

            migrationBuilder.AddColumn<string>(
                name: "ScreenUrl",
                table: "Screens",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }
    }
}
