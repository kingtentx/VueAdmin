using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VueAdmin.Data.Migrations
{
    /// <inheritdoc />
    public partial class _04092 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CascadeId",
                table: "department",
                type: "varchar(500)",
                maxLength: 500,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "Leaf",
                table: "department",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CascadeId",
                table: "department");

            migrationBuilder.DropColumn(
                name: "Leaf",
                table: "department");
        }
    }
}
