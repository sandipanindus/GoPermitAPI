using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LabelPadCoreApi.Migrations
{
    public partial class visitorparking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VisitorParkings",
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
                    FirstName = table.Column<string>(maxLength: 200, nullable: true),
                    LastName = table.Column<string>(maxLength: 200, nullable: true),
                    Email = table.Column<string>(maxLength: 200, nullable: true),
                    MobileNumber = table.Column<string>(maxLength: 100, nullable: true),
                    Address = table.Column<string>(nullable: true),
                    City = table.Column<string>(maxLength: 200, nullable: true),
                    State = table.Column<string>(maxLength: 200, nullable: true),
                    Zipcode = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitorParkings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VisitorParkings_RegisterUsers_RegisterUserId",
                        column: x => x.RegisterUserId,
                        principalTable: "RegisterUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VisitorParkings_Sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Sites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VisitorParkingVehicleDetails",
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
                    Make = table.Column<string>(nullable: true),
                    Model = table.Column<string>(nullable: true),
                    VRMNumber = table.Column<string>(nullable: true),
                    VisitorBayNoId = table.Column<int>(nullable: false),
                    StartDate = table.Column<string>(nullable: true),
                    EndDate = table.Column<string>(nullable: true),
                    VisitorParkingId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitorParkingVehicleDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VisitorParkingVehicleDetails_VisitorBayNos_VisitorBayNoId",
                        column: x => x.VisitorBayNoId,
                        principalTable: "VisitorBayNos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VisitorParkingVehicleDetails_VisitorParkings_VisitorParkingId",
                        column: x => x.VisitorParkingId,
                        principalTable: "VisitorParkings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VisitorParkings_RegisterUserId",
                table: "VisitorParkings",
                column: "RegisterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitorParkings_SiteId",
                table: "VisitorParkings",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitorParkingVehicleDetails_VisitorBayNoId",
                table: "VisitorParkingVehicleDetails",
                column: "VisitorBayNoId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitorParkingVehicleDetails_VisitorParkingId",
                table: "VisitorParkingVehicleDetails",
                column: "VisitorParkingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VisitorParkingVehicleDetails");

            migrationBuilder.DropTable(
                name: "VisitorParkings");
        }
    }
}
