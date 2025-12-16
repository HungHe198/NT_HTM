using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NT.DAL.Migrations
{
    /// <inheritdoc />
  public partial class AddTestVouchers : Migration
    {
     /// <inheritdoc />
   protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
    table: "Voucher",
             columns: new[] { "Id", "Code", "DiscountPercentage", "MaxDiscountAmount", "MinOrderAmount", "StartDate", "EndDate", "UsageCount", "MaxUsage" },
  values: new object[,]
          {
       { new Guid("aaaaaaaa-1111-1111-1111-000000000001"), "SAVE10", 10m, null, null, DateTime.UtcNow, DateTime.UtcNow.AddDays(30), 0, null },
          { new Guid("aaaaaaaa-2222-2222-2222-000000000002"), "SAVE20", 20m, 50000m, 500000m, DateTime.UtcNow, DateTime.UtcNow.AddDays(30), 0, 100 },
       { new Guid("aaaaaaaa-3333-3333-3333-000000000003"), "SALE5", 5m, null, null, DateTime.UtcNow, DateTime.UtcNow.AddDays(30), 0, null },
     { new Guid("aaaaaaaa-4444-4444-4444-000000000004"), "VIP30", 30m, 100000m, null, DateTime.UtcNow, DateTime.UtcNow.AddDays(30), 0, 50 },
        { new Guid("aaaaaaaa-5555-5555-5555-000000000005"), "FREESHIP", 0m, 35000m, 100000m, DateTime.UtcNow, DateTime.UtcNow.AddDays(30), 0, null }
           });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
    migrationBuilder.DeleteData(
     table: "Voucher",
       keyColumn: "Id",
       keyValues: new object[]
     {
     new Guid("aaaaaaaa-1111-1111-1111-000000000001"),
      new Guid("aaaaaaaa-2222-2222-2222-000000000002"),
        new Guid("aaaaaaaa-3333-3333-3333-000000000003"),
           new Guid("aaaaaaaa-4444-4444-4444-000000000004"),
           new Guid("aaaaaaaa-5555-5555-5555-000000000005")
            });
        }
    }
}
