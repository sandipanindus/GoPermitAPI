using Microsoft.EntityFrameworkCore.Migrations;

namespace LabelPadCoreApi.Migrations
{
    public partial class Mobilenumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address1",
                table: "RegisterUsers");

            migrationBuilder.DropColumn(
                name: "Address2",
                table: "RegisterUsers");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "RegisterUsers");

            migrationBuilder.DropColumn(
                name: "ContactNumber",
                table: "RegisterUsers");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "RegisterUsers");

            migrationBuilder.DropColumn(
                name: "OrganisationName",
                table: "RegisterUsers");

            migrationBuilder.DropColumn(
                name: "Subdomain",
                table: "RegisterUsers");

            migrationBuilder.AlterColumn<string>(
                name: "ZipCode",
                table: "RegisterUsers",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "RegisterUsers",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MobileNumber",
                table: "RegisterUsers",
                maxLength: 20,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "RegisterUsers");

            migrationBuilder.DropColumn(
                name: "MobileNumber",
                table: "RegisterUsers");

            migrationBuilder.AlterColumn<string>(
                name: "ZipCode",
                table: "RegisterUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address1",
                table: "RegisterUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address2",
                table: "RegisterUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "RegisterUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactNumber",
                table: "RegisterUsers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "RegisterUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OrganisationName",
                table: "RegisterUsers",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Subdomain",
                table: "RegisterUsers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }
    }
}
