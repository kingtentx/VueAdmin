using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VueAdmin.Data.Migrations
{
    /// <inheritdoc />
    public partial class _0404 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Sex",
                table: "user",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sex",
                table: "user");
        }
    }
}
