using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LabelPadCoreApi.Migrations
{
    public partial class visitorparkingtemp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VisitorParkingTemps",
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
                    SiteId = table.Column<int>(nullable: false),
                    RegisterUserId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    Surname = table.Column<string>(maxLength: 200, nullable: true),
                    Email = table.Column<string>(maxLength: 200, nullable: true),
                    MobileNumber = table.Column<string>(maxLength: 100, nullable: true),
                    Make = table.Column<string>(nullable: true),
                    Model = table.Column<string>(nullable: true),
                    VRMNumber = table.Column<string>(nullable: true),
                    VisitorBayNoId = table.Column<int>(nullable: false),
                    Duration = table.Column<string>(nullable: true),
                    SessionUnit = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    StartTime = table.Column<string>(nullable: true),
                    EndTime = table.Column<string>(nullable: true),
                    CCtome = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitorParkingTemps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VisitorParkingTemps_Sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Sites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VisitorParkingTemps_SiteId",
                table: "VisitorParkingTemps",
                column: "SiteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VisitorParkingTemps");
        }
    }
}
