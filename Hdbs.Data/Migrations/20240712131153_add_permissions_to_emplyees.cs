﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hdbs.Data.Migrations
{
    /// <inheritdoc />
    public partial class add_permissions_to_emplyees : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Permissions",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Permissions",
                table: "AspNetUsers");
        }
    }
}
