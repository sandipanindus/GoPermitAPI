using Microsoft.EntityFrameworkCore.Migrations;

namespace LabelPadCoreApi.Migrations
{
    public partial class vrm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoomNo",
                table: "VehicleRegistrations");

            migrationBuilder.AddColumn<string>(
                name: "VRM",
                table: "VehicleRegistrations",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VRM",
                table: "VehicleRegistrations");

            migrationBuilder.AddColumn<string>(
                name: "RoomNo",
                table: "VehicleRegistrations",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
