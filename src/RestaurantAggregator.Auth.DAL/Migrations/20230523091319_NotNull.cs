using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RestaurantAggregator.Auth.DAL.Migrations
{
    /// <inheritdoc />
    public partial class NotNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("19140e48-a5cd-4d57-9713-bb9d2a298ab3"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("4376b100-699e-48d0-95c9-9bd36460cdd2"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7699a47b-b7e1-460e-a524-57e8f2d5c838"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("94b2fac1-a44c-4245-b1c8-9d0b8c83ca6d"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("e68eff36-d3a2-440b-a1c2-425ae958ba82"));

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "AspNetUsers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName", "RoleType" },
                values: new object[,]
                {
                    { new Guid("19140e48-a5cd-4d57-9713-bb9d2a298ab3"), null, "Admin", "ADMIN", 4 },
                    { new Guid("4376b100-699e-48d0-95c9-9bd36460cdd2"), null, "Courier", "COURIER", 3 },
                    { new Guid("7699a47b-b7e1-460e-a524-57e8f2d5c838"), null, "Cook", "COOK", 2 },
                    { new Guid("94b2fac1-a44c-4245-b1c8-9d0b8c83ca6d"), null, "Client", "CLIENT", 0 },
                    { new Guid("e68eff36-d3a2-440b-a1c2-425ae958ba82"), null, "Manager", "MANAGER", 1 }
                });
        }
    }
}
