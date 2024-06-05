using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Talabat.Repository.Data.Migrations
{
    public partial class orderModuleAfterSetDevilveryMeethodSetNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_deliveryMethods_DeliveryMethodId",
                table: "Order");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_deliveryMethods_DeliveryMethodId",
                table: "Order",
                column: "DeliveryMethodId",
                principalTable: "deliveryMethods",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_deliveryMethods_DeliveryMethodId",
                table: "Order");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_deliveryMethods_DeliveryMethodId",
                table: "Order",
                column: "DeliveryMethodId",
                principalTable: "deliveryMethods",
                principalColumn: "Id");
        }
    }
}
