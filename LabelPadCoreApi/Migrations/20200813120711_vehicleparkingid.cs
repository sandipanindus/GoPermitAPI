using Microsoft.EntityFrameworkCore.Migrations;

namespace LabelPadCoreApi.Migrations
{
    public partial class vehicleparkingid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VisitorParkingVehicleDetails_VisitorParkings_VisitorParkingId",
                table: "VisitorParkingVehicleDetails");

            migrationBuilder.DropIndex(
                name: "IX_VisitorParkingVehicleDetails_VisitorParkingId",
                table: "VisitorParkingVehicleDetails");

            migrationBuilder.AlterColumn<int>(
                name: "VisitorParkingId",
                table: "VisitorParkingVehicleDetails",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "VisitorParkingId",
                table: "VisitorParkingVehicleDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_VisitorParkingVehicleDetails_VisitorParkingId",
                table: "VisitorParkingVehicleDetails",
                column: "VisitorParkingId");

            migrationBuilder.AddForeignKey(
                name: "FK_VisitorParkingVehicleDetails_VisitorParkings_VisitorParkingId",
                table: "VisitorParkingVehicleDetails",
                column: "VisitorParkingId",
                principalTable: "VisitorParkings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
