using Microsoft.EntityFrameworkCore.Migrations;

namespace LabelPadCoreApi.Migrations
{
    public partial class parkingbaykey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ParkingBays_SiteId",
                table: "ParkingBays",
                column: "SiteId");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingBays_Sites_SiteId",
                table: "ParkingBays",
                column: "SiteId",
                principalTable: "Sites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkingBays_Sites_SiteId",
                table: "ParkingBays");

            migrationBuilder.DropIndex(
                name: "IX_ParkingBays_SiteId",
                table: "ParkingBays");
        }
    }
}
