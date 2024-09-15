using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VueAdmin.Data.Migrations
{
    /// <inheritdoc />
    public partial class _0914 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Menu");

            migrationBuilder.RenameColumn(
                name: "IsHasSubset",
                table: "Menu",
                newName: "IsShow");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsShow",
                table: "Menu",
                newName: "IsHasSubset");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Menu",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
