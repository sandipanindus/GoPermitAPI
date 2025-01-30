using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LabelPadCoreApi.Migrations
{
    public partial class bayconfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BayConfigs",
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
                    ParkingBayNoId = table.Column<int>(nullable: false),
                    RegisterUserId = table.Column<int>(nullable: false),
                    SiteId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BayConfigs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BayConfigs_ParkingBayNos_ParkingBayNoId",
                        column: x => x.ParkingBayNoId,
                        principalTable: "ParkingBayNos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BayConfigs_RegisterUsers_RegisterUserId",
                        column: x => x.RegisterUserId,
                        principalTable: "RegisterUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BayConfigs_Sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Sites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BayConfigs_ParkingBayNoId",
                table: "BayConfigs",
                column: "ParkingBayNoId");

            migrationBuilder.CreateIndex(
                name: "IX_BayConfigs_RegisterUserId",
                table: "BayConfigs",
                column: "RegisterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BayConfigs_SiteId",
                table: "BayConfigs",
                column: "SiteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BayConfigs");
        }
    }
}
