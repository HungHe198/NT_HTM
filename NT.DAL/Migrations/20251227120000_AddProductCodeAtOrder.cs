using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NT.DAL.Migrations
{
    public partial class AddProductCodeAtOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductCodeAtOrder",
                table: "OrderDetail",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductCodeAtOrder",
                table: "OrderDetail");
        }
    }
}
