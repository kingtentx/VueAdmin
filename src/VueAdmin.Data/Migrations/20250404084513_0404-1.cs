using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VueAdmin.Data.Migrations
{
    /// <inheritdoc />
    public partial class _04041 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RealName",
                table: "user",
                newName: "NickName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NickName",
                table: "user",
                newName: "RealName");
        }
    }
}
