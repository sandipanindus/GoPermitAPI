using Microsoft.EntityFrameworkCore.Migrations;

namespace LabelPadCoreApi.Migrations
{
    public partial class zatparklogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "VRM",
                table: "VehicleRegistrations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Zatparklogs24hrs",
                table: "Sites",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Zatparklogs24hrs",
                table: "Sites");

            migrationBuilder.AlterColumn<string>(
                name: "VRM",
                table: "VehicleRegistrations",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
