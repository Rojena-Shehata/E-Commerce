using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce.Presistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModifyColumnNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItem_Order_OrderId",
                table: "OrderItem");

            migrationBuilder.RenameColumn(
                name: "Address_Street",
                table: "Order",
                newName: "ShipToAddress_Street");

            migrationBuilder.RenameColumn(
                name: "Address_LastName",
                table: "Order",
                newName: "ShipToAddress_LastName");

            migrationBuilder.RenameColumn(
                name: "Address_FirstName",
                table: "Order",
                newName: "ShipToAddress_FirstName");

            migrationBuilder.RenameColumn(
                name: "Address_Country",
                table: "Order",
                newName: "ShipToAddress_Country");

            migrationBuilder.RenameColumn(
                name: "Address_City",
                table: "Order",
                newName: "ShipToAddress_City");

            migrationBuilder.RenameColumn(
                name: "UserEmail",
                table: "Order",
                newName: "BuyerEmail");

            migrationBuilder.RenameColumn(
                name: "OrderStatus",
                table: "Order",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "DeliveryMethod",
                newName: "Cost");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItem_Order_OrderId",
                table: "OrderItem",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItem_Order_OrderId",
                table: "OrderItem");

            migrationBuilder.RenameColumn(
                name: "ShipToAddress_Street",
                table: "Order",
                newName: "Address_Street");

            migrationBuilder.RenameColumn(
                name: "ShipToAddress_LastName",
                table: "Order",
                newName: "Address_LastName");

            migrationBuilder.RenameColumn(
                name: "ShipToAddress_FirstName",
                table: "Order",
                newName: "Address_FirstName");

            migrationBuilder.RenameColumn(
                name: "ShipToAddress_Country",
                table: "Order",
                newName: "Address_Country");

            migrationBuilder.RenameColumn(
                name: "ShipToAddress_City",
                table: "Order",
                newName: "Address_City");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Order",
                newName: "OrderStatus");

            migrationBuilder.RenameColumn(
                name: "BuyerEmail",
                table: "Order",
                newName: "UserEmail");

            migrationBuilder.RenameColumn(
                name: "Cost",
                table: "DeliveryMethod",
                newName: "Price");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItem_Order_OrderId",
                table: "OrderItem",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id");
        }
    }
}
