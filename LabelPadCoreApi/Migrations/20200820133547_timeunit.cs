using Microsoft.EntityFrameworkCore.Migrations;

namespace LabelPadCoreApi.Migrations
{
    public partial class timeunit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxParkingSessionAllowed",
                table: "VisitorBays",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TimeUnit",
                table: "VisitorBays",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxParkingSessionAllowed",
                table: "VisitorBays");

            migrationBuilder.DropColumn(
                name: "TimeUnit",
                table: "VisitorBays");
        }
    }
}
