using Microsoft.EntityFrameworkCore.Migrations;

namespace LabelPadCoreApi.Migrations
{
    public partial class vrm1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "VisitorParkings");

            migrationBuilder.DropColumn(
                name: "City",
                table: "VisitorParkings");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "VisitorParkings");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "VisitorParkings");

            migrationBuilder.DropColumn(
                name: "State",
                table: "VisitorParkings");

            migrationBuilder.DropColumn(
                name: "Zipcode",
                table: "VisitorParkings");

            migrationBuilder.AddColumn<string>(
                name: "Duration",
                table: "VisitorParkings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Make",
                table: "VisitorParkings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Model",
                table: "VisitorParkings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "VisitorParkings",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TimeSlot",
                table: "VisitorParkings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VRMNumber",
                table: "VisitorParkings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "VisitorParkings");

            migrationBuilder.DropColumn(
                name: "Make",
                table: "VisitorParkings");

            migrationBuilder.DropColumn(
                name: "Model",
                table: "VisitorParkings");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "VisitorParkings");

            migrationBuilder.DropColumn(
                name: "TimeSlot",
                table: "VisitorParkings");

            migrationBuilder.DropColumn(
                name: "VRMNumber",
                table: "VisitorParkings");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "VisitorParkings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "VisitorParkings",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "VisitorParkings",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "VisitorParkings",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "VisitorParkings",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Zipcode",
                table: "VisitorParkings",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
