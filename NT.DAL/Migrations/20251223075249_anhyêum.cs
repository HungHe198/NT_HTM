using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NT.DAL.Migrations
{
    /// <inheritdoc />
    public partial class anhyêum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Brand",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Website = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brand", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Color",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    HexCode = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Color", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Elasticity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Elasticity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Hardness",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hardness", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Length",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Length", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OriginCountry",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OriginCountry", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethod",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethod", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Resource = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Action = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Method = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SurfaceFinish",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurfaceFinish", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Voucher",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    MaxDiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MinOrderAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsageCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    MaxUsage = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voucher", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BrandId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Thumbnail = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SeoTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SeoDescription = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_Brand_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brand",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolePermission",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermission", x => new { x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_RolePermission_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermission_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Fullname = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategory",
                columns: table => new
                {
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategory", x => new { x.CategoryId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_ProductCategory_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductCategory_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LengthId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SurfaceFinishId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HardnessId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ElasticityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OriginCountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ColorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Sections = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CollapsedLength = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Weight = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TipWeight = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ButtWeight = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TipDiameter = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TopDiameter = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ButtDiameter = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    BalancePoint = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    BalanceLoadPoint = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    BalanceLoadDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RecommendedLine = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RecommendedFishWeight = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HandleType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    JointType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Warranty = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StockQuantity = table.Column<int>(type: "int", nullable: false),
                    SoldQuantity = table.Column<int>(type: "int", nullable: false),
                    CostPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LastImportDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductDetail_Color_ColorId",
                        column: x => x.ColorId,
                        principalTable: "Color",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductDetail_Elasticity_ElasticityId",
                        column: x => x.ElasticityId,
                        principalTable: "Elasticity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductDetail_Hardness_HardnessId",
                        column: x => x.HardnessId,
                        principalTable: "Hardness",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductDetail_Length_LengthId",
                        column: x => x.LengthId,
                        principalTable: "Length",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductDetail_OriginCountry_OriginCountryId",
                        column: x => x.OriginCountryId,
                        principalTable: "OriginCountry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductDetail_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductDetail_SurfaceFinish_SurfaceFinishId",
                        column: x => x.SurfaceFinishId,
                        principalTable: "SurfaceFinish",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Admin",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Admin_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    DoB = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customer_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employee_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductImage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImage_ProductDetail_ProductDetailId",
                        column: x => x.ProductDetailId,
                        principalTable: "ProductDetail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cart",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cart", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cart_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VoucherId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PaymentMethodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConfirmedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HandoverByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CompletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CancelledByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FinalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Order_PaymentMethod_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalTable: "PaymentMethod",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_Voucher_VoucherId",
                        column: x => x.VoucherId,
                        principalTable: "Voucher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "CartDetail",
                columns: table => new
                {
                    CartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartDetail", x => new { x.CartId, x.ProductDetailId });
                    table.ForeignKey(
                        name: "FK_CartDetail_Cart_CartId",
                        column: x => x.CartId,
                        principalTable: "Cart",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartDetail_ProductDetail_ProductDetailId",
                        column: x => x.ProductDetailId,
                        principalTable: "ProductDetail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAtOrder = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LengthAtOrder = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ColorAtOrder = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HardnessAtOrder = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDetail_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetail_ProductDetail_ProductDetailId",
                        column: x => x.ProductDetailId,
                        principalTable: "ProductDetail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Brand",
                columns: new[] { "Id", "Name", "Website" },
                values: new object[,]
                {
                    { new Guid("7ca06798-b314-4c1f-850e-1c8495119e2e"), "NT Fishing", "https://ntfishing.example" },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "Daiwa", "https://www.daiwa.com" },
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "Shimano", "https://www.shimano.com" },
                    { new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), "Okuma", "https://www.okumafishing.com" },
                    { new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), "Trabucco", "https://www.trabucco.it" }
                });

            migrationBuilder.InsertData(
                table: "Color",
                columns: new[] { "Id", "HexCode", "Name" },
                values: new object[] { new Guid("66666666-6666-6666-6666-000000000001"), "#000000", "Đen" });

            migrationBuilder.InsertData(
                table: "Elasticity",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { new Guid("88888888-8888-8888-8888-000000000001"), null, "Đàn hồi tiêu chuẩn" });

            migrationBuilder.InsertData(
                table: "Hardness",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000003"), "Độ cứng 3H cho cần câu đài", "3H" },
                    { new Guid("00000000-0000-0000-0000-000000000004"), "Độ cứng 4H cho cần câu đài", "4H" },
                    { new Guid("00000000-0000-0000-0000-000000000005"), "Độ cứng 5H cho cần câu đài", "5H" },
                    { new Guid("00000000-0000-0000-0000-000000000006"), "Độ cứng 6H cho cần câu đài", "6H" },
                    { new Guid("00000000-0000-0000-0000-000000000007"), "Độ cứng 7H cho cần câu đài", "7H" },
                    { new Guid("00000000-0000-0000-0000-000000000008"), "Độ cứng 8H cho cần câu đài", "8H" },
                    { new Guid("00000000-0000-0000-0000-000000000009"), "Độ cứng 9H cho cần câu đài", "9H" },
                    { new Guid("00000000-0000-0000-0000-000000000010"), "Độ cứng 10H cho cần câu đài", "10H" },
                    { new Guid("00000000-0000-0000-0000-000000000011"), "Độ cứng 11H cho cần câu đài", "11H" },
                    { new Guid("00000000-0000-0000-0000-000000000012"), "Độ cứng 12H cho cần câu đài", "12H" }
                });

            migrationBuilder.InsertData(
                table: "Length",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Cần câu đài 2m7", "2m7" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "Cần câu đài 3m9", "3m9" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "Cần câu đài 4m5", "4m5" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), "Cần câu đài 5m4", "5m4" },
                    { new Guid("55555555-5555-5555-5555-555555555555"), "Cần câu đài 6m3", "6m3" },
                    { new Guid("66666666-6666-6666-6666-666666666666"), "Cần câu đài 7m2", "7m2" },
                    { new Guid("77777777-7777-7777-7777-777777777777"), "Cần câu đài 8m1", "8m1" },
                    { new Guid("88888888-8888-8888-8888-888888888888"), "Cần câu đài 9m", "9m" }
                });

            migrationBuilder.InsertData(
                table: "OriginCountry",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { new Guid("77777777-7777-7777-7777-000000000001"), null, "Việt Nam" });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Admin" },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0001"), "Employee" },
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccc0001"), "Customer" }
                });

            migrationBuilder.InsertData(
                table: "SurfaceFinish",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { new Guid("99999999-9999-9999-9999-999999999999"), null, "Sơn bóng" });

            migrationBuilder.InsertData(
                table: "Voucher",
                columns: new[] { "Id", "Code", "DiscountPercentage", "EndDate", "MaxDiscountAmount", "MaxUsage", "MinOrderAmount", "StartDate" },
                values: new object[,]
                {
                    { new Guid("11111111-aaaa-bbbb-cccc-111111111111"), "WELCOME10", 10m, new DateTime(2026, 1, 23, 14, 52, 48, 940, DateTimeKind.Local).AddTicks(589), 50000m, 1000, 300000m, new DateTime(2025, 12, 16, 14, 52, 48, 940, DateTimeKind.Local).AddTicks(568) },
                    { new Guid("22222222-aaaa-bbbb-cccc-222222222222"), "SAVE15", 15m, new DateTime(2026, 2, 23, 14, 52, 48, 940, DateTimeKind.Local).AddTicks(598), 100000m, 500, 500000m, new DateTime(2025, 12, 20, 14, 52, 48, 940, DateTimeKind.Local).AddTicks(597) },
                    { new Guid("33333333-aaaa-bbbb-cccc-333333333333"), "BIGSALE25", 25m, new DateTime(2026, 3, 23, 14, 52, 48, 940, DateTimeKind.Local).AddTicks(612), 200000m, 300, 800000m, new DateTime(2025, 12, 22, 14, 52, 48, 940, DateTimeKind.Local).AddTicks(611) }
                });

            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "Id", "BrandId", "CreatedBy", "CreatedDate", "Description", "Name", "ProductCode", "SeoDescription", "SeoTitle", "ShortDescription", "Status", "Thumbnail", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), new Guid("7ca06798-b314-4c1f-850e-1c8495119e2e"), null, new DateTime(2025, 12, 23, 7, 52, 48, 882, DateTimeKind.Utc).AddTicks(7115), null, "Cần câu đài mẫu 1", "CD001", null, null, "Cần câu đài chất lượng, phù hợp hỗ dịch vụ và tự nhiên", "1", "/images/product-placeholder.png", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000002"), new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), null, new DateTime(2025, 12, 23, 7, 52, 48, 882, DateTimeKind.Utc).AddTicks(7184), null, "Cần câu đài mẫu 2", "CD002", null, null, "Cần câu đài chất lượng, phù hợp hỗ dịch vụ và tự nhiên", "1", "/images/product-placeholder.png", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000003"), new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), null, new DateTime(2025, 12, 23, 7, 52, 48, 882, DateTimeKind.Utc).AddTicks(7200), null, "Cần câu đài mẫu 3", "CD003", null, null, "Cần câu đài chất lượng, phù hợp hỗ dịch vụ và tự nhiên", "1", "/images/product-placeholder.png", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000004"), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), null, new DateTime(2025, 12, 23, 7, 52, 48, 882, DateTimeKind.Utc).AddTicks(7207), null, "Cần câu đài mẫu 4", "CD004", null, null, "Cần câu đài chất lượng, phù hợp hỗ dịch vụ và tự nhiên", "1", "/images/product-placeholder.png", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000005"), new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), null, new DateTime(2025, 12, 23, 7, 52, 48, 882, DateTimeKind.Utc).AddTicks(7215), null, "Cần câu đài mẫu 5", "CD005", null, null, "Cần câu đài chất lượng, phù hợp hỗ dịch vụ và tự nhiên", "1", "/images/product-placeholder.png", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000006"), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), null, new DateTime(2025, 12, 23, 7, 52, 48, 882, DateTimeKind.Utc).AddTicks(7226), null, "Cần câu đài mẫu 6", "CD006", null, null, "Cần câu đài chất lượng, phù hợp hỗ dịch vụ và tự nhiên", "1", "/images/product-placeholder.png", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000007"), new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), null, new DateTime(2025, 12, 23, 7, 52, 48, 882, DateTimeKind.Utc).AddTicks(7233), null, "Cần câu đài mẫu 7", "CD007", null, null, "Cần câu đài chất lượng, phù hợp hỗ dịch vụ và tự nhiên", "1", "/images/product-placeholder.png", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000008"), new Guid("7ca06798-b314-4c1f-850e-1c8495119e2e"), null, new DateTime(2025, 12, 23, 7, 52, 48, 882, DateTimeKind.Utc).AddTicks(7240), null, "Cần câu đài mẫu 8", "CD008", null, null, "Cần câu đài chất lượng, phù hợp hỗ dịch vụ và tự nhiên", "1", "/images/product-placeholder.png", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000009"), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), null, new DateTime(2025, 12, 23, 7, 52, 48, 882, DateTimeKind.Utc).AddTicks(7248), null, "Cần câu đài mẫu 9", "CD009", null, null, "Cần câu đài chất lượng, phù hợp hỗ dịch vụ và tự nhiên", "1", "/images/product-placeholder.png", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000010"), new Guid("7ca06798-b314-4c1f-850e-1c8495119e2e"), null, new DateTime(2025, 12, 23, 7, 52, 48, 882, DateTimeKind.Utc).AddTicks(7259), null, "Cần câu đài mẫu 10", "CD010", null, null, "Cần câu đài chất lượng, phù hợp hỗ dịch vụ và tự nhiên", "1", "/images/product-placeholder.png", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000011"), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), null, new DateTime(2025, 12, 23, 7, 52, 48, 882, DateTimeKind.Utc).AddTicks(7283), null, "Cần câu đài mẫu 11", "CD011", null, null, "Cần câu đài chất lượng, phù hợp hỗ dịch vụ và tự nhiên", "1", "/images/product-placeholder.png", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000012"), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), null, new DateTime(2025, 12, 23, 7, 52, 48, 882, DateTimeKind.Utc).AddTicks(7290), null, "Cần câu đài mẫu 12", "CD012", null, null, "Cần câu đài chất lượng, phù hợp hỗ dịch vụ và tự nhiên", "1", "/images/product-placeholder.png", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000013"), new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), null, new DateTime(2025, 12, 23, 7, 52, 48, 882, DateTimeKind.Utc).AddTicks(7296), null, "Cần câu đài mẫu 13", "CD013", null, null, "Cần câu đài chất lượng, phù hợp hỗ dịch vụ và tự nhiên", "1", "/images/product-placeholder.png", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000014"), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), null, new DateTime(2025, 12, 23, 7, 52, 48, 882, DateTimeKind.Utc).AddTicks(7303), null, "Cần câu đài mẫu 14", "CD014", null, null, "Cần câu đài chất lượng, phù hợp hỗ dịch vụ và tự nhiên", "1", "/images/product-placeholder.png", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000015"), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), null, new DateTime(2025, 12, 23, 7, 52, 48, 882, DateTimeKind.Utc).AddTicks(7310), null, "Cần câu đài mẫu 15", "CD015", null, null, "Cần câu đài chất lượng, phù hợp hỗ dịch vụ và tự nhiên", "1", "/images/product-placeholder.png", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000016"), new Guid("7ca06798-b314-4c1f-850e-1c8495119e2e"), null, new DateTime(2025, 12, 23, 7, 52, 48, 882, DateTimeKind.Utc).AddTicks(7316), null, "Cần câu đài mẫu 16", "CD016", null, null, "Cần câu đài chất lượng, phù hợp hỗ dịch vụ và tự nhiên", "1", "/images/product-placeholder.png", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000017"), new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), null, new DateTime(2025, 12, 23, 7, 52, 48, 882, DateTimeKind.Utc).AddTicks(7323), null, "Cần câu đài mẫu 17", "CD017", null, null, "Cần câu đài chất lượng, phù hợp hỗ dịch vụ và tự nhiên", "1", "/images/product-placeholder.png", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000018"), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), null, new DateTime(2025, 12, 23, 7, 52, 48, 882, DateTimeKind.Utc).AddTicks(7331), null, "Cần câu đài mẫu 18", "CD018", null, null, "Cần câu đài chất lượng, phù hợp hỗ dịch vụ và tự nhiên", "1", "/images/product-placeholder.png", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000019"), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), null, new DateTime(2025, 12, 23, 7, 52, 48, 882, DateTimeKind.Utc).AddTicks(7340), null, "Cần câu đài mẫu 19", "CD019", null, null, "Cần câu đài chất lượng, phù hợp hỗ dịch vụ và tự nhiên", "1", "/images/product-placeholder.png", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000020"), new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), null, new DateTime(2025, 12, 23, 7, 52, 48, 882, DateTimeKind.Utc).AddTicks(7346), null, "Cần câu đài mẫu 20", "CD020", null, null, "Cần câu đài chất lượng, phù hợp hỗ dịch vụ và tự nhiên", "1", "/images/product-placeholder.png", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000021"), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), null, new DateTime(2025, 12, 23, 7, 52, 48, 882, DateTimeKind.Utc).AddTicks(7353), null, "Cần câu đài mẫu 21", "CD021", null, null, "Cần câu đài chất lượng, phù hợp hỗ dịch vụ và tự nhiên", "1", "/images/product-placeholder.png", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000022"), new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), null, new DateTime(2025, 12, 23, 7, 52, 48, 882, DateTimeKind.Utc).AddTicks(7360), null, "Cần câu đài mẫu 22", "CD022", null, null, "Cần câu đài chất lượng, phù hợp hỗ dịch vụ và tự nhiên", "1", "/images/product-placeholder.png", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000023"), new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), null, new DateTime(2025, 12, 23, 7, 52, 48, 882, DateTimeKind.Utc).AddTicks(7368), null, "Cần câu đài mẫu 23", "CD023", null, null, "Cần câu đài chất lượng, phù hợp hỗ dịch vụ và tự nhiên", "1", "/images/product-placeholder.png", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000024"), new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), null, new DateTime(2025, 12, 23, 7, 52, 48, 882, DateTimeKind.Utc).AddTicks(7376), null, "Cần câu đài mẫu 24", "CD024", null, null, "Cần câu đài chất lượng, phù hợp hỗ dịch vụ và tự nhiên", "1", "/images/product-placeholder.png", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000025"), new Guid("7ca06798-b314-4c1f-850e-1c8495119e2e"), null, new DateTime(2025, 12, 23, 7, 52, 48, 882, DateTimeKind.Utc).AddTicks(7384), null, "Cần câu đài mẫu 25", "CD025", null, null, "Cần câu đài chất lượng, phù hợp hỗ dịch vụ và tự nhiên", "1", "/images/product-placeholder.png", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000026"), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), null, new DateTime(2025, 12, 23, 7, 52, 48, 882, DateTimeKind.Utc).AddTicks(7392), null, "Cần câu đài mẫu 26", "CD026", null, null, "Cần câu đài chất lượng, phù hợp hỗ dịch vụ và tự nhiên", "1", "/images/product-placeholder.png", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000027"), new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), null, new DateTime(2025, 12, 23, 7, 52, 48, 882, DateTimeKind.Utc).AddTicks(7412), null, "Cần câu đài mẫu 27", "CD027", null, null, "Cần câu đài chất lượng, phù hợp hỗ dịch vụ và tự nhiên", "1", "/images/product-placeholder.png", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000028"), new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), null, new DateTime(2025, 12, 23, 7, 52, 48, 882, DateTimeKind.Utc).AddTicks(7423), null, "Cần câu đài mẫu 28", "CD028", null, null, "Cần câu đài chất lượng, phù hợp hỗ dịch vụ và tự nhiên", "1", "/images/product-placeholder.png", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000029"), new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), null, new DateTime(2025, 12, 23, 7, 52, 48, 882, DateTimeKind.Utc).AddTicks(7432), null, "Cần câu đài mẫu 29", "CD029", null, null, "Cần câu đài chất lượng, phù hợp hỗ dịch vụ và tự nhiên", "1", "/images/product-placeholder.png", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000030"), new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), null, new DateTime(2025, 12, 23, 7, 52, 48, 882, DateTimeKind.Utc).AddTicks(7439), null, "Cần câu đài mẫu 30", "CD030", null, null, "Cần câu đài chất lượng, phù hợp hỗ dịch vụ và tự nhiên", "1", "/images/product-placeholder.png", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000031"), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), null, new DateTime(2025, 12, 23, 7, 52, 48, 882, DateTimeKind.Utc).AddTicks(7447), null, "Cần câu đài mẫu 31", "CD031", null, null, "Cần câu đài chất lượng, phù hợp hỗ dịch vụ và tự nhiên", "1", "/images/product-placeholder.png", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000032"), new Guid("7ca06798-b314-4c1f-850e-1c8495119e2e"), null, new DateTime(2025, 12, 23, 7, 52, 48, 882, DateTimeKind.Utc).AddTicks(7455), null, "Cần câu đài mẫu 32", "CD032", null, null, "Cần câu đài chất lượng, phù hợp hỗ dịch vụ và tự nhiên", "1", "/images/product-placeholder.png", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000033"), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), null, new DateTime(2025, 12, 23, 7, 52, 48, 882, DateTimeKind.Utc).AddTicks(7464), null, "Cần câu đài mẫu 33", "CD033", null, null, "Cần câu đài chất lượng, phù hợp hỗ dịch vụ và tự nhiên", "1", "/images/product-placeholder.png", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000034"), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), null, new DateTime(2025, 12, 23, 7, 52, 48, 882, DateTimeKind.Utc).AddTicks(7473), null, "Cần câu đài mẫu 34", "CD034", null, null, "Cần câu đài chất lượng, phù hợp hỗ dịch vụ và tự nhiên", "1", "/images/product-placeholder.png", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000035"), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), null, new DateTime(2025, 12, 23, 7, 52, 48, 882, DateTimeKind.Utc).AddTicks(7483), null, "Cần câu đài mẫu 35", "CD035", null, null, "Cần câu đài chất lượng, phù hợp hỗ dịch vụ và tự nhiên", "1", "/images/product-placeholder.png", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000036"), new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), null, new DateTime(2025, 12, 23, 7, 52, 48, 882, DateTimeKind.Utc).AddTicks(7490), null, "Cần câu đài mẫu 36", "CD036", null, null, "Cần câu đài chất lượng, phù hợp hỗ dịch vụ và tự nhiên", "1", "/images/product-placeholder.png", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000037"), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), null, new DateTime(2025, 12, 23, 7, 52, 48, 882, DateTimeKind.Utc).AddTicks(7496), null, "Cần câu đài mẫu 37", "CD037", null, null, "Cần câu đài chất lượng, phù hợp hỗ dịch vụ và tự nhiên", "1", "/images/product-placeholder.png", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000038"), new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), null, new DateTime(2025, 12, 23, 7, 52, 48, 882, DateTimeKind.Utc).AddTicks(7529), null, "Cần câu đài mẫu 38", "CD038", null, null, "Cần câu đài chất lượng, phù hợp hỗ dịch vụ và tự nhiên", "1", "/images/product-placeholder.png", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000039"), new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), null, new DateTime(2025, 12, 23, 7, 52, 48, 882, DateTimeKind.Utc).AddTicks(7537), null, "Cần câu đài mẫu 39", "CD039", null, null, "Cần câu đài chất lượng, phù hợp hỗ dịch vụ và tự nhiên", "1", "/images/product-placeholder.png", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000040"), new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), null, new DateTime(2025, 12, 23, 7, 52, 48, 882, DateTimeKind.Utc).AddTicks(7545), null, "Cần câu đài mẫu 40", "CD040", null, null, "Cần câu đài chất lượng, phù hợp hỗ dịch vụ và tự nhiên", "1", "/images/product-placeholder.png", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000041"), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), null, new DateTime(2025, 12, 23, 7, 52, 48, 882, DateTimeKind.Utc).AddTicks(7553), null, "Cần câu đài mẫu 41", "CD041", null, null, "Cần câu đài chất lượng, phù hợp hỗ dịch vụ và tự nhiên", "1", "/images/product-placeholder.png", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000042"), new Guid("7ca06798-b314-4c1f-850e-1c8495119e2e"), null, new DateTime(2025, 12, 23, 7, 52, 48, 882, DateTimeKind.Utc).AddTicks(7561), null, "Cần câu đài mẫu 42", "CD042", null, null, "Cần câu đài chất lượng, phù hợp hỗ dịch vụ và tự nhiên", "1", "/images/product-placeholder.png", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000043"), new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), null, new DateTime(2025, 12, 23, 7, 52, 48, 882, DateTimeKind.Utc).AddTicks(7571), null, "Cần câu đài mẫu 43", "CD043", null, null, "Cần câu đài chất lượng, phù hợp hỗ dịch vụ và tự nhiên", "1", "/images/product-placeholder.png", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000044"), new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), null, new DateTime(2025, 12, 23, 7, 52, 48, 882, DateTimeKind.Utc).AddTicks(7588), null, "Cần câu đài mẫu 44", "CD044", null, null, "Cần câu đài chất lượng, phù hợp hỗ dịch vụ và tự nhiên", "1", "/images/product-placeholder.png", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000045"), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), null, new DateTime(2025, 12, 23, 7, 52, 48, 882, DateTimeKind.Utc).AddTicks(7596), null, "Cần câu đài mẫu 45", "CD045", null, null, "Cần câu đài chất lượng, phù hợp hỗ dịch vụ và tự nhiên", "1", "/images/product-placeholder.png", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000046"), new Guid("7ca06798-b314-4c1f-850e-1c8495119e2e"), null, new DateTime(2025, 12, 23, 7, 52, 48, 882, DateTimeKind.Utc).AddTicks(7604), null, "Cần câu đài mẫu 46", "CD046", null, null, "Cần câu đài chất lượng, phù hợp hỗ dịch vụ và tự nhiên", "1", "/images/product-placeholder.png", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000047"), new Guid("7ca06798-b314-4c1f-850e-1c8495119e2e"), null, new DateTime(2025, 12, 23, 7, 52, 48, 882, DateTimeKind.Utc).AddTicks(7612), null, "Cần câu đài mẫu 47", "CD047", null, null, "Cần câu đài chất lượng, phù hợp hỗ dịch vụ và tự nhiên", "1", "/images/product-placeholder.png", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000048"), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), null, new DateTime(2025, 12, 23, 7, 52, 48, 882, DateTimeKind.Utc).AddTicks(7620), null, "Cần câu đài mẫu 48", "CD048", null, null, "Cần câu đài chất lượng, phù hợp hỗ dịch vụ và tự nhiên", "1", "/images/product-placeholder.png", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000049"), new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), null, new DateTime(2025, 12, 23, 7, 52, 48, 882, DateTimeKind.Utc).AddTicks(7626), null, "Cần câu đài mẫu 49", "CD049", null, null, "Cần câu đài chất lượng, phù hợp hỗ dịch vụ và tự nhiên", "1", "/images/product-placeholder.png", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000050"), new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), null, new DateTime(2025, 12, 23, 7, 52, 48, 882, DateTimeKind.Utc).AddTicks(7632), null, "Cần câu đài mẫu 50", "CD050", null, null, "Cần câu đài chất lượng, phù hợp hỗ dịch vụ và tự nhiên", "1", "/images/product-placeholder.png", null, null }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Email", "Fullname", "PasswordHash", "PhoneNumber", "RoleId", "Status", "Username" },
                values: new object[] { new Guid("11111111-2222-3333-4444-555555555555"), "admin@example.com", "System Administrator", "AQAAAAIAAYagAAAAEDM1q++3kJXbZlbYCIHhyFq0K7svyrgKLO3tUxNxYk3JouHy0HB5Q2hehDpIiD0Dag==", null, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Active", "admin" });

            migrationBuilder.InsertData(
                table: "Admin",
                columns: new[] { "Id", "Position", "Salary", "UserId" },
                values: new object[] { new Guid("22222222-3333-4444-5555-666666666666"), "Administrator", null, new Guid("11111111-2222-3333-4444-555555555555") });

            migrationBuilder.InsertData(
                table: "ProductDetail",
                columns: new[] { "Id", "BalanceLoadDescription", "BalanceLoadPoint", "BalancePoint", "ButtDiameter", "ButtWeight", "CollapsedLength", "ColorId", "CostPrice", "ElasticityId", "HandleType", "HardnessId", "IsActive", "JointType", "LastImportDate", "LengthId", "OriginCountryId", "Price", "ProductId", "RecommendedFishWeight", "RecommendedLine", "Sections", "SoldQuantity", "StockQuantity", "SurfaceFinishId", "TipDiameter", "TipWeight", "TopDiameter", "Warranty", "Weight" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000004"), true, null, null, new Guid("22222222-2222-2222-2222-222222222222"), new Guid("77777777-7777-7777-7777-000000000001"), 690000m, new Guid("00000000-0000-0000-0000-000000000001"), null, null, null, 0, 45, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000002"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000006"), true, null, null, new Guid("44444444-4444-4444-4444-444444444444"), new Guid("77777777-7777-7777-7777-000000000001"), 650000m, new Guid("00000000-0000-0000-0000-000000000001"), null, null, null, 0, 47, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000003"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000008"), true, null, null, new Guid("66666666-6666-6666-6666-666666666666"), new Guid("77777777-7777-7777-7777-000000000001"), 660000m, new Guid("00000000-0000-0000-0000-000000000001"), null, null, null, 0, 47, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000004"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000010"), true, null, null, new Guid("88888888-8888-8888-8888-888888888888"), new Guid("77777777-7777-7777-7777-000000000001"), 900000m, new Guid("00000000-0000-0000-0000-000000000001"), null, null, null, 0, 28, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000005"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000012"), true, null, null, new Guid("22222222-2222-2222-2222-222222222222"), new Guid("77777777-7777-7777-7777-000000000001"), 820000m, new Guid("00000000-0000-0000-0000-000000000001"), null, null, null, 0, 19, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000006"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000009"), true, null, null, new Guid("77777777-7777-7777-7777-777777777777"), new Guid("77777777-7777-7777-7777-000000000001"), 700000m, new Guid("00000000-0000-0000-0000-000000000002"), null, null, null, 0, 44, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000007"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000011"), true, null, null, new Guid("11111111-1111-1111-1111-111111111111"), new Guid("77777777-7777-7777-7777-000000000001"), 970000m, new Guid("00000000-0000-0000-0000-000000000002"), null, null, null, 0, 25, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000008"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000003"), true, null, null, new Guid("33333333-3333-3333-3333-333333333333"), new Guid("77777777-7777-7777-7777-000000000001"), 870000m, new Guid("00000000-0000-0000-0000-000000000002"), null, null, null, 0, 36, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000009"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000005"), true, null, null, new Guid("55555555-5555-5555-5555-555555555555"), new Guid("77777777-7777-7777-7777-000000000001"), 790000m, new Guid("00000000-0000-0000-0000-000000000002"), null, null, null, 0, 8, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000010"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000007"), true, null, null, new Guid("77777777-7777-7777-7777-777777777777"), new Guid("77777777-7777-7777-7777-000000000001"), 540000m, new Guid("00000000-0000-0000-0000-000000000002"), null, null, null, 0, 10, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000011"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000004"), true, null, null, new Guid("44444444-4444-4444-4444-444444444444"), new Guid("77777777-7777-7777-7777-000000000001"), 880000m, new Guid("00000000-0000-0000-0000-000000000003"), null, null, null, 0, 31, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000012"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000006"), true, null, null, new Guid("66666666-6666-6666-6666-666666666666"), new Guid("77777777-7777-7777-7777-000000000001"), 770000m, new Guid("00000000-0000-0000-0000-000000000003"), null, null, null, 0, 10, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000013"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000008"), true, null, null, new Guid("88888888-8888-8888-8888-888888888888"), new Guid("77777777-7777-7777-7777-000000000001"), 810000m, new Guid("00000000-0000-0000-0000-000000000003"), null, null, null, 0, 14, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000014"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000010"), true, null, null, new Guid("22222222-2222-2222-2222-222222222222"), new Guid("77777777-7777-7777-7777-000000000001"), 940000m, new Guid("00000000-0000-0000-0000-000000000003"), null, null, null, 0, 17, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000015"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000012"), true, null, null, new Guid("44444444-4444-4444-4444-444444444444"), new Guid("77777777-7777-7777-7777-000000000001"), 690000m, new Guid("00000000-0000-0000-0000-000000000003"), null, null, null, 0, 45, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000016"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000009"), true, null, null, new Guid("11111111-1111-1111-1111-111111111111"), new Guid("77777777-7777-7777-7777-000000000001"), 870000m, new Guid("00000000-0000-0000-0000-000000000004"), null, null, null, 0, 27, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000017"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000011"), true, null, null, new Guid("33333333-3333-3333-3333-333333333333"), new Guid("77777777-7777-7777-7777-000000000001"), 750000m, new Guid("00000000-0000-0000-0000-000000000004"), null, null, null, 0, 46, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000018"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000003"), true, null, null, new Guid("55555555-5555-5555-5555-555555555555"), new Guid("77777777-7777-7777-7777-000000000001"), 760000m, new Guid("00000000-0000-0000-0000-000000000004"), null, null, null, 0, 15, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000019"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000005"), true, null, null, new Guid("77777777-7777-7777-7777-777777777777"), new Guid("77777777-7777-7777-7777-000000000001"), 600000m, new Guid("00000000-0000-0000-0000-000000000004"), null, null, null, 0, 20, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000020"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000007"), true, null, null, new Guid("11111111-1111-1111-1111-111111111111"), new Guid("77777777-7777-7777-7777-000000000001"), 830000m, new Guid("00000000-0000-0000-0000-000000000004"), null, null, null, 0, 33, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000021"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000004"), true, null, null, new Guid("66666666-6666-6666-6666-666666666666"), new Guid("77777777-7777-7777-7777-000000000001"), 920000m, new Guid("00000000-0000-0000-0000-000000000005"), null, null, null, 0, 29, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000022"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000006"), true, null, null, new Guid("88888888-8888-8888-8888-888888888888"), new Guid("77777777-7777-7777-7777-000000000001"), 570000m, new Guid("00000000-0000-0000-0000-000000000005"), null, null, null, 0, 46, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000023"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000008"), true, null, null, new Guid("22222222-2222-2222-2222-222222222222"), new Guid("77777777-7777-7777-7777-000000000001"), 750000m, new Guid("00000000-0000-0000-0000-000000000005"), null, null, null, 0, 27, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000024"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000010"), true, null, null, new Guid("44444444-4444-4444-4444-444444444444"), new Guid("77777777-7777-7777-7777-000000000001"), 580000m, new Guid("00000000-0000-0000-0000-000000000005"), null, null, null, 0, 40, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000025"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000012"), true, null, null, new Guid("66666666-6666-6666-6666-666666666666"), new Guid("77777777-7777-7777-7777-000000000001"), 850000m, new Guid("00000000-0000-0000-0000-000000000005"), null, null, null, 0, 19, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000026"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000009"), true, null, null, new Guid("33333333-3333-3333-3333-333333333333"), new Guid("77777777-7777-7777-7777-000000000001"), 880000m, new Guid("00000000-0000-0000-0000-000000000006"), null, null, null, 0, 44, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000027"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000011"), true, null, null, new Guid("55555555-5555-5555-5555-555555555555"), new Guid("77777777-7777-7777-7777-000000000001"), 710000m, new Guid("00000000-0000-0000-0000-000000000006"), null, null, null, 0, 44, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000028"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000003"), true, null, null, new Guid("77777777-7777-7777-7777-777777777777"), new Guid("77777777-7777-7777-7777-000000000001"), 630000m, new Guid("00000000-0000-0000-0000-000000000006"), null, null, null, 0, 41, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000029"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000005"), true, null, null, new Guid("11111111-1111-1111-1111-111111111111"), new Guid("77777777-7777-7777-7777-000000000001"), 670000m, new Guid("00000000-0000-0000-0000-000000000006"), null, null, null, 0, 13, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000030"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000007"), true, null, null, new Guid("33333333-3333-3333-3333-333333333333"), new Guid("77777777-7777-7777-7777-000000000001"), 650000m, new Guid("00000000-0000-0000-0000-000000000006"), null, null, null, 0, 10, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000031"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000004"), true, null, null, new Guid("88888888-8888-8888-8888-888888888888"), new Guid("77777777-7777-7777-7777-000000000001"), 530000m, new Guid("00000000-0000-0000-0000-000000000007"), null, null, null, 0, 28, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000032"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000006"), true, null, null, new Guid("22222222-2222-2222-2222-222222222222"), new Guid("77777777-7777-7777-7777-000000000001"), 560000m, new Guid("00000000-0000-0000-0000-000000000007"), null, null, null, 0, 38, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000033"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000008"), true, null, null, new Guid("44444444-4444-4444-4444-444444444444"), new Guid("77777777-7777-7777-7777-000000000001"), 780000m, new Guid("00000000-0000-0000-0000-000000000007"), null, null, null, 0, 46, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000034"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000010"), true, null, null, new Guid("66666666-6666-6666-6666-666666666666"), new Guid("77777777-7777-7777-7777-000000000001"), 670000m, new Guid("00000000-0000-0000-0000-000000000007"), null, null, null, 0, 5, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000035"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000012"), true, null, null, new Guid("88888888-8888-8888-8888-888888888888"), new Guid("77777777-7777-7777-7777-000000000001"), 960000m, new Guid("00000000-0000-0000-0000-000000000007"), null, null, null, 0, 27, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000036"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000009"), true, null, null, new Guid("55555555-5555-5555-5555-555555555555"), new Guid("77777777-7777-7777-7777-000000000001"), 730000m, new Guid("00000000-0000-0000-0000-000000000008"), null, null, null, 0, 15, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000037"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000011"), true, null, null, new Guid("77777777-7777-7777-7777-777777777777"), new Guid("77777777-7777-7777-7777-000000000001"), 700000m, new Guid("00000000-0000-0000-0000-000000000008"), null, null, null, 0, 25, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000038"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000003"), true, null, null, new Guid("11111111-1111-1111-1111-111111111111"), new Guid("77777777-7777-7777-7777-000000000001"), 640000m, new Guid("00000000-0000-0000-0000-000000000008"), null, null, null, 0, 15, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000039"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000005"), true, null, null, new Guid("33333333-3333-3333-3333-333333333333"), new Guid("77777777-7777-7777-7777-000000000001"), 720000m, new Guid("00000000-0000-0000-0000-000000000008"), null, null, null, 0, 33, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000040"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000007"), true, null, null, new Guid("55555555-5555-5555-5555-555555555555"), new Guid("77777777-7777-7777-7777-000000000001"), 800000m, new Guid("00000000-0000-0000-0000-000000000008"), null, null, null, 0, 11, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000041"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000004"), true, null, null, new Guid("22222222-2222-2222-2222-222222222222"), new Guid("77777777-7777-7777-7777-000000000001"), 520000m, new Guid("00000000-0000-0000-0000-000000000009"), null, null, null, 0, 9, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000042"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000006"), true, null, null, new Guid("44444444-4444-4444-4444-444444444444"), new Guid("77777777-7777-7777-7777-000000000001"), 790000m, new Guid("00000000-0000-0000-0000-000000000009"), null, null, null, 0, 8, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000043"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000008"), true, null, null, new Guid("66666666-6666-6666-6666-666666666666"), new Guid("77777777-7777-7777-7777-000000000001"), 550000m, new Guid("00000000-0000-0000-0000-000000000009"), null, null, null, 0, 44, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000044"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000010"), true, null, null, new Guid("88888888-8888-8888-8888-888888888888"), new Guid("77777777-7777-7777-7777-000000000001"), 520000m, new Guid("00000000-0000-0000-0000-000000000009"), null, null, null, 0, 33, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000045"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000012"), true, null, null, new Guid("22222222-2222-2222-2222-222222222222"), new Guid("77777777-7777-7777-7777-000000000001"), 820000m, new Guid("00000000-0000-0000-0000-000000000009"), null, null, null, 0, 37, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000046"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000009"), true, null, null, new Guid("77777777-7777-7777-7777-777777777777"), new Guid("77777777-7777-7777-7777-000000000001"), 940000m, new Guid("00000000-0000-0000-0000-000000000010"), null, null, null, 0, 5, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000047"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000011"), true, null, null, new Guid("11111111-1111-1111-1111-111111111111"), new Guid("77777777-7777-7777-7777-000000000001"), 520000m, new Guid("00000000-0000-0000-0000-000000000010"), null, null, null, 0, 29, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000048"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000003"), true, null, null, new Guid("33333333-3333-3333-3333-333333333333"), new Guid("77777777-7777-7777-7777-000000000001"), 780000m, new Guid("00000000-0000-0000-0000-000000000010"), null, null, null, 0, 19, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000049"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000005"), true, null, null, new Guid("55555555-5555-5555-5555-555555555555"), new Guid("77777777-7777-7777-7777-000000000001"), 710000m, new Guid("00000000-0000-0000-0000-000000000010"), null, null, null, 0, 22, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000050"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000007"), true, null, null, new Guid("77777777-7777-7777-7777-777777777777"), new Guid("77777777-7777-7777-7777-000000000001"), 670000m, new Guid("00000000-0000-0000-0000-000000000010"), null, null, null, 0, 31, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000051"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000004"), true, null, null, new Guid("44444444-4444-4444-4444-444444444444"), new Guid("77777777-7777-7777-7777-000000000001"), 570000m, new Guid("00000000-0000-0000-0000-000000000011"), null, null, null, 0, 11, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000052"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000006"), true, null, null, new Guid("66666666-6666-6666-6666-666666666666"), new Guid("77777777-7777-7777-7777-000000000001"), 920000m, new Guid("00000000-0000-0000-0000-000000000011"), null, null, null, 0, 13, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000053"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000008"), true, null, null, new Guid("88888888-8888-8888-8888-888888888888"), new Guid("77777777-7777-7777-7777-000000000001"), 920000m, new Guid("00000000-0000-0000-0000-000000000011"), null, null, null, 0, 29, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000054"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000010"), true, null, null, new Guid("22222222-2222-2222-2222-222222222222"), new Guid("77777777-7777-7777-7777-000000000001"), 730000m, new Guid("00000000-0000-0000-0000-000000000011"), null, null, null, 0, 49, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000055"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000012"), true, null, null, new Guid("44444444-4444-4444-4444-444444444444"), new Guid("77777777-7777-7777-7777-000000000001"), 800000m, new Guid("00000000-0000-0000-0000-000000000011"), null, null, null, 0, 7, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000056"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000009"), true, null, null, new Guid("11111111-1111-1111-1111-111111111111"), new Guid("77777777-7777-7777-7777-000000000001"), 670000m, new Guid("00000000-0000-0000-0000-000000000012"), null, null, null, 0, 37, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000057"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000011"), true, null, null, new Guid("33333333-3333-3333-3333-333333333333"), new Guid("77777777-7777-7777-7777-000000000001"), 790000m, new Guid("00000000-0000-0000-0000-000000000012"), null, null, null, 0, 12, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000058"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000003"), true, null, null, new Guid("55555555-5555-5555-5555-555555555555"), new Guid("77777777-7777-7777-7777-000000000001"), 540000m, new Guid("00000000-0000-0000-0000-000000000012"), null, null, null, 0, 48, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000059"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000005"), true, null, null, new Guid("77777777-7777-7777-7777-777777777777"), new Guid("77777777-7777-7777-7777-000000000001"), 970000m, new Guid("00000000-0000-0000-0000-000000000012"), null, null, null, 0, 7, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000060"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000007"), true, null, null, new Guid("11111111-1111-1111-1111-111111111111"), new Guid("77777777-7777-7777-7777-000000000001"), 810000m, new Guid("00000000-0000-0000-0000-000000000012"), null, null, null, 0, 36, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000061"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000004"), true, null, null, new Guid("66666666-6666-6666-6666-666666666666"), new Guid("77777777-7777-7777-7777-000000000001"), 930000m, new Guid("00000000-0000-0000-0000-000000000013"), null, null, null, 0, 37, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000062"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000006"), true, null, null, new Guid("88888888-8888-8888-8888-888888888888"), new Guid("77777777-7777-7777-7777-000000000001"), 680000m, new Guid("00000000-0000-0000-0000-000000000013"), null, null, null, 0, 14, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000063"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000008"), true, null, null, new Guid("22222222-2222-2222-2222-222222222222"), new Guid("77777777-7777-7777-7777-000000000001"), 800000m, new Guid("00000000-0000-0000-0000-000000000013"), null, null, null, 0, 25, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000064"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000010"), true, null, null, new Guid("44444444-4444-4444-4444-444444444444"), new Guid("77777777-7777-7777-7777-000000000001"), 590000m, new Guid("00000000-0000-0000-0000-000000000013"), null, null, null, 0, 43, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000065"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000012"), true, null, null, new Guid("66666666-6666-6666-6666-666666666666"), new Guid("77777777-7777-7777-7777-000000000001"), 940000m, new Guid("00000000-0000-0000-0000-000000000013"), null, null, null, 0, 48, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000066"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000009"), true, null, null, new Guid("33333333-3333-3333-3333-333333333333"), new Guid("77777777-7777-7777-7777-000000000001"), 900000m, new Guid("00000000-0000-0000-0000-000000000014"), null, null, null, 0, 7, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000067"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000011"), true, null, null, new Guid("55555555-5555-5555-5555-555555555555"), new Guid("77777777-7777-7777-7777-000000000001"), 630000m, new Guid("00000000-0000-0000-0000-000000000014"), null, null, null, 0, 5, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000068"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000003"), true, null, null, new Guid("77777777-7777-7777-7777-777777777777"), new Guid("77777777-7777-7777-7777-000000000001"), 990000m, new Guid("00000000-0000-0000-0000-000000000014"), null, null, null, 0, 45, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000069"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000005"), true, null, null, new Guid("11111111-1111-1111-1111-111111111111"), new Guid("77777777-7777-7777-7777-000000000001"), 620000m, new Guid("00000000-0000-0000-0000-000000000014"), null, null, null, 0, 22, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000070"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000007"), true, null, null, new Guid("33333333-3333-3333-3333-333333333333"), new Guid("77777777-7777-7777-7777-000000000001"), 610000m, new Guid("00000000-0000-0000-0000-000000000014"), null, null, null, 0, 31, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000071"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000004"), true, null, null, new Guid("88888888-8888-8888-8888-888888888888"), new Guid("77777777-7777-7777-7777-000000000001"), 690000m, new Guid("00000000-0000-0000-0000-000000000015"), null, null, null, 0, 8, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000072"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000006"), true, null, null, new Guid("22222222-2222-2222-2222-222222222222"), new Guid("77777777-7777-7777-7777-000000000001"), 510000m, new Guid("00000000-0000-0000-0000-000000000015"), null, null, null, 0, 31, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000073"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000008"), true, null, null, new Guid("44444444-4444-4444-4444-444444444444"), new Guid("77777777-7777-7777-7777-000000000001"), 680000m, new Guid("00000000-0000-0000-0000-000000000015"), null, null, null, 0, 12, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000074"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000010"), true, null, null, new Guid("66666666-6666-6666-6666-666666666666"), new Guid("77777777-7777-7777-7777-000000000001"), 710000m, new Guid("00000000-0000-0000-0000-000000000015"), null, null, null, 0, 44, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000075"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000012"), true, null, null, new Guid("88888888-8888-8888-8888-888888888888"), new Guid("77777777-7777-7777-7777-000000000001"), 730000m, new Guid("00000000-0000-0000-0000-000000000015"), null, null, null, 0, 31, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000076"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000009"), true, null, null, new Guid("55555555-5555-5555-5555-555555555555"), new Guid("77777777-7777-7777-7777-000000000001"), 680000m, new Guid("00000000-0000-0000-0000-000000000016"), null, null, null, 0, 21, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000077"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000011"), true, null, null, new Guid("77777777-7777-7777-7777-777777777777"), new Guid("77777777-7777-7777-7777-000000000001"), 880000m, new Guid("00000000-0000-0000-0000-000000000016"), null, null, null, 0, 35, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000078"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000003"), true, null, null, new Guid("11111111-1111-1111-1111-111111111111"), new Guid("77777777-7777-7777-7777-000000000001"), 860000m, new Guid("00000000-0000-0000-0000-000000000016"), null, null, null, 0, 24, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000079"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000005"), true, null, null, new Guid("33333333-3333-3333-3333-333333333333"), new Guid("77777777-7777-7777-7777-000000000001"), 880000m, new Guid("00000000-0000-0000-0000-000000000016"), null, null, null, 0, 33, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000080"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000007"), true, null, null, new Guid("55555555-5555-5555-5555-555555555555"), new Guid("77777777-7777-7777-7777-000000000001"), 790000m, new Guid("00000000-0000-0000-0000-000000000016"), null, null, null, 0, 22, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000081"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000004"), true, null, null, new Guid("22222222-2222-2222-2222-222222222222"), new Guid("77777777-7777-7777-7777-000000000001"), 670000m, new Guid("00000000-0000-0000-0000-000000000017"), null, null, null, 0, 32, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000082"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000006"), true, null, null, new Guid("44444444-4444-4444-4444-444444444444"), new Guid("77777777-7777-7777-7777-000000000001"), 540000m, new Guid("00000000-0000-0000-0000-000000000017"), null, null, null, 0, 33, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000083"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000008"), true, null, null, new Guid("66666666-6666-6666-6666-666666666666"), new Guid("77777777-7777-7777-7777-000000000001"), 620000m, new Guid("00000000-0000-0000-0000-000000000017"), null, null, null, 0, 19, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000084"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000010"), true, null, null, new Guid("88888888-8888-8888-8888-888888888888"), new Guid("77777777-7777-7777-7777-000000000001"), 720000m, new Guid("00000000-0000-0000-0000-000000000017"), null, null, null, 0, 30, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000085"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000012"), true, null, null, new Guid("22222222-2222-2222-2222-222222222222"), new Guid("77777777-7777-7777-7777-000000000001"), 590000m, new Guid("00000000-0000-0000-0000-000000000017"), null, null, null, 0, 13, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000086"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000009"), true, null, null, new Guid("77777777-7777-7777-7777-777777777777"), new Guid("77777777-7777-7777-7777-000000000001"), 860000m, new Guid("00000000-0000-0000-0000-000000000018"), null, null, null, 0, 29, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000087"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000011"), true, null, null, new Guid("11111111-1111-1111-1111-111111111111"), new Guid("77777777-7777-7777-7777-000000000001"), 900000m, new Guid("00000000-0000-0000-0000-000000000018"), null, null, null, 0, 7, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000088"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000003"), true, null, null, new Guid("33333333-3333-3333-3333-333333333333"), new Guid("77777777-7777-7777-7777-000000000001"), 640000m, new Guid("00000000-0000-0000-0000-000000000018"), null, null, null, 0, 40, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000089"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000005"), true, null, null, new Guid("55555555-5555-5555-5555-555555555555"), new Guid("77777777-7777-7777-7777-000000000001"), 840000m, new Guid("00000000-0000-0000-0000-000000000018"), null, null, null, 0, 39, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000090"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000007"), true, null, null, new Guid("77777777-7777-7777-7777-777777777777"), new Guid("77777777-7777-7777-7777-000000000001"), 920000m, new Guid("00000000-0000-0000-0000-000000000018"), null, null, null, 0, 25, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000091"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000004"), true, null, null, new Guid("44444444-4444-4444-4444-444444444444"), new Guid("77777777-7777-7777-7777-000000000001"), 510000m, new Guid("00000000-0000-0000-0000-000000000019"), null, null, null, 0, 19, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000092"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000006"), true, null, null, new Guid("66666666-6666-6666-6666-666666666666"), new Guid("77777777-7777-7777-7777-000000000001"), 700000m, new Guid("00000000-0000-0000-0000-000000000019"), null, null, null, 0, 18, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000093"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000008"), true, null, null, new Guid("88888888-8888-8888-8888-888888888888"), new Guid("77777777-7777-7777-7777-000000000001"), 790000m, new Guid("00000000-0000-0000-0000-000000000019"), null, null, null, 0, 24, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000094"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000010"), true, null, null, new Guid("22222222-2222-2222-2222-222222222222"), new Guid("77777777-7777-7777-7777-000000000001"), 630000m, new Guid("00000000-0000-0000-0000-000000000019"), null, null, null, 0, 32, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000095"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000012"), true, null, null, new Guid("44444444-4444-4444-4444-444444444444"), new Guid("77777777-7777-7777-7777-000000000001"), 630000m, new Guid("00000000-0000-0000-0000-000000000019"), null, null, null, 0, 29, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000096"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000009"), true, null, null, new Guid("11111111-1111-1111-1111-111111111111"), new Guid("77777777-7777-7777-7777-000000000001"), 560000m, new Guid("00000000-0000-0000-0000-000000000020"), null, null, null, 0, 31, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000097"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000011"), true, null, null, new Guid("33333333-3333-3333-3333-333333333333"), new Guid("77777777-7777-7777-7777-000000000001"), 900000m, new Guid("00000000-0000-0000-0000-000000000020"), null, null, null, 0, 43, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000098"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000003"), true, null, null, new Guid("55555555-5555-5555-5555-555555555555"), new Guid("77777777-7777-7777-7777-000000000001"), 610000m, new Guid("00000000-0000-0000-0000-000000000020"), null, null, null, 0, 40, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000099"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000005"), true, null, null, new Guid("77777777-7777-7777-7777-777777777777"), new Guid("77777777-7777-7777-7777-000000000001"), 990000m, new Guid("00000000-0000-0000-0000-000000000020"), null, null, null, 0, 22, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000100"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000007"), true, null, null, new Guid("11111111-1111-1111-1111-111111111111"), new Guid("77777777-7777-7777-7777-000000000001"), 670000m, new Guid("00000000-0000-0000-0000-000000000020"), null, null, null, 0, 7, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000101"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000004"), true, null, null, new Guid("66666666-6666-6666-6666-666666666666"), new Guid("77777777-7777-7777-7777-000000000001"), 850000m, new Guid("00000000-0000-0000-0000-000000000021"), null, null, null, 0, 43, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000102"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000006"), true, null, null, new Guid("88888888-8888-8888-8888-888888888888"), new Guid("77777777-7777-7777-7777-000000000001"), 830000m, new Guid("00000000-0000-0000-0000-000000000021"), null, null, null, 0, 17, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000103"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000008"), true, null, null, new Guid("22222222-2222-2222-2222-222222222222"), new Guid("77777777-7777-7777-7777-000000000001"), 930000m, new Guid("00000000-0000-0000-0000-000000000021"), null, null, null, 0, 42, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000104"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000010"), true, null, null, new Guid("44444444-4444-4444-4444-444444444444"), new Guid("77777777-7777-7777-7777-000000000001"), 770000m, new Guid("00000000-0000-0000-0000-000000000021"), null, null, null, 0, 37, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000105"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000012"), true, null, null, new Guid("66666666-6666-6666-6666-666666666666"), new Guid("77777777-7777-7777-7777-000000000001"), 680000m, new Guid("00000000-0000-0000-0000-000000000021"), null, null, null, 0, 47, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000106"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000009"), true, null, null, new Guid("33333333-3333-3333-3333-333333333333"), new Guid("77777777-7777-7777-7777-000000000001"), 870000m, new Guid("00000000-0000-0000-0000-000000000022"), null, null, null, 0, 49, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000107"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000011"), true, null, null, new Guid("55555555-5555-5555-5555-555555555555"), new Guid("77777777-7777-7777-7777-000000000001"), 890000m, new Guid("00000000-0000-0000-0000-000000000022"), null, null, null, 0, 10, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000108"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000003"), true, null, null, new Guid("77777777-7777-7777-7777-777777777777"), new Guid("77777777-7777-7777-7777-000000000001"), 680000m, new Guid("00000000-0000-0000-0000-000000000022"), null, null, null, 0, 6, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000109"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000005"), true, null, null, new Guid("11111111-1111-1111-1111-111111111111"), new Guid("77777777-7777-7777-7777-000000000001"), 600000m, new Guid("00000000-0000-0000-0000-000000000022"), null, null, null, 0, 40, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000110"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000007"), true, null, null, new Guid("33333333-3333-3333-3333-333333333333"), new Guid("77777777-7777-7777-7777-000000000001"), 520000m, new Guid("00000000-0000-0000-0000-000000000022"), null, null, null, 0, 41, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000111"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000004"), true, null, null, new Guid("88888888-8888-8888-8888-888888888888"), new Guid("77777777-7777-7777-7777-000000000001"), 510000m, new Guid("00000000-0000-0000-0000-000000000023"), null, null, null, 0, 43, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000112"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000006"), true, null, null, new Guid("22222222-2222-2222-2222-222222222222"), new Guid("77777777-7777-7777-7777-000000000001"), 640000m, new Guid("00000000-0000-0000-0000-000000000023"), null, null, null, 0, 34, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000113"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000008"), true, null, null, new Guid("44444444-4444-4444-4444-444444444444"), new Guid("77777777-7777-7777-7777-000000000001"), 530000m, new Guid("00000000-0000-0000-0000-000000000023"), null, null, null, 0, 10, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000114"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000010"), true, null, null, new Guid("66666666-6666-6666-6666-666666666666"), new Guid("77777777-7777-7777-7777-000000000001"), 870000m, new Guid("00000000-0000-0000-0000-000000000023"), null, null, null, 0, 48, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000115"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000012"), true, null, null, new Guid("88888888-8888-8888-8888-888888888888"), new Guid("77777777-7777-7777-7777-000000000001"), 910000m, new Guid("00000000-0000-0000-0000-000000000023"), null, null, null, 0, 28, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000116"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000009"), true, null, null, new Guid("55555555-5555-5555-5555-555555555555"), new Guid("77777777-7777-7777-7777-000000000001"), 890000m, new Guid("00000000-0000-0000-0000-000000000024"), null, null, null, 0, 18, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000117"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000011"), true, null, null, new Guid("77777777-7777-7777-7777-777777777777"), new Guid("77777777-7777-7777-7777-000000000001"), 710000m, new Guid("00000000-0000-0000-0000-000000000024"), null, null, null, 0, 40, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000118"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000003"), true, null, null, new Guid("11111111-1111-1111-1111-111111111111"), new Guid("77777777-7777-7777-7777-000000000001"), 870000m, new Guid("00000000-0000-0000-0000-000000000024"), null, null, null, 0, 12, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000119"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000005"), true, null, null, new Guid("33333333-3333-3333-3333-333333333333"), new Guid("77777777-7777-7777-7777-000000000001"), 820000m, new Guid("00000000-0000-0000-0000-000000000024"), null, null, null, 0, 10, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000120"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000007"), true, null, null, new Guid("55555555-5555-5555-5555-555555555555"), new Guid("77777777-7777-7777-7777-000000000001"), 710000m, new Guid("00000000-0000-0000-0000-000000000024"), null, null, null, 0, 39, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000121"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000004"), true, null, null, new Guid("22222222-2222-2222-2222-222222222222"), new Guid("77777777-7777-7777-7777-000000000001"), 940000m, new Guid("00000000-0000-0000-0000-000000000025"), null, null, null, 0, 30, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000122"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000006"), true, null, null, new Guid("44444444-4444-4444-4444-444444444444"), new Guid("77777777-7777-7777-7777-000000000001"), 610000m, new Guid("00000000-0000-0000-0000-000000000025"), null, null, null, 0, 19, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000123"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000008"), true, null, null, new Guid("66666666-6666-6666-6666-666666666666"), new Guid("77777777-7777-7777-7777-000000000001"), 900000m, new Guid("00000000-0000-0000-0000-000000000025"), null, null, null, 0, 10, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000124"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000010"), true, null, null, new Guid("88888888-8888-8888-8888-888888888888"), new Guid("77777777-7777-7777-7777-000000000001"), 900000m, new Guid("00000000-0000-0000-0000-000000000025"), null, null, null, 0, 35, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000125"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000012"), true, null, null, new Guid("22222222-2222-2222-2222-222222222222"), new Guid("77777777-7777-7777-7777-000000000001"), 740000m, new Guid("00000000-0000-0000-0000-000000000025"), null, null, null, 0, 14, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000126"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000009"), true, null, null, new Guid("77777777-7777-7777-7777-777777777777"), new Guid("77777777-7777-7777-7777-000000000001"), 780000m, new Guid("00000000-0000-0000-0000-000000000026"), null, null, null, 0, 13, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000127"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000011"), true, null, null, new Guid("11111111-1111-1111-1111-111111111111"), new Guid("77777777-7777-7777-7777-000000000001"), 670000m, new Guid("00000000-0000-0000-0000-000000000026"), null, null, null, 0, 29, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000128"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000003"), true, null, null, new Guid("33333333-3333-3333-3333-333333333333"), new Guid("77777777-7777-7777-7777-000000000001"), 500000m, new Guid("00000000-0000-0000-0000-000000000026"), null, null, null, 0, 43, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000129"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000005"), true, null, null, new Guid("55555555-5555-5555-5555-555555555555"), new Guid("77777777-7777-7777-7777-000000000001"), 780000m, new Guid("00000000-0000-0000-0000-000000000026"), null, null, null, 0, 6, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000130"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000007"), true, null, null, new Guid("77777777-7777-7777-7777-777777777777"), new Guid("77777777-7777-7777-7777-000000000001"), 600000m, new Guid("00000000-0000-0000-0000-000000000026"), null, null, null, 0, 38, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000131"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000004"), true, null, null, new Guid("44444444-4444-4444-4444-444444444444"), new Guid("77777777-7777-7777-7777-000000000001"), 530000m, new Guid("00000000-0000-0000-0000-000000000027"), null, null, null, 0, 31, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000132"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000006"), true, null, null, new Guid("66666666-6666-6666-6666-666666666666"), new Guid("77777777-7777-7777-7777-000000000001"), 940000m, new Guid("00000000-0000-0000-0000-000000000027"), null, null, null, 0, 43, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000133"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000008"), true, null, null, new Guid("88888888-8888-8888-8888-888888888888"), new Guid("77777777-7777-7777-7777-000000000001"), 570000m, new Guid("00000000-0000-0000-0000-000000000027"), null, null, null, 0, 24, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000134"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000010"), true, null, null, new Guid("22222222-2222-2222-2222-222222222222"), new Guid("77777777-7777-7777-7777-000000000001"), 780000m, new Guid("00000000-0000-0000-0000-000000000027"), null, null, null, 0, 49, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000135"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000012"), true, null, null, new Guid("44444444-4444-4444-4444-444444444444"), new Guid("77777777-7777-7777-7777-000000000001"), 690000m, new Guid("00000000-0000-0000-0000-000000000027"), null, null, null, 0, 14, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000136"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000009"), true, null, null, new Guid("11111111-1111-1111-1111-111111111111"), new Guid("77777777-7777-7777-7777-000000000001"), 690000m, new Guid("00000000-0000-0000-0000-000000000028"), null, null, null, 0, 8, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000137"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000011"), true, null, null, new Guid("33333333-3333-3333-3333-333333333333"), new Guid("77777777-7777-7777-7777-000000000001"), 670000m, new Guid("00000000-0000-0000-0000-000000000028"), null, null, null, 0, 17, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000138"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000003"), true, null, null, new Guid("55555555-5555-5555-5555-555555555555"), new Guid("77777777-7777-7777-7777-000000000001"), 960000m, new Guid("00000000-0000-0000-0000-000000000028"), null, null, null, 0, 26, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000139"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000005"), true, null, null, new Guid("77777777-7777-7777-7777-777777777777"), new Guid("77777777-7777-7777-7777-000000000001"), 810000m, new Guid("00000000-0000-0000-0000-000000000028"), null, null, null, 0, 48, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000140"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000007"), true, null, null, new Guid("11111111-1111-1111-1111-111111111111"), new Guid("77777777-7777-7777-7777-000000000001"), 920000m, new Guid("00000000-0000-0000-0000-000000000028"), null, null, null, 0, 47, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000141"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000004"), true, null, null, new Guid("66666666-6666-6666-6666-666666666666"), new Guid("77777777-7777-7777-7777-000000000001"), 660000m, new Guid("00000000-0000-0000-0000-000000000029"), null, null, null, 0, 8, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000142"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000006"), true, null, null, new Guid("88888888-8888-8888-8888-888888888888"), new Guid("77777777-7777-7777-7777-000000000001"), 740000m, new Guid("00000000-0000-0000-0000-000000000029"), null, null, null, 0, 32, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000143"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000008"), true, null, null, new Guid("22222222-2222-2222-2222-222222222222"), new Guid("77777777-7777-7777-7777-000000000001"), 970000m, new Guid("00000000-0000-0000-0000-000000000029"), null, null, null, 0, 31, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000144"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000010"), true, null, null, new Guid("44444444-4444-4444-4444-444444444444"), new Guid("77777777-7777-7777-7777-000000000001"), 980000m, new Guid("00000000-0000-0000-0000-000000000029"), null, null, null, 0, 44, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000145"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000012"), true, null, null, new Guid("66666666-6666-6666-6666-666666666666"), new Guid("77777777-7777-7777-7777-000000000001"), 890000m, new Guid("00000000-0000-0000-0000-000000000029"), null, null, null, 0, 45, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000146"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000009"), true, null, null, new Guid("33333333-3333-3333-3333-333333333333"), new Guid("77777777-7777-7777-7777-000000000001"), 790000m, new Guid("00000000-0000-0000-0000-000000000030"), null, null, null, 0, 32, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000147"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000011"), true, null, null, new Guid("55555555-5555-5555-5555-555555555555"), new Guid("77777777-7777-7777-7777-000000000001"), 950000m, new Guid("00000000-0000-0000-0000-000000000030"), null, null, null, 0, 36, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000148"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000003"), true, null, null, new Guid("77777777-7777-7777-7777-777777777777"), new Guid("77777777-7777-7777-7777-000000000001"), 840000m, new Guid("00000000-0000-0000-0000-000000000030"), null, null, null, 0, 18, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000149"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000005"), true, null, null, new Guid("11111111-1111-1111-1111-111111111111"), new Guid("77777777-7777-7777-7777-000000000001"), 830000m, new Guid("00000000-0000-0000-0000-000000000030"), null, null, null, 0, 22, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000150"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000007"), true, null, null, new Guid("33333333-3333-3333-3333-333333333333"), new Guid("77777777-7777-7777-7777-000000000001"), 580000m, new Guid("00000000-0000-0000-0000-000000000030"), null, null, null, 0, 21, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000151"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000004"), true, null, null, new Guid("88888888-8888-8888-8888-888888888888"), new Guid("77777777-7777-7777-7777-000000000001"), 770000m, new Guid("00000000-0000-0000-0000-000000000031"), null, null, null, 0, 41, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000152"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000006"), true, null, null, new Guid("22222222-2222-2222-2222-222222222222"), new Guid("77777777-7777-7777-7777-000000000001"), 640000m, new Guid("00000000-0000-0000-0000-000000000031"), null, null, null, 0, 17, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000153"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000008"), true, null, null, new Guid("44444444-4444-4444-4444-444444444444"), new Guid("77777777-7777-7777-7777-000000000001"), 910000m, new Guid("00000000-0000-0000-0000-000000000031"), null, null, null, 0, 26, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000154"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000010"), true, null, null, new Guid("66666666-6666-6666-6666-666666666666"), new Guid("77777777-7777-7777-7777-000000000001"), 920000m, new Guid("00000000-0000-0000-0000-000000000031"), null, null, null, 0, 7, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000155"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000012"), true, null, null, new Guid("88888888-8888-8888-8888-888888888888"), new Guid("77777777-7777-7777-7777-000000000001"), 800000m, new Guid("00000000-0000-0000-0000-000000000031"), null, null, null, 0, 29, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000156"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000009"), true, null, null, new Guid("55555555-5555-5555-5555-555555555555"), new Guid("77777777-7777-7777-7777-000000000001"), 610000m, new Guid("00000000-0000-0000-0000-000000000032"), null, null, null, 0, 32, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000157"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000011"), true, null, null, new Guid("77777777-7777-7777-7777-777777777777"), new Guid("77777777-7777-7777-7777-000000000001"), 590000m, new Guid("00000000-0000-0000-0000-000000000032"), null, null, null, 0, 16, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000158"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000003"), true, null, null, new Guid("11111111-1111-1111-1111-111111111111"), new Guid("77777777-7777-7777-7777-000000000001"), 700000m, new Guid("00000000-0000-0000-0000-000000000032"), null, null, null, 0, 5, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000159"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000005"), true, null, null, new Guid("33333333-3333-3333-3333-333333333333"), new Guid("77777777-7777-7777-7777-000000000001"), 540000m, new Guid("00000000-0000-0000-0000-000000000032"), null, null, null, 0, 17, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000160"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000007"), true, null, null, new Guid("55555555-5555-5555-5555-555555555555"), new Guid("77777777-7777-7777-7777-000000000001"), 950000m, new Guid("00000000-0000-0000-0000-000000000032"), null, null, null, 0, 29, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000161"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000004"), true, null, null, new Guid("22222222-2222-2222-2222-222222222222"), new Guid("77777777-7777-7777-7777-000000000001"), 730000m, new Guid("00000000-0000-0000-0000-000000000033"), null, null, null, 0, 36, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000162"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000006"), true, null, null, new Guid("44444444-4444-4444-4444-444444444444"), new Guid("77777777-7777-7777-7777-000000000001"), 600000m, new Guid("00000000-0000-0000-0000-000000000033"), null, null, null, 0, 27, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000163"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000008"), true, null, null, new Guid("66666666-6666-6666-6666-666666666666"), new Guid("77777777-7777-7777-7777-000000000001"), 800000m, new Guid("00000000-0000-0000-0000-000000000033"), null, null, null, 0, 39, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000164"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000010"), true, null, null, new Guid("88888888-8888-8888-8888-888888888888"), new Guid("77777777-7777-7777-7777-000000000001"), 580000m, new Guid("00000000-0000-0000-0000-000000000033"), null, null, null, 0, 34, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000165"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000012"), true, null, null, new Guid("22222222-2222-2222-2222-222222222222"), new Guid("77777777-7777-7777-7777-000000000001"), 790000m, new Guid("00000000-0000-0000-0000-000000000033"), null, null, null, 0, 33, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000166"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000009"), true, null, null, new Guid("77777777-7777-7777-7777-777777777777"), new Guid("77777777-7777-7777-7777-000000000001"), 900000m, new Guid("00000000-0000-0000-0000-000000000034"), null, null, null, 0, 15, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000167"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000011"), true, null, null, new Guid("11111111-1111-1111-1111-111111111111"), new Guid("77777777-7777-7777-7777-000000000001"), 890000m, new Guid("00000000-0000-0000-0000-000000000034"), null, null, null, 0, 26, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000168"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000003"), true, null, null, new Guid("33333333-3333-3333-3333-333333333333"), new Guid("77777777-7777-7777-7777-000000000001"), 690000m, new Guid("00000000-0000-0000-0000-000000000034"), null, null, null, 0, 28, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000169"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000005"), true, null, null, new Guid("55555555-5555-5555-5555-555555555555"), new Guid("77777777-7777-7777-7777-000000000001"), 880000m, new Guid("00000000-0000-0000-0000-000000000034"), null, null, null, 0, 14, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000170"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000007"), true, null, null, new Guid("77777777-7777-7777-7777-777777777777"), new Guid("77777777-7777-7777-7777-000000000001"), 890000m, new Guid("00000000-0000-0000-0000-000000000034"), null, null, null, 0, 25, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000171"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000004"), true, null, null, new Guid("44444444-4444-4444-4444-444444444444"), new Guid("77777777-7777-7777-7777-000000000001"), 870000m, new Guid("00000000-0000-0000-0000-000000000035"), null, null, null, 0, 45, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000172"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000006"), true, null, null, new Guid("66666666-6666-6666-6666-666666666666"), new Guid("77777777-7777-7777-7777-000000000001"), 630000m, new Guid("00000000-0000-0000-0000-000000000035"), null, null, null, 0, 16, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000173"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000008"), true, null, null, new Guid("88888888-8888-8888-8888-888888888888"), new Guid("77777777-7777-7777-7777-000000000001"), 830000m, new Guid("00000000-0000-0000-0000-000000000035"), null, null, null, 0, 49, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000174"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000010"), true, null, null, new Guid("22222222-2222-2222-2222-222222222222"), new Guid("77777777-7777-7777-7777-000000000001"), 710000m, new Guid("00000000-0000-0000-0000-000000000035"), null, null, null, 0, 34, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000175"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000012"), true, null, null, new Guid("44444444-4444-4444-4444-444444444444"), new Guid("77777777-7777-7777-7777-000000000001"), 640000m, new Guid("00000000-0000-0000-0000-000000000035"), null, null, null, 0, 35, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000176"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000009"), true, null, null, new Guid("11111111-1111-1111-1111-111111111111"), new Guid("77777777-7777-7777-7777-000000000001"), 600000m, new Guid("00000000-0000-0000-0000-000000000036"), null, null, null, 0, 22, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000177"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000011"), true, null, null, new Guid("33333333-3333-3333-3333-333333333333"), new Guid("77777777-7777-7777-7777-000000000001"), 730000m, new Guid("00000000-0000-0000-0000-000000000036"), null, null, null, 0, 33, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000178"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000003"), true, null, null, new Guid("55555555-5555-5555-5555-555555555555"), new Guid("77777777-7777-7777-7777-000000000001"), 940000m, new Guid("00000000-0000-0000-0000-000000000036"), null, null, null, 0, 43, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000179"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000005"), true, null, null, new Guid("77777777-7777-7777-7777-777777777777"), new Guid("77777777-7777-7777-7777-000000000001"), 790000m, new Guid("00000000-0000-0000-0000-000000000036"), null, null, null, 0, 40, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000180"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000007"), true, null, null, new Guid("11111111-1111-1111-1111-111111111111"), new Guid("77777777-7777-7777-7777-000000000001"), 820000m, new Guid("00000000-0000-0000-0000-000000000036"), null, null, null, 0, 7, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000181"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000004"), true, null, null, new Guid("66666666-6666-6666-6666-666666666666"), new Guid("77777777-7777-7777-7777-000000000001"), 660000m, new Guid("00000000-0000-0000-0000-000000000037"), null, null, null, 0, 14, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000182"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000006"), true, null, null, new Guid("88888888-8888-8888-8888-888888888888"), new Guid("77777777-7777-7777-7777-000000000001"), 730000m, new Guid("00000000-0000-0000-0000-000000000037"), null, null, null, 0, 49, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000183"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000008"), true, null, null, new Guid("22222222-2222-2222-2222-222222222222"), new Guid("77777777-7777-7777-7777-000000000001"), 860000m, new Guid("00000000-0000-0000-0000-000000000037"), null, null, null, 0, 49, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000184"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000010"), true, null, null, new Guid("44444444-4444-4444-4444-444444444444"), new Guid("77777777-7777-7777-7777-000000000001"), 900000m, new Guid("00000000-0000-0000-0000-000000000037"), null, null, null, 0, 37, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000185"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000012"), true, null, null, new Guid("66666666-6666-6666-6666-666666666666"), new Guid("77777777-7777-7777-7777-000000000001"), 930000m, new Guid("00000000-0000-0000-0000-000000000037"), null, null, null, 0, 45, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000186"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000009"), true, null, null, new Guid("33333333-3333-3333-3333-333333333333"), new Guid("77777777-7777-7777-7777-000000000001"), 610000m, new Guid("00000000-0000-0000-0000-000000000038"), null, null, null, 0, 44, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000187"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000011"), true, null, null, new Guid("55555555-5555-5555-5555-555555555555"), new Guid("77777777-7777-7777-7777-000000000001"), 740000m, new Guid("00000000-0000-0000-0000-000000000038"), null, null, null, 0, 24, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000188"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000003"), true, null, null, new Guid("77777777-7777-7777-7777-777777777777"), new Guid("77777777-7777-7777-7777-000000000001"), 900000m, new Guid("00000000-0000-0000-0000-000000000038"), null, null, null, 0, 30, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000189"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000005"), true, null, null, new Guid("11111111-1111-1111-1111-111111111111"), new Guid("77777777-7777-7777-7777-000000000001"), 710000m, new Guid("00000000-0000-0000-0000-000000000038"), null, null, null, 0, 48, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000190"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000007"), true, null, null, new Guid("33333333-3333-3333-3333-333333333333"), new Guid("77777777-7777-7777-7777-000000000001"), 910000m, new Guid("00000000-0000-0000-0000-000000000038"), null, null, null, 0, 33, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000191"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000004"), true, null, null, new Guid("88888888-8888-8888-8888-888888888888"), new Guid("77777777-7777-7777-7777-000000000001"), 670000m, new Guid("00000000-0000-0000-0000-000000000039"), null, null, null, 0, 27, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000192"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000006"), true, null, null, new Guid("22222222-2222-2222-2222-222222222222"), new Guid("77777777-7777-7777-7777-000000000001"), 670000m, new Guid("00000000-0000-0000-0000-000000000039"), null, null, null, 0, 45, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000193"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000008"), true, null, null, new Guid("44444444-4444-4444-4444-444444444444"), new Guid("77777777-7777-7777-7777-000000000001"), 700000m, new Guid("00000000-0000-0000-0000-000000000039"), null, null, null, 0, 24, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000194"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000010"), true, null, null, new Guid("66666666-6666-6666-6666-666666666666"), new Guid("77777777-7777-7777-7777-000000000001"), 880000m, new Guid("00000000-0000-0000-0000-000000000039"), null, null, null, 0, 12, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000195"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000012"), true, null, null, new Guid("88888888-8888-8888-8888-888888888888"), new Guid("77777777-7777-7777-7777-000000000001"), 790000m, new Guid("00000000-0000-0000-0000-000000000039"), null, null, null, 0, 29, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000196"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000009"), true, null, null, new Guid("55555555-5555-5555-5555-555555555555"), new Guid("77777777-7777-7777-7777-000000000001"), 950000m, new Guid("00000000-0000-0000-0000-000000000040"), null, null, null, 0, 49, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000197"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000011"), true, null, null, new Guid("77777777-7777-7777-7777-777777777777"), new Guid("77777777-7777-7777-7777-000000000001"), 780000m, new Guid("00000000-0000-0000-0000-000000000040"), null, null, null, 0, 38, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000198"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000003"), true, null, null, new Guid("11111111-1111-1111-1111-111111111111"), new Guid("77777777-7777-7777-7777-000000000001"), 560000m, new Guid("00000000-0000-0000-0000-000000000040"), null, null, null, 0, 29, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000199"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000005"), true, null, null, new Guid("33333333-3333-3333-3333-333333333333"), new Guid("77777777-7777-7777-7777-000000000001"), 710000m, new Guid("00000000-0000-0000-0000-000000000040"), null, null, null, 0, 18, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000200"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000007"), true, null, null, new Guid("55555555-5555-5555-5555-555555555555"), new Guid("77777777-7777-7777-7777-000000000001"), 760000m, new Guid("00000000-0000-0000-0000-000000000040"), null, null, null, 0, 35, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000201"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000004"), true, null, null, new Guid("22222222-2222-2222-2222-222222222222"), new Guid("77777777-7777-7777-7777-000000000001"), 580000m, new Guid("00000000-0000-0000-0000-000000000041"), null, null, null, 0, 36, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000202"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000006"), true, null, null, new Guid("44444444-4444-4444-4444-444444444444"), new Guid("77777777-7777-7777-7777-000000000001"), 890000m, new Guid("00000000-0000-0000-0000-000000000041"), null, null, null, 0, 22, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000203"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000008"), true, null, null, new Guid("66666666-6666-6666-6666-666666666666"), new Guid("77777777-7777-7777-7777-000000000001"), 720000m, new Guid("00000000-0000-0000-0000-000000000041"), null, null, null, 0, 20, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000204"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000010"), true, null, null, new Guid("88888888-8888-8888-8888-888888888888"), new Guid("77777777-7777-7777-7777-000000000001"), 940000m, new Guid("00000000-0000-0000-0000-000000000041"), null, null, null, 0, 6, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000205"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000012"), true, null, null, new Guid("22222222-2222-2222-2222-222222222222"), new Guid("77777777-7777-7777-7777-000000000001"), 910000m, new Guid("00000000-0000-0000-0000-000000000041"), null, null, null, 0, 18, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000206"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000009"), true, null, null, new Guid("77777777-7777-7777-7777-777777777777"), new Guid("77777777-7777-7777-7777-000000000001"), 710000m, new Guid("00000000-0000-0000-0000-000000000042"), null, null, null, 0, 33, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000207"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000011"), true, null, null, new Guid("11111111-1111-1111-1111-111111111111"), new Guid("77777777-7777-7777-7777-000000000001"), 970000m, new Guid("00000000-0000-0000-0000-000000000042"), null, null, null, 0, 5, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000208"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000003"), true, null, null, new Guid("33333333-3333-3333-3333-333333333333"), new Guid("77777777-7777-7777-7777-000000000001"), 850000m, new Guid("00000000-0000-0000-0000-000000000042"), null, null, null, 0, 41, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000209"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000005"), true, null, null, new Guid("55555555-5555-5555-5555-555555555555"), new Guid("77777777-7777-7777-7777-000000000001"), 920000m, new Guid("00000000-0000-0000-0000-000000000042"), null, null, null, 0, 30, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000210"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000007"), true, null, null, new Guid("77777777-7777-7777-7777-777777777777"), new Guid("77777777-7777-7777-7777-000000000001"), 780000m, new Guid("00000000-0000-0000-0000-000000000042"), null, null, null, 0, 17, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000211"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000004"), true, null, null, new Guid("44444444-4444-4444-4444-444444444444"), new Guid("77777777-7777-7777-7777-000000000001"), 610000m, new Guid("00000000-0000-0000-0000-000000000043"), null, null, null, 0, 34, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000212"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000006"), true, null, null, new Guid("66666666-6666-6666-6666-666666666666"), new Guid("77777777-7777-7777-7777-000000000001"), 560000m, new Guid("00000000-0000-0000-0000-000000000043"), null, null, null, 0, 19, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000213"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000008"), true, null, null, new Guid("88888888-8888-8888-8888-888888888888"), new Guid("77777777-7777-7777-7777-000000000001"), 990000m, new Guid("00000000-0000-0000-0000-000000000043"), null, null, null, 0, 15, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000214"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000010"), true, null, null, new Guid("22222222-2222-2222-2222-222222222222"), new Guid("77777777-7777-7777-7777-000000000001"), 650000m, new Guid("00000000-0000-0000-0000-000000000043"), null, null, null, 0, 39, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000215"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000012"), true, null, null, new Guid("44444444-4444-4444-4444-444444444444"), new Guid("77777777-7777-7777-7777-000000000001"), 650000m, new Guid("00000000-0000-0000-0000-000000000043"), null, null, null, 0, 16, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000216"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000009"), true, null, null, new Guid("11111111-1111-1111-1111-111111111111"), new Guid("77777777-7777-7777-7777-000000000001"), 570000m, new Guid("00000000-0000-0000-0000-000000000044"), null, null, null, 0, 10, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000217"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000011"), true, null, null, new Guid("33333333-3333-3333-3333-333333333333"), new Guid("77777777-7777-7777-7777-000000000001"), 720000m, new Guid("00000000-0000-0000-0000-000000000044"), null, null, null, 0, 12, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000218"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000003"), true, null, null, new Guid("55555555-5555-5555-5555-555555555555"), new Guid("77777777-7777-7777-7777-000000000001"), 730000m, new Guid("00000000-0000-0000-0000-000000000044"), null, null, null, 0, 33, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000219"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000005"), true, null, null, new Guid("77777777-7777-7777-7777-777777777777"), new Guid("77777777-7777-7777-7777-000000000001"), 860000m, new Guid("00000000-0000-0000-0000-000000000044"), null, null, null, 0, 48, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000220"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000007"), true, null, null, new Guid("11111111-1111-1111-1111-111111111111"), new Guid("77777777-7777-7777-7777-000000000001"), 720000m, new Guid("00000000-0000-0000-0000-000000000044"), null, null, null, 0, 8, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000221"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000004"), true, null, null, new Guid("66666666-6666-6666-6666-666666666666"), new Guid("77777777-7777-7777-7777-000000000001"), 770000m, new Guid("00000000-0000-0000-0000-000000000045"), null, null, null, 0, 37, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000222"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000006"), true, null, null, new Guid("88888888-8888-8888-8888-888888888888"), new Guid("77777777-7777-7777-7777-000000000001"), 660000m, new Guid("00000000-0000-0000-0000-000000000045"), null, null, null, 0, 17, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000223"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000008"), true, null, null, new Guid("22222222-2222-2222-2222-222222222222"), new Guid("77777777-7777-7777-7777-000000000001"), 550000m, new Guid("00000000-0000-0000-0000-000000000045"), null, null, null, 0, 17, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000224"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000010"), true, null, null, new Guid("44444444-4444-4444-4444-444444444444"), new Guid("77777777-7777-7777-7777-000000000001"), 510000m, new Guid("00000000-0000-0000-0000-000000000045"), null, null, null, 0, 29, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000225"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000012"), true, null, null, new Guid("66666666-6666-6666-6666-666666666666"), new Guid("77777777-7777-7777-7777-000000000001"), 510000m, new Guid("00000000-0000-0000-0000-000000000045"), null, null, null, 0, 19, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000226"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000009"), true, null, null, new Guid("33333333-3333-3333-3333-333333333333"), new Guid("77777777-7777-7777-7777-000000000001"), 840000m, new Guid("00000000-0000-0000-0000-000000000046"), null, null, null, 0, 44, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000227"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000011"), true, null, null, new Guid("55555555-5555-5555-5555-555555555555"), new Guid("77777777-7777-7777-7777-000000000001"), 860000m, new Guid("00000000-0000-0000-0000-000000000046"), null, null, null, 0, 15, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000228"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000003"), true, null, null, new Guid("77777777-7777-7777-7777-777777777777"), new Guid("77777777-7777-7777-7777-000000000001"), 720000m, new Guid("00000000-0000-0000-0000-000000000046"), null, null, null, 0, 27, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000229"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000005"), true, null, null, new Guid("11111111-1111-1111-1111-111111111111"), new Guid("77777777-7777-7777-7777-000000000001"), 790000m, new Guid("00000000-0000-0000-0000-000000000046"), null, null, null, 0, 25, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000230"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000007"), true, null, null, new Guid("33333333-3333-3333-3333-333333333333"), new Guid("77777777-7777-7777-7777-000000000001"), 690000m, new Guid("00000000-0000-0000-0000-000000000046"), null, null, null, 0, 14, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000231"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000004"), true, null, null, new Guid("88888888-8888-8888-8888-888888888888"), new Guid("77777777-7777-7777-7777-000000000001"), 520000m, new Guid("00000000-0000-0000-0000-000000000047"), null, null, null, 0, 10, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000232"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000006"), true, null, null, new Guid("22222222-2222-2222-2222-222222222222"), new Guid("77777777-7777-7777-7777-000000000001"), 860000m, new Guid("00000000-0000-0000-0000-000000000047"), null, null, null, 0, 30, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000233"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000008"), true, null, null, new Guid("44444444-4444-4444-4444-444444444444"), new Guid("77777777-7777-7777-7777-000000000001"), 580000m, new Guid("00000000-0000-0000-0000-000000000047"), null, null, null, 0, 18, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000234"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000010"), true, null, null, new Guid("66666666-6666-6666-6666-666666666666"), new Guid("77777777-7777-7777-7777-000000000001"), 590000m, new Guid("00000000-0000-0000-0000-000000000047"), null, null, null, 0, 40, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000235"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000012"), true, null, null, new Guid("88888888-8888-8888-8888-888888888888"), new Guid("77777777-7777-7777-7777-000000000001"), 770000m, new Guid("00000000-0000-0000-0000-000000000047"), null, null, null, 0, 8, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000236"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000009"), true, null, null, new Guid("55555555-5555-5555-5555-555555555555"), new Guid("77777777-7777-7777-7777-000000000001"), 540000m, new Guid("00000000-0000-0000-0000-000000000048"), null, null, null, 0, 44, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000237"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000011"), true, null, null, new Guid("77777777-7777-7777-7777-777777777777"), new Guid("77777777-7777-7777-7777-000000000001"), 550000m, new Guid("00000000-0000-0000-0000-000000000048"), null, null, null, 0, 27, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000238"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000003"), true, null, null, new Guid("11111111-1111-1111-1111-111111111111"), new Guid("77777777-7777-7777-7777-000000000001"), 860000m, new Guid("00000000-0000-0000-0000-000000000048"), null, null, null, 0, 27, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000239"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000005"), true, null, null, new Guid("33333333-3333-3333-3333-333333333333"), new Guid("77777777-7777-7777-7777-000000000001"), 660000m, new Guid("00000000-0000-0000-0000-000000000048"), null, null, null, 0, 42, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000240"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000007"), true, null, null, new Guid("55555555-5555-5555-5555-555555555555"), new Guid("77777777-7777-7777-7777-000000000001"), 610000m, new Guid("00000000-0000-0000-0000-000000000048"), null, null, null, 0, 36, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000241"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000004"), true, null, null, new Guid("22222222-2222-2222-2222-222222222222"), new Guid("77777777-7777-7777-7777-000000000001"), 600000m, new Guid("00000000-0000-0000-0000-000000000049"), null, null, null, 0, 38, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000242"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000006"), true, null, null, new Guid("44444444-4444-4444-4444-444444444444"), new Guid("77777777-7777-7777-7777-000000000001"), 860000m, new Guid("00000000-0000-0000-0000-000000000049"), null, null, null, 0, 49, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000243"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000008"), true, null, null, new Guid("66666666-6666-6666-6666-666666666666"), new Guid("77777777-7777-7777-7777-000000000001"), 780000m, new Guid("00000000-0000-0000-0000-000000000049"), null, null, null, 0, 16, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000244"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000010"), true, null, null, new Guid("88888888-8888-8888-8888-888888888888"), new Guid("77777777-7777-7777-7777-000000000001"), 700000m, new Guid("00000000-0000-0000-0000-000000000049"), null, null, null, 0, 14, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000245"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000012"), true, null, null, new Guid("22222222-2222-2222-2222-222222222222"), new Guid("77777777-7777-7777-7777-000000000001"), 850000m, new Guid("00000000-0000-0000-0000-000000000049"), null, null, null, 0, 48, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000246"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000009"), true, null, null, new Guid("77777777-7777-7777-7777-777777777777"), new Guid("77777777-7777-7777-7777-000000000001"), 520000m, new Guid("00000000-0000-0000-0000-000000000050"), null, null, null, 0, 17, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000247"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000011"), true, null, null, new Guid("11111111-1111-1111-1111-111111111111"), new Guid("77777777-7777-7777-7777-000000000001"), 780000m, new Guid("00000000-0000-0000-0000-000000000050"), null, null, null, 0, 15, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000248"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000003"), true, null, null, new Guid("33333333-3333-3333-3333-333333333333"), new Guid("77777777-7777-7777-7777-000000000001"), 510000m, new Guid("00000000-0000-0000-0000-000000000050"), null, null, null, 0, 23, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000249"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000005"), true, null, null, new Guid("55555555-5555-5555-5555-555555555555"), new Guid("77777777-7777-7777-7777-000000000001"), 500000m, new Guid("00000000-0000-0000-0000-000000000050"), null, null, null, 0, 39, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000250"), null, null, null, null, null, null, new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000007"), true, null, null, new Guid("77777777-7777-7777-7777-777777777777"), new Guid("77777777-7777-7777-7777-000000000001"), 550000m, new Guid("00000000-0000-0000-0000-000000000050"), null, null, null, 0, 40, new Guid("99999999-9999-9999-9999-999999999999"), null, null, null, null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Admin_UserId",
                table: "Admin",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cart_CustomerId",
                table: "Cart",
                column: "CustomerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartDetail_ProductDetailId",
                table: "CartDetail",
                column: "ProductDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_UserId",
                table: "Customer",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employee_UserId",
                table: "Employee",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_CustomerId",
                table: "Order",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_PaymentMethodId",
                table: "Order",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_VoucherId",
                table: "Order",
                column: "VoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_OrderId",
                table: "OrderDetail",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_ProductDetailId",
                table: "OrderDetail",
                column: "ProductDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_BrandId",
                table: "Product",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ProductCode",
                table: "Product",
                column: "ProductCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategory_ProductId",
                table: "ProductCategory",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetail_ColorId",
                table: "ProductDetail",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetail_ElasticityId",
                table: "ProductDetail",
                column: "ElasticityId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetail_HardnessId",
                table: "ProductDetail",
                column: "HardnessId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetail_LengthId",
                table: "ProductDetail",
                column: "LengthId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetail_OriginCountryId",
                table: "ProductDetail",
                column: "OriginCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetail_ProductId",
                table: "ProductDetail",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetail_SurfaceFinishId",
                table: "ProductDetail",
                column: "SurfaceFinishId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImage_ProductDetailId",
                table: "ProductImage",
                column: "ProductDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_PermissionId",
                table: "RolePermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_User_RoleId",
                table: "User",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admin");

            migrationBuilder.DropTable(
                name: "CartDetail");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "OrderDetail");

            migrationBuilder.DropTable(
                name: "ProductCategory");

            migrationBuilder.DropTable(
                name: "ProductImage");

            migrationBuilder.DropTable(
                name: "RolePermission");

            migrationBuilder.DropTable(
                name: "Cart");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "ProductDetail");

            migrationBuilder.DropTable(
                name: "Permission");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "PaymentMethod");

            migrationBuilder.DropTable(
                name: "Voucher");

            migrationBuilder.DropTable(
                name: "Color");

            migrationBuilder.DropTable(
                name: "Elasticity");

            migrationBuilder.DropTable(
                name: "Hardness");

            migrationBuilder.DropTable(
                name: "Length");

            migrationBuilder.DropTable(
                name: "OriginCountry");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "SurfaceFinish");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Brand");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
