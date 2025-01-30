using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LabelPadCoreApi.Migrations
{
    public partial class NewMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RegisterUsers",
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
                    FirstName = table.Column<string>(maxLength: 300, nullable: false),
                    LastName = table.Column<string>(maxLength: 300, nullable: false),
                    OrganisationName = table.Column<string>(maxLength: 300, nullable: false),
                    WorkEmail = table.Column<string>(maxLength: 100, nullable: false),
                    ContactNumber = table.Column<string>(maxLength: 20, nullable: false),
                    Password = table.Column<string>(maxLength: 100, nullable: false),
                    Subdomain = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisterUsers", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegisterUsers");
        }
    }
}
