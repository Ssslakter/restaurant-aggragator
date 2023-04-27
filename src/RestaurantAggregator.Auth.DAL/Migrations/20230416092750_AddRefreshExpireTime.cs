using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RestaurantAggregator.Auth.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshExpireTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("0381159f-a0a0-45e9-9a6f-a5b5054fc8c1"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7d1869fb-7e07-4138-b69d-6228ff0f319e"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a33efb0c-880d-4e15-a3e9-5a20b9ef7d87"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a5e243d6-1629-4e02-8e7b-fcd91987493e"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("f148397d-9a22-454e-9dae-f7e4a8b13fa3"));

            migrationBuilder.AddColumn<DateTime>(
                name: "Expires",
                table: "RefreshTokens",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "Expires",
                table: "RefreshTokens");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName", "RoleType" },
                values: new object[,]
                {
                    { new Guid("0381159f-a0a0-45e9-9a6f-a5b5054fc8c1"), null, "Client", "CLIENT", 0 },
                    { new Guid("7d1869fb-7e07-4138-b69d-6228ff0f319e"), null, "Cook", "COOK", 2 },
                    { new Guid("a33efb0c-880d-4e15-a3e9-5a20b9ef7d87"), null, "Admin", "ADMIN", 4 },
                    { new Guid("a5e243d6-1629-4e02-8e7b-fcd91987493e"), null, "Courier", "COURIER", 3 },
                    { new Guid("f148397d-9a22-454e-9dae-f7e4a8b13fa3"), null, "Manager", "MANAGER", 1 }
                });
        }
    }
}
