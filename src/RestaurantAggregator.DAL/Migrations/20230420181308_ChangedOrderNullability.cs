using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantAggregator.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangedOrderNullability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DishesInCarts_Orders_OrderId",
                table: "DishesInCarts");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_DishesInCarts_ClientId_DishId_OrderId",
                table: "DishesInCarts");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrderId",
                table: "DishesInCarts",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_DishesInCarts_Orders_OrderId",
                table: "DishesInCarts",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DishesInCarts_Orders_OrderId",
                table: "DishesInCarts");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrderId",
                table: "DishesInCarts",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_DishesInCarts_ClientId_DishId_OrderId",
                table: "DishesInCarts",
                columns: new[] { "ClientId", "DishId", "OrderId" });

            migrationBuilder.AddForeignKey(
                name: "FK_DishesInCarts_Orders_OrderId",
                table: "DishesInCarts",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
