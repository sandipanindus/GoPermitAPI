using Microsoft.EntityFrameworkCore.Migrations;

namespace LabelPadCoreApi.Migrations
{
    public partial class parkingavaible : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ManageParkingAvailble",
                table: "Sites",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "visitorParkingAvailble",
                table: "Sites",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ManageParkingAvailble",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "visitorParkingAvailble",
                table: "Sites");
        }
    }
}
