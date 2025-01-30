using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LabelPadCoreApi.Migrations
{
    public partial class Modules : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Modules",
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
                    ModuleName = table.Column<string>(maxLength: 200, nullable: true),
                    Icon = table.Column<string>(maxLength: 200, nullable: true),
                    SequenceNo = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleScreens",
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
                    RoleId = table.Column<int>(nullable: false),
                    ScreenId = table.Column<int>(nullable: false),
                    View = table.Column<bool>(nullable: false),
                    Add = table.Column<bool>(nullable: false),
                    Edit = table.Column<bool>(nullable: false),
                    Delete = table.Column<bool>(nullable: false),
                    Approve = table.Column<bool>(nullable: false),
                    Reject = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleScreens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Screens",
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
                    ModuleId = table.Column<int>(nullable: false),
                    ScreenName = table.Column<string>(maxLength: 200, nullable: true),
                    Icon = table.Column<string>(maxLength: 200, nullable: true),
                    SequenceNo = table.Column<int>(maxLength: 200, nullable: false),
                    ScreenTableName = table.Column<string>(maxLength: 200, nullable: true),
                    ScreenChildTableName = table.Column<string>(maxLength: 200, nullable: true),
                    ScreenUrl = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Screens", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Modules");

            migrationBuilder.DropTable(
                name: "RoleScreens");

            migrationBuilder.DropTable(
                name: "Screens");
        }
    }
}
