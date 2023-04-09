using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantAggregator.Auth.Migrations
{
    /// <inheritdoc />
    public partial class AddRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserUserRole_UserRole_RolesId",
                table: "UserUserRole");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRole",
                table: "UserRole");

            migrationBuilder.RenameTable(
                name: "UserRole",
                newName: "UserRoles");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "UserRoles",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_UserRoles_Name",
                table: "UserRoles",
                column: "Name");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_UserRoles_Role",
                table: "UserRoles",
                column: "Role");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserUserRole_UserRoles_RolesId",
                table: "UserUserRole",
                column: "RolesId",
                principalTable: "UserRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserUserRole_UserRoles_RolesId",
                table: "UserUserRole");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_UserRoles_Name",
                table: "UserRoles");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_UserRoles_Role",
                table: "UserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                newName: "UserRole");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "UserRole",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRole",
                table: "UserRole",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserUserRole_UserRole_RolesId",
                table: "UserUserRole",
                column: "RolesId",
                principalTable: "UserRole",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
