using Microsoft.EntityFrameworkCore.Migrations;

namespace LabelPadCoreApi.Migrations
{
    public partial class client : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address1",
                table: "RegisterUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address2",
                table: "RegisterUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "RegisterUsers",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "RegisterUsers",
                maxLength: 100,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "RegisterUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "RegisterUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "RegisterUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "RegisterUsers",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ZipCode",
                table: "RegisterUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address1",
                table: "RegisterUsers");

            migrationBuilder.DropColumn(
                name: "Address2",
                table: "RegisterUsers");

            migrationBuilder.DropColumn(
                name: "City",
                table: "RegisterUsers");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "RegisterUsers");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "RegisterUsers");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "RegisterUsers");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "RegisterUsers");

            migrationBuilder.DropColumn(
                name: "State",
                table: "RegisterUsers");

            migrationBuilder.DropColumn(
                name: "ZipCode",
                table: "RegisterUsers");
        }
    }
}
