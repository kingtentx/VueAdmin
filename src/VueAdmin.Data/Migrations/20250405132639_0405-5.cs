using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VueAdmin.Data.Migrations
{
    /// <inheritdoc />
    public partial class _04055 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Permission",
                table: "role_menu");

            migrationBuilder.DropColumn(
                name: "RoleType",
                table: "role");

            migrationBuilder.RenameColumn(
                name: "RoleName",
                table: "role",
                newName: "UpdateBy");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "role",
                newName: "Remark");

            migrationBuilder.AddColumn<int>(
                name: "MenuId",
                table: "role_menu",
                type: "int",
                maxLength: 100,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "role",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "CreateBy",
                table: "role",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "role",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "role",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateTime",
                table: "role",
                type: "datetime(6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MenuId",
                table: "role_menu");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "role");

            migrationBuilder.DropColumn(
                name: "CreateBy",
                table: "role");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "role");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "role");

            migrationBuilder.DropColumn(
                name: "UpdateTime",
                table: "role");

            migrationBuilder.RenameColumn(
                name: "UpdateBy",
                table: "role",
                newName: "RoleName");

            migrationBuilder.RenameColumn(
                name: "Remark",
                table: "role",
                newName: "Description");

            migrationBuilder.AddColumn<string>(
                name: "Permission",
                table: "role_menu",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "RoleType",
                table: "role",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
