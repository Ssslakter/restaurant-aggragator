using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantAggregator.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangedOrderRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Dishes_DishId",
                table: "Carts");

            migrationBuilder.DropTable(
                name: "DishDTO");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Carts_ClientId_DishId",
                table: "Carts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Carts",
                table: "Carts");

            migrationBuilder.RenameTable(
                name: "Carts",
                newName: "DishesInCarts");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Orders",
                newName: "TotalPrice");

            migrationBuilder.RenameIndex(
                name: "IX_Carts_DishId",
                table: "DishesInCarts",
                newName: "IX_DishesInCarts_DishId");

            migrationBuilder.AlterColumn<Guid>(
                name: "CourierId",
                table: "Orders",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CookId",
                table: "Orders",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "RestaurantId",
                table: "Orders",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "InOrder",
                table: "DishesInCarts",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "DishesInCarts",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "DishesInCarts",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_DishesInCarts_ClientId_DishId_OrderId",
                table: "DishesInCarts",
                columns: new[] { "ClientId", "DishId", "OrderId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_DishesInCarts",
                table: "DishesInCarts",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_DishesInCarts_OrderId",
                table: "DishesInCarts",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_DishesInCarts_Dishes_DishId",
                table: "DishesInCarts",
                column: "DishId",
                principalTable: "Dishes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DishesInCarts_Orders_OrderId",
                table: "DishesInCarts",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DishesInCarts_Dishes_DishId",
                table: "DishesInCarts");

            migrationBuilder.DropForeignKey(
                name: "FK_DishesInCarts_Orders_OrderId",
                table: "DishesInCarts");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_DishesInCarts_ClientId_DishId_OrderId",
                table: "DishesInCarts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DishesInCarts",
                table: "DishesInCarts");

            migrationBuilder.DropIndex(
                name: "IX_DishesInCarts_OrderId",
                table: "DishesInCarts");

            migrationBuilder.DropColumn(
                name: "RestaurantId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "InOrder",
                table: "DishesInCarts");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "DishesInCarts");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "DishesInCarts");

            migrationBuilder.RenameTable(
                name: "DishesInCarts",
                newName: "Carts");

            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "Orders",
                newName: "Price");

            migrationBuilder.RenameIndex(
                name: "IX_DishesInCarts_DishId",
                table: "Carts",
                newName: "IX_Carts_DishId");

            migrationBuilder.AlterColumn<Guid>(
                name: "CourierId",
                table: "Orders",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CookId",
                table: "Orders",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Carts_ClientId_DishId",
                table: "Carts",
                columns: new[] { "ClientId", "DishId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Carts",
                table: "Carts",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "DishDTO",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Category = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsVegeterian = table.Column<bool>(type: "boolean", nullable: false),
                    MenuId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: true),
                    Photo = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishDTO", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DishDTO_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DishDTO_OrderId",
                table: "DishDTO",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Dishes_DishId",
                table: "Carts",
                column: "DishId",
                principalTable: "Dishes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
