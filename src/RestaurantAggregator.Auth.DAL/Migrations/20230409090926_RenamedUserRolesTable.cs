using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantAggregator.Auth.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RenamedUserRolesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserUserRole_UserRoles_RolesId",
                table: "UserUserRole");

            migrationBuilder.DropForeignKey(
                name: "FK_UserUserRole_Users_UsersId",
                table: "UserUserRole");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserUserRole",
                table: "UserUserRole");

            migrationBuilder.RenameTable(
                name: "UserUserRole",
                newName: "UsersWithRoles");

            migrationBuilder.RenameIndex(
                name: "IX_UserUserRole_UsersId",
                table: "UsersWithRoles",
                newName: "IX_UsersWithRoles_UsersId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersWithRoles",
                table: "UsersWithRoles",
                columns: new[] { "RolesId", "UsersId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UsersWithRoles_UserRoles_RolesId",
                table: "UsersWithRoles",
                column: "RolesId",
                principalTable: "UserRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersWithRoles_Users_UsersId",
                table: "UsersWithRoles",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersWithRoles_UserRoles_RolesId",
                table: "UsersWithRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersWithRoles_Users_UsersId",
                table: "UsersWithRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersWithRoles",
                table: "UsersWithRoles");

            migrationBuilder.RenameTable(
                name: "UsersWithRoles",
                newName: "UserUserRole");

            migrationBuilder.RenameIndex(
                name: "IX_UsersWithRoles_UsersId",
                table: "UserUserRole",
                newName: "IX_UserUserRole_UsersId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserUserRole",
                table: "UserUserRole",
                columns: new[] { "RolesId", "UsersId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserUserRole_UserRoles_RolesId",
                table: "UserUserRole",
                column: "RolesId",
                principalTable: "UserRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserUserRole_Users_UsersId",
                table: "UserUserRole",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
