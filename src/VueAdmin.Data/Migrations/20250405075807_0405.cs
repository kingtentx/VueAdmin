using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VueAdmin.Data.Migrations
{
    /// <inheritdoc />
    public partial class _0405 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Buttons",
                table: "menu");

            migrationBuilder.RenameColumn(
                name: "Spread",
                table: "menu",
                newName: "ShowParent");

            migrationBuilder.RenameColumn(
                name: "Pid",
                table: "menu",
                newName: "ParentId");

            migrationBuilder.RenameColumn(
                name: "PermissionKey",
                table: "menu",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "IsShow",
                table: "menu",
                newName: "ShowLink");

            migrationBuilder.AlterColumn<int>(
                name: "MenuType",
                table: "menu",
                type: "int",
                nullable: false,
                comment: "菜单类型 0-菜单 1-iframe 2-外链 3-按钮",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "ActivePath",
                table: "menu",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Auths",
                table: "menu",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Component",
                table: "menu",
                type: "varchar(250)",
                maxLength: 250,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "EnterTransition",
                table: "menu",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ExtraIcon",
                table: "menu",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "FixedTag",
                table: "menu",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "FrameLoading",
                table: "menu",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "FrameSrc",
                table: "menu",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "HiddenTag",
                table: "menu",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "KeepAlive",
                table: "menu",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LeaveTransition",
                table: "menu",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Redirect",
                table: "menu",
                type: "varchar(250)",
                maxLength: 250,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivePath",
                table: "menu");

            migrationBuilder.DropColumn(
                name: "Auths",
                table: "menu");

            migrationBuilder.DropColumn(
                name: "Component",
                table: "menu");

            migrationBuilder.DropColumn(
                name: "EnterTransition",
                table: "menu");

            migrationBuilder.DropColumn(
                name: "ExtraIcon",
                table: "menu");

            migrationBuilder.DropColumn(
                name: "FixedTag",
                table: "menu");

            migrationBuilder.DropColumn(
                name: "FrameLoading",
                table: "menu");

            migrationBuilder.DropColumn(
                name: "FrameSrc",
                table: "menu");

            migrationBuilder.DropColumn(
                name: "HiddenTag",
                table: "menu");

            migrationBuilder.DropColumn(
                name: "KeepAlive",
                table: "menu");

            migrationBuilder.DropColumn(
                name: "LeaveTransition",
                table: "menu");

            migrationBuilder.DropColumn(
                name: "Redirect",
                table: "menu");

            migrationBuilder.RenameColumn(
                name: "ShowParent",
                table: "menu",
                newName: "Spread");

            migrationBuilder.RenameColumn(
                name: "ShowLink",
                table: "menu",
                newName: "IsShow");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "menu",
                newName: "Pid");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "menu",
                newName: "PermissionKey");

            migrationBuilder.AlterColumn<int>(
                name: "MenuType",
                table: "menu",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "菜单类型 0-菜单 1-iframe 2-外链 3-按钮");

            migrationBuilder.AddColumn<string>(
                name: "Buttons",
                table: "menu",
                type: "varchar(500)",
                maxLength: 500,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
