using Microsoft.EntityFrameworkCore.Migrations;

namespace LabelPadCoreApi.Migrations
{
    public partial class keyvisitor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VisitorBayNos_RegisterUsers_RegisterUserId",
                table: "VisitorBayNos");

            migrationBuilder.DropIndex(
                name: "IX_VisitorBayNos_RegisterUserId",
                table: "VisitorBayNos");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_VisitorBayNos_RegisterUserId",
                table: "VisitorBayNos",
                column: "RegisterUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_VisitorBayNos_RegisterUsers_RegisterUserId",
                table: "VisitorBayNos",
                column: "RegisterUserId",
                principalTable: "RegisterUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
