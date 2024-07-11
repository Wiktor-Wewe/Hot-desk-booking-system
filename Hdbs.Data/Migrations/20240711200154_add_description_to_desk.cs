using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hdbs.Data.Migrations
{
    /// <inheritdoc />
    public partial class add_description_to_desk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Desks",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Desks");
        }
    }
}
