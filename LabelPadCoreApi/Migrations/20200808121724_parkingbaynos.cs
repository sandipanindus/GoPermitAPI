using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LabelPadCoreApi.Migrations
{
    public partial class parkingbaynos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SiteId",
                table: "ParkingBays",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ParkingBayNos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    BayName = table.Column<string>(nullable: true),
                    ParkingBayId = table.Column<int>(nullable: false),
                    SiteId = table.Column<int>(nullable: false),
                    Section = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingBayNos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParkingBayNos_ParkingBays_ParkingBayId",
                        column: x => x.ParkingBayId,
                        principalTable: "ParkingBays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParkingBayNos_ParkingBayId",
                table: "ParkingBayNos",
                column: "ParkingBayId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParkingBayNos");

            migrationBuilder.DropColumn(
                name: "SiteId",
                table: "ParkingBays");
        }
    }
}
