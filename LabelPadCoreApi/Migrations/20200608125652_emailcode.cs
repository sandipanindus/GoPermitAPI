using Microsoft.EntityFrameworkCore.Migrations;

namespace LabelPadCoreApi.Migrations
{
    public partial class emailcode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailCode",
                table: "RegisterUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "RegisterUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailCode",
                table: "RegisterUsers");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "RegisterUsers");
        }
    }
}
