using Microsoft.EntityFrameworkCore.Migrations;

namespace LabelPadCoreApi.Migrations
{
    public partial class filename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "DatasetFiles",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "DatasetFiles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "DatasetFiles");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "DatasetFiles");
        }
    }
}
