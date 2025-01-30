using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LabelPadCoreApi.Migrations
{
    public partial class site : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sites",
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
                    SiteName = table.Column<string>(maxLength: 200, nullable: true),
                    SiteAddress = table.Column<string>(nullable: true),
                    City = table.Column<string>(maxLength: 200, nullable: true),
                    State = table.Column<string>(maxLength: 200, nullable: true),
                    Zipcode = table.Column<string>(maxLength: 20, nullable: true),
                    ContactPersonName = table.Column<string>(maxLength: 200, nullable: true),
                    Email = table.Column<string>(maxLength: 200, nullable: true),
                    ContactNumber = table.Column<string>(maxLength: 20, nullable: true),
                    MobileNumber = table.Column<string>(maxLength: 20, nullable: true),
                    TenantParkingBays = table.Column<string>(maxLength: 100, nullable: true),
                    VisitorParkingBays = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sites", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sites");
        }
    }
}
