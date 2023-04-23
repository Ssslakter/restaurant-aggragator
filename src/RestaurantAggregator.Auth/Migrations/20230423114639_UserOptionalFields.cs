using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RestaurantAggregator.Auth.Migrations
{
    /// <inheritdoc />
    public partial class UserOptionalFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("09217e9e-148f-4d88-9f4e-6c3bedaeb2d0"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5a8465ea-28d7-4808-8bad-fb9e07ac4d76"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("903a0e39-e124-4c1d-8bba-62a6cd359904"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("c47eb2ac-a179-4f2c-988f-bf3a474c52c1"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("fa4daa23-1c82-4d3e-b47d-9718e002df47"));

            migrationBuilder.AlterColumn<int>(
                name: "Gender",
                table: "AspNetUsers",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "BirthDate",
                table: "AspNetUsers",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName", "RoleType" },
                values: new object[,]
                {
                    { new Guid("06713623-f49e-4262-8756-543c9dbf7629"), null, "Cook", "COOK", 2 },
                    { new Guid("468f17c1-3b98-4982-80aa-6c419f47ab9a"), null, "Admin", "ADMIN", 4 },
                    { new Guid("abdf3564-f3fb-4e9c-8522-33d5281f9516"), null, "Manager", "MANAGER", 1 },
                    { new Guid("d0a3ed94-0234-49ab-8582-ee39e56ddc0c"), null, "Courier", "COURIER", 3 },
                    { new Guid("d374ee1d-399e-45f7-af98-1d8a820824b8"), null, "Client", "CLIENT", 0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("06713623-f49e-4262-8756-543c9dbf7629"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("468f17c1-3b98-4982-80aa-6c419f47ab9a"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("abdf3564-f3fb-4e9c-8522-33d5281f9516"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("d0a3ed94-0234-49ab-8582-ee39e56ddc0c"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("d374ee1d-399e-45f7-af98-1d8a820824b8"));

            migrationBuilder.AlterColumn<int>(
                name: "Gender",
                table: "AspNetUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "BirthDate",
                table: "AspNetUsers",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName", "RoleType" },
                values: new object[,]
                {
                    { new Guid("09217e9e-148f-4d88-9f4e-6c3bedaeb2d0"), null, "Admin", "ADMIN", 4 },
                    { new Guid("5a8465ea-28d7-4808-8bad-fb9e07ac4d76"), null, "Cook", "COOK", 2 },
                    { new Guid("903a0e39-e124-4c1d-8bba-62a6cd359904"), null, "Manager", "MANAGER", 1 },
                    { new Guid("c47eb2ac-a179-4f2c-988f-bf3a474c52c1"), null, "Courier", "COURIER", 3 },
                    { new Guid("fa4daa23-1c82-4d3e-b47d-9718e002df47"), null, "Client", "CLIENT", 0 }
                });
        }
    }
}
