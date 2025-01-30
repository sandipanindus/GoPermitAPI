using Microsoft.EntityFrameworkCore.Migrations;

namespace LabelPadCoreApi.Migrations
{
    public partial class parking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkingBayNos_ParkingBays_ParkingBayId",
                table: "ParkingBayNos");

            migrationBuilder.DropIndex(
                name: "IX_ParkingBayNos_ParkingBayId",
                table: "ParkingBayNos");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ParkingBayNos_ParkingBayId",
                table: "ParkingBayNos",
                column: "ParkingBayId");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingBayNos_ParkingBays_ParkingBayId",
                table: "ParkingBayNos",
                column: "ParkingBayId",
                principalTable: "ParkingBays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
