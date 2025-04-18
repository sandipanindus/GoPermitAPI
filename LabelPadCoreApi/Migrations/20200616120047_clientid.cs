﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace LabelPadCoreApi.Migrations
{
    public partial class clientid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "Users",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Users");
        }
    }
}
