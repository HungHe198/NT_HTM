using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NT.DAL.Migrations
{
    /// <inheritdoc />
    public partial class lan2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitColor",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "UnitHardness",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "UnitLength",
                table: "OrderDetails");

            migrationBuilder.RenameColumn(
                name: "UnitName",
                table: "OrderDetails",
                newName: "LengthAtOrder");

            migrationBuilder.AddColumn<string>(
                name: "ColorAtOrder",
                table: "OrderDetails",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HardnessAtOrder",
                table: "OrderDetails",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameAtOrder",
                table: "OrderDetails",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Role_Name",
                table: "Role",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permission_Code",
                table: "Permission",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Role_Name",
                table: "Role");

            migrationBuilder.DropIndex(
                name: "IX_Permission_Code",
                table: "Permission");

            migrationBuilder.DropColumn(
                name: "ColorAtOrder",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "HardnessAtOrder",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "NameAtOrder",
                table: "OrderDetails");

            migrationBuilder.RenameColumn(
                name: "LengthAtOrder",
                table: "OrderDetails",
                newName: "UnitName");

            migrationBuilder.AddColumn<decimal>(
                name: "UnitColor",
                table: "OrderDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitHardness",
                table: "OrderDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitLength",
                table: "OrderDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
