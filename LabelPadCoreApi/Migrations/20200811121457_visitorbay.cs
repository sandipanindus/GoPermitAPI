using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LabelPadCoreApi.Migrations
{
    public partial class visitorbay : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SectionsOrFloors",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "Seperator",
                table: "Sites");

            migrationBuilder.AddColumn<int>(
                name: "ParkingBaySectionsOrFloors",
                table: "Sites",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ParkingBaySeperator",
                table: "Sites",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VisitorSectionsOrFloors",
                table: "Sites",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "VisitorSeperator",
                table: "Sites",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "VisitorBays",
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
                    Prefix = table.Column<string>(nullable: true),
                    From = table.Column<int>(nullable: false),
                    To = table.Column<int>(nullable: false),
                    count = table.Column<int>(nullable: false),
                    SiteId = table.Column<int>(nullable: false),
                    Section = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitorBays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VisitorBays_Sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Sites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VisitorBayNos",
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
                    VisitorBayId = table.Column<int>(nullable: false),
                    RegisterUserId = table.Column<int>(nullable: false),
                    SiteId = table.Column<int>(nullable: false),
                    Section = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitorBayNos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VisitorBayNos_RegisterUsers_RegisterUserId",
                        column: x => x.RegisterUserId,
                        principalTable: "RegisterUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VisitorBayNos_VisitorBays_VisitorBayId",
                        column: x => x.VisitorBayId,
                        principalTable: "VisitorBays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VisitorBayNos_RegisterUserId",
                table: "VisitorBayNos",
                column: "RegisterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitorBayNos_VisitorBayId",
                table: "VisitorBayNos",
                column: "VisitorBayId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitorBays_SiteId",
                table: "VisitorBays",
                column: "SiteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VisitorBayNos");

            migrationBuilder.DropTable(
                name: "VisitorBays");

            migrationBuilder.DropColumn(
                name: "ParkingBaySectionsOrFloors",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "ParkingBaySeperator",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "VisitorSectionsOrFloors",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "VisitorSeperator",
                table: "Sites");

            migrationBuilder.AddColumn<int>(
                name: "SectionsOrFloors",
                table: "Sites",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Seperator",
                table: "Sites",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
