using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hdbs.Data.Migrations
{
    /// <inheritdoc />
    public partial class add_ForcedUnavailable_to_desk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ForcedUnavailable",
                table: "Desks",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForcedUnavailable",
                table: "Desks");
        }
    }
}
