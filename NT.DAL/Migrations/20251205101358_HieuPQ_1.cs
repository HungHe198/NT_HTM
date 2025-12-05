using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NT.DAL.Migrations
{
    /// <inheritdoc />
    public partial class HieuPQ_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Employee_ManagerId",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Employee_ManagerId1",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Voucher_CouponId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Employee_ManagerId",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_ManagerId1",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "InventoryValue",
                table: "ProductDetail");

            migrationBuilder.DropColumn(
                name: "ProfitMargin",
                table: "ProductDetail");

            migrationBuilder.DropColumn(
                name: "DiscountPercent",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "ManagerId1",
                table: "Employee");

            migrationBuilder.RenameColumn(
                name: "CouponId",
                table: "Order",
                newName: "VoucherId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_CouponId",
                table: "Order",
                newName: "IX_Order_VoucherId");

            migrationBuilder.AddColumn<string>(
                name: "TipDiameter",
                table: "ProductDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ShippingAddress",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Voucher_VoucherId",
                table: "Order",
                column: "VoucherId",
                principalTable: "Voucher",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Voucher_VoucherId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "TipDiameter",
                table: "ProductDetail");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "VoucherId",
                table: "Order",
                newName: "CouponId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_VoucherId",
                table: "Order",
                newName: "IX_Order_CouponId");

            migrationBuilder.AddColumn<decimal>(
                name: "InventoryValue",
                table: "ProductDetail",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ProfitMargin",
                table: "ProductDetail",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountPercent",
                table: "OrderDetail",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ShippingAddress",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "ManagerId",
                table: "Employee",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ManagerId1",
                table: "Employee",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employee_ManagerId",
                table: "Employee",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_ManagerId1",
                table: "Employee",
                column: "ManagerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Employee_ManagerId",
                table: "Employee",
                column: "ManagerId",
                principalTable: "Employee",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Employee_ManagerId1",
                table: "Employee",
                column: "ManagerId1",
                principalTable: "Employee",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Voucher_CouponId",
                table: "Order",
                column: "CouponId",
                principalTable: "Voucher",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
