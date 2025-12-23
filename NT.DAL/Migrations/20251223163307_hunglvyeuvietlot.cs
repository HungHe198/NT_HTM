using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NT.DAL.Migrations
{
    /// <inheritdoc />
    public partial class hunglvyeuvietlot : Migration
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
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Channel = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
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
                    ReceiverName = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    { new Guid("2bafb8bf-a661-4ef4-9499-dbd459655c80"), "MC", "https://mc.example" },
                    { new Guid("5a482c17-5bfe-4dbf-98be-24b7f4f03a80"), "Bing", "https://b.example" },
                    { new Guid("9ab22734-bae9-429e-bbf6-6baae321a646"), "Daiwa", "https://www.daiwa.com" },
                    { new Guid("abd56e6c-e30f-407a-8a17-e50f38de390c"), "Gama", "https://g.example" },
                    { new Guid("ba521afa-602b-4912-bc0f-1c28a431d26d"), "Haidiao", "https://haidiao.example" },
                    { new Guid("e11dfd95-2327-4187-acf3-01063d7f44f3"), "Shimano", "https://www.shimano.com" }
                });

            migrationBuilder.InsertData(
                table: "Color",
                columns: new[] { "Id", "HexCode", "Name" },
                values: new object[,]
                {
                    { new Guid("66666666-6666-6666-6666-000000000001"), "#000000", "Đen" },
                    { new Guid("79927f97-f7f2-4ecd-bed3-d89a5f9ed414"), "#FFFF00", "Vàng" },
                    { new Guid("7a964316-12ed-4b83-bd75-924e93528f5e"), "#0000FF", "Xanh" },
                    { new Guid("9b701fba-417e-4791-85ad-fd9df6cfe703"), "#FFFFFF", "Trắng" },
                    { new Guid("a0c2ae82-56fb-4af1-8a42-2678ad491853"), "#FF0000", "Đỏ" },
                    { new Guid("e250bc3f-d40a-4b82-823a-fc1ad1526156"), "#808080", "Xám" }
                });

            migrationBuilder.InsertData(
                table: "Elasticity",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("88888888-8888-8888-8888-000000000001"), null, "Đàn hồi tiêu chuẩn" },
                    { new Guid("c335cf47-08b0-4faa-94c0-2d531b76c874"), null, "Đàn hồi cao" }
                });

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
                    { new Guid("88888888-8888-8888-8888-888888888888"), "Cần câu đài 9m", "9m" },
                    { new Guid("fb1b4d03-dbb5-440e-b16c-a60c8181b17a"), "Cần câu đài 3m6", "3m6" }
                });

            migrationBuilder.InsertData(
                table: "OriginCountry",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("66c9552e-8714-4e99-8452-152632333abc"), null, "Trung Quốc" },
                    { new Guid("77777777-7777-7777-7777-000000000001"), null, "Việt Nam" },
                    { new Guid("8d455725-d3a3-47d2-a8ae-1887b2595f87"), null, "Nhật Bản" }
                });

            migrationBuilder.InsertData(
                table: "PaymentMethod",
                columns: new[] { "Id", "Channel", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111001"), 1, "Thanh toán khi nhận hàng (Cash On Delivery)", "COD" },
                    { new Guid("11111111-1111-1111-1111-111111111002"), 2, "Thanh toán bằng tiền mặt tại cửa hàng", "Tiền mặt" },
                    { new Guid("11111111-1111-1111-1111-111111111003"), 2, "Thanh toán qua chuyển khoản ngân hàng", "Chuyển khoản" }
                });

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
                values: new object[,]
                {
                    { new Guid("94ce350e-4bf0-499d-9b80-8c7c0a9510d0"), null, "Sơn mờ" },
                    { new Guid("99999999-9999-9999-9999-999999999999"), null, "Sơn bóng" }
                });

            migrationBuilder.InsertData(
                table: "Voucher",
                columns: new[] { "Id", "Code", "DiscountPercentage", "EndDate", "MaxDiscountAmount", "MaxUsage", "MinOrderAmount", "StartDate" },
                values: new object[,]
                {
                    { new Guid("11111111-aaaa-bbbb-cccc-111111111111"), "WELCOME10", 10m, new DateTime(2026, 1, 23, 23, 33, 7, 366, DateTimeKind.Local).AddTicks(7759), 50000m, 1000, 300000m, new DateTime(2025, 12, 16, 23, 33, 7, 366, DateTimeKind.Local).AddTicks(7731) },
                    { new Guid("22222222-aaaa-bbbb-cccc-222222222222"), "SAVE15", 15m, new DateTime(2026, 2, 23, 23, 33, 7, 366, DateTimeKind.Local).AddTicks(7767), 100000m, 500, 500000m, new DateTime(2025, 12, 20, 23, 33, 7, 366, DateTimeKind.Local).AddTicks(7767) },
                    { new Guid("33333333-aaaa-bbbb-cccc-333333333333"), "BIGSALE25", 25m, new DateTime(2026, 3, 23, 23, 33, 7, 366, DateTimeKind.Local).AddTicks(7771), 200000m, 300, 800000m, new DateTime(2025, 12, 22, 23, 33, 7, 366, DateTimeKind.Local).AddTicks(7770) }
                });

            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "Id", "BrandId", "CreatedBy", "CreatedDate", "Description", "Name", "ProductCode", "SeoDescription", "SeoTitle", "ShortDescription", "Status", "Thumbnail", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("2c7e99bd-94cb-415f-806d-5ac175805818"), new Guid("abd56e6c-e30f-407a-8a17-e50f38de390c"), null, null, null, "Phục Long Săn Hàng", "G01", null, null, null, "1", "/uploads/products/f21a5144-e337-4a4b-ba8f-b65d7786e26b.webp", null, null },
                    { new Guid("3103e99c-5ca0-4093-9687-afb58a02b101"), new Guid("ba521afa-602b-4912-bc0f-1c28a431d26d"), null, null, null, "Đại Phong", "HD01", null, null, null, "1", "/uploads/products/7234a007-a8cd-4c14-b96b-d6e9f25f3414.webp", null, null },
                    { new Guid("3d4087a9-8e60-4adc-9f3a-d3c3e037923b"), new Guid("9ab22734-bae9-429e-bbf6-6baae321a646"), null, null, null, "Hoành Thiên Hạ Limited", "KW01", null, null, null, "1", "/uploads/products/4f662526-8278-41e5-90fa-de03725eede5.webp", null, null },
                    { new Guid("45109257-a076-425e-98ae-3c29b232ea13"), new Guid("ba521afa-602b-4912-bc0f-1c28a431d26d"), null, null, null, "Bất Phàm", "HD03", null, null, null, "1", "/uploads/products/78845cd3-cedf-4e4a-98d5-1df2cabac9d9.webp", null, null },
                    { new Guid("4ae4f11b-3e49-468f-84b7-73d100890f04"), new Guid("9ab22734-bae9-429e-bbf6-6baae321a646"), null, null, null, "Vũ Thiết Sa", "DW04", null, null, "Hangding Vũ thiết", "1", "/uploads/products/f8d0cb97-7780-49e9-bbd7-7683dbfbabb8.jpg", null, null },
                    { new Guid("4fbcad35-dd5e-4e69-811e-b01167a7adda"), new Guid("2bafb8bf-a661-4ef4-9499-dbd459655c80"), null, null, null, "Đại Hải", "HD06", null, null, "cần câu đại hải", "1", "/uploads/products/848ded34-9bbe-4440-963c-5cf3c376bc61.jpg", null, null },
                    { new Guid("62342343-5e30-41f2-8e73-2c13f72fedc7"), new Guid("9ab22734-bae9-429e-bbf6-6baae321a646"), null, null, null, "Tầm Vương", "DW02", null, null, null, "1", "/uploads/products/250d649f-91de-4380-b4cc-62a1b08d4ac6.webp", null, null },
                    { new Guid("a7e70ded-b43d-4439-b57a-90894fd91083"), new Guid("5a482c17-5bfe-4dbf-98be-24b7f4f03a80"), null, null, null, "Long Tịnh", "B01", null, null, null, "1", "/uploads/products/d2263568-4bc7-4885-8b61-abe984608073.jpg", null, null },
                    { new Guid("ad07e3ea-ed59-4c10-a634-dc30f4f17fb0"), new Guid("ba521afa-602b-4912-bc0f-1c28a431d26d"), null, null, null, "Chiến Plus", "HD04", null, null, null, "1", "/uploads/products/7c5bceac-764c-4d00-b510-a733760f605b.webp", null, null },
                    { new Guid("bd482f00-c2a4-4b79-9441-260891ed1744"), new Guid("ba521afa-602b-4912-bc0f-1c28a431d26d"), null, null, null, "Chiến Vũ Lí", "HD02", null, null, null, "1", "/uploads/products/79044a1b-bfb7-4488-86f0-23a0cc443b57.webp", null, null },
                    { new Guid("d4726e64-22ab-4936-8338-3b4b5a54e4dd"), new Guid("2bafb8bf-a661-4ef4-9499-dbd459655c80"), null, null, null, "Phục Ma Bá Đạo", "MC01", null, null, null, "1", "/uploads/products/9c1fc107-bf90-4f99-a323-34394a7cd234.webp", null, null },
                    { new Guid("fc6f1a89-2072-4d30-8782-d61f1c66b425"), new Guid("e11dfd95-2327-4187-acf3-01063d7f44f3"), null, null, null, "Lôi Phong", "SH01", null, null, null, "1", "/uploads/products/41426851-b611-4fe5-abba-b7dde6dae7d4.jpg", null, null }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Email", "Fullname", "PasswordHash", "PhoneNumber", "RoleId", "Status", "Username" },
                values: new object[] { new Guid("11111111-2222-3333-4444-555555555555"), "admin@example.com", "System Administrator", "AQAAAAIAAYagAAAAEPk+sWyMqPT+g5jsL0BiBKxRF2N+Cpn1pGpnfoZgkq2bsbeH5RFDYAPHJyZXk/+APg==", null, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Active", "admin" });

            migrationBuilder.InsertData(
                table: "Admin",
                columns: new[] { "Id", "Position", "Salary", "UserId" },
                values: new object[] { new Guid("22222222-3333-4444-5555-666666666666"), "Administrator", null, new Guid("11111111-2222-3333-4444-555555555555") });

            migrationBuilder.InsertData(
                table: "ProductDetail",
                columns: new[] { "Id", "BalanceLoadDescription", "BalanceLoadPoint", "BalancePoint", "ButtDiameter", "ButtWeight", "CollapsedLength", "ColorId", "CostPrice", "ElasticityId", "HandleType", "HardnessId", "IsActive", "JointType", "LastImportDate", "LengthId", "OriginCountryId", "Price", "ProductId", "RecommendedFishWeight", "RecommendedLine", "Sections", "SoldQuantity", "StockQuantity", "SurfaceFinishId", "TipDiameter", "TipWeight", "TopDiameter", "Warranty", "Weight" },
                values: new object[,]
                {
                    { new Guid("014ffd65-5d70-4534-a11c-245361dafbe8"), null, "8", "7000", "29,5mm", "29g", "121cm", new Guid("a0c2ae82-56fb-4af1-8a42-2678ad491853"), 1100000.00m, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000006"), true, null, new DateTime(2025, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("77777777-7777-7777-7777-777777777777"), new Guid("66c9552e-8714-4e99-8452-152632333abc"), 2300000.00m, new Guid("d4726e64-22ab-4936-8338-3b4b5a54e4dd"), null, null, "8", 2, 43, new Guid("99999999-9999-9999-9999-999999999999"), "1,3mm", "24g", "27,9mm", null, "528g" },
                    { new Guid("049e2071-4402-47e0-b223-a0b224005ce6"), null, "14", "15", "26", "12", "18", new Guid("e250bc3f-d40a-4b82-823a-fc1ad1526156"), 3200000.00m, new Guid("c335cf47-08b0-4faa-94c0-2d531b76c874"), null, new Guid("00000000-0000-0000-0000-000000000007"), true, null, new DateTime(2025, 12, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("66666666-6666-6666-6666-666666666666"), new Guid("77777777-7777-7777-7777-000000000001"), 5000000.00m, new Guid("4ae4f11b-3e49-468f-84b7-73d100890f04"), null, "30m", "13", 22, 27, new Guid("94ce350e-4bf0-499d-9b80-8c7c0a9510d0"), "25", "2", "44", null, "32" },
                    { new Guid("0677c96a-7474-4936-b8d6-24bd1c1ac08b"), null, "9", "6500", "27,2mm", "29g", "120cm", new Guid("a0c2ae82-56fb-4af1-8a42-2678ad491853"), 5900000.00m, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000006"), true, null, new DateTime(2025, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("66666666-6666-6666-6666-666666666666"), new Guid("66c9552e-8714-4e99-8452-152632333abc"), 7180000.00m, new Guid("62342343-5e30-41f2-8e73-2c13f72fedc7"), null, null, "7", 0, 37, new Guid("99999999-9999-9999-9999-999999999999"), "1,4mm", "13g", "24,3mm", null, "339g" },
                    { new Guid("06efd08e-338d-41aa-b6cb-ad5d63a8f412"), null, "7", "4000-5000g", "23,8mm", "18g", "122cm", new Guid("a0c2ae82-56fb-4af1-8a42-2678ad491853"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000005"), true, null, new DateTime(2025, 12, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("33333333-3333-3333-3333-333333333333"), new Guid("66c9552e-8714-4e99-8452-152632333abc"), 1450000.00m, new Guid("d4726e64-22ab-4936-8338-3b4b5a54e4dd"), null, null, "5", 0, 11, new Guid("99999999-9999-9999-9999-999999999999"), "1,6mm", "10g", "20mm", null, "172g" },
                    { new Guid("1d5b0c18-60f3-46a6-9df8-003211c32983"), null, "9", "4000-5000g", "22mm", "18g", "121cm", new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("c335cf47-08b0-4faa-94c0-2d531b76c874"), null, new Guid("00000000-0000-0000-0000-000000000005"), true, null, new DateTime(2025, 12, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("55555555-5555-5555-5555-555555555555"), new Guid("66c9552e-8714-4e99-8452-152632333abc"), 2000000.00m, new Guid("2c7e99bd-94cb-415f-806d-5ac175805818"), null, null, "6", 0, 6, new Guid("99999999-9999-9999-9999-999999999999"), "1,6mm", "10g", "15,3mm", null, "172g" },
                    { new Guid("268d8602-8ffb-4f08-af04-309d9ee7a652"), null, "8", "5000-6000g", null, "26g", "121cm", new Guid("a0c2ae82-56fb-4af1-8a42-2678ad491853"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000006"), true, null, new DateTime(2025, 12, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("66666666-6666-6666-6666-666666666666"), new Guid("66c9552e-8714-4e99-8452-152632333abc"), 2120000.00m, new Guid("d4726e64-22ab-4936-8338-3b4b5a54e4dd"), null, null, "7", 0, 21, new Guid("99999999-9999-9999-9999-999999999999"), "1,6mm", "12g", null, null, "172g" },
                    { new Guid("26d96697-c8ca-4cb9-8314-be1d32386d74"), null, "7", "5000-6000g", "26mm", "18g", "121cm", new Guid("a0c2ae82-56fb-4af1-8a42-2678ad491853"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000006"), true, null, new DateTime(2025, 12, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("44444444-4444-4444-4444-444444444444"), new Guid("66c9552e-8714-4e99-8452-152632333abc"), 1790000.00m, new Guid("d4726e64-22ab-4936-8338-3b4b5a54e4dd"), null, null, "5", 0, 13, new Guid("99999999-9999-9999-9999-999999999999"), null, null, "20mm", null, "172g" },
                    { new Guid("2b77eed7-80c0-42e3-9bc5-94677c01dc99"), null, "8", "5000-6000g", "26mm", "30g", "122cm", new Guid("a0c2ae82-56fb-4af1-8a42-2678ad491853"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000005"), true, null, new DateTime(2025, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("77777777-7777-7777-7777-777777777777"), new Guid("66c9552e-8714-4e99-8452-152632333abc"), 2890000.00m, new Guid("bd482f00-c2a4-4b79-9441-260891ed1744"), null, null, "8", 0, 6, new Guid("99999999-9999-9999-9999-999999999999"), "1,6mm", "12g", "20mm", null, "411g" },
                    { new Guid("333c99b1-b645-4b45-8588-edd50e706c1e"), null, null, "15", "26", "22", "16", new Guid("9b701fba-417e-4791-85ad-fd9df6cfe703"), 3200000.00m, new Guid("c335cf47-08b0-4faa-94c0-2d531b76c874"), null, new Guid("00000000-0000-0000-0000-000000000010"), true, null, new DateTime(2025, 12, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("77777777-7777-7777-7777-777777777777"), new Guid("66c9552e-8714-4e99-8452-152632333abc"), 4500000.00m, new Guid("ad07e3ea-ed59-4c10-a634-dc30f4f17fb0"), null, null, "23", 29, 26, new Guid("99999999-9999-9999-9999-999999999999"), "25", "6", "44", null, "35" },
                    { new Guid("3dbe0697-6b17-4854-a251-05db0b49a684"), null, "8", "7000g", "28,9mm", "18g", "121cm", new Guid("7a964316-12ed-4b83-bd75-924e93528f5e"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000006"), true, null, new DateTime(2025, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("77777777-7777-7777-7777-777777777777"), new Guid("8d455725-d3a3-47d2-a8ae-1887b2595f87"), 9200000.00m, new Guid("62342343-5e30-41f2-8e73-2c13f72fedc7"), null, null, "8", 3, 4, new Guid("99999999-9999-9999-9999-999999999999"), "1,6mm", "10g", "15,3mm", null, "172g" },
                    { new Guid("3dd3a835-efb9-4656-8128-1bccf2020e95"), null, "8", "4000-5000g", "26mm", "20g", "122cm", new Guid("a0c2ae82-56fb-4af1-8a42-2678ad491853"), 1450000.00m, new Guid("c335cf47-08b0-4faa-94c0-2d531b76c874"), null, new Guid("00000000-0000-0000-0000-000000000005"), true, null, new DateTime(2025, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("55555555-5555-5555-5555-555555555555"), new Guid("77777777-7777-7777-7777-000000000001"), 2100000.00m, new Guid("bd482f00-c2a4-4b79-9441-260891ed1744"), null, null, "6", 12, 7, new Guid("99999999-9999-9999-9999-999999999999"), "1,4mm", "10g", "15,3mm", null, "89g" },
                    { new Guid("46a67c25-d91e-4cf6-8c24-3434dbd06a34"), null, "22", "11", "32", "22", "16", new Guid("a0c2ae82-56fb-4af1-8a42-2678ad491853"), 500000.00m, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000011"), true, null, new DateTime(2025, 12, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("55555555-5555-5555-5555-555555555555"), new Guid("77777777-7777-7777-7777-000000000001"), 1200000.00m, new Guid("4ae4f11b-3e49-468f-84b7-73d100890f04"), "5-7kg", "30m", "23", 35, 38, new Guid("94ce350e-4bf0-499d-9b80-8c7c0a9510d0"), "25", "1", "34", null, "35" },
                    { new Guid("487f82fc-6678-4bf1-a3d8-25d2cada8633"), null, "7", "5000", "13,2mm", "15g", "104cm", new Guid("7a964316-12ed-4b83-bd75-924e93528f5e"), 5600000.00m, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000010"), true, null, new DateTime(2025, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("33333333-3333-3333-3333-333333333333"), new Guid("66c9552e-8714-4e99-8452-152632333abc"), 7200000.00m, new Guid("a7e70ded-b43d-4439-b57a-90894fd91083"), null, null, "5", 2, 17, new Guid("99999999-9999-9999-9999-999999999999"), "1,1mm", "8g", "10mm", null, "121,6g" },
                    { new Guid("49219172-62e3-4e6d-bbb8-57b7466a59f4"), null, "14", "11", "32", "26", "14", new Guid("79927f97-f7f2-4ecd-bed3-d89a5f9ed414"), 2800000.00m, new Guid("c335cf47-08b0-4faa-94c0-2d531b76c874"), null, new Guid("00000000-0000-0000-0000-000000000003"), true, null, new DateTime(2025, 12, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("44444444-4444-4444-4444-444444444444"), new Guid("8d455725-d3a3-47d2-a8ae-1887b2595f87"), 4200000.00m, new Guid("4fbcad35-dd5e-4e69-811e-b01167a7adda"), "15-17kg", "30m", "16", 23, 24, new Guid("99999999-9999-9999-9999-999999999999"), "31", "32", "56", null, "53" },
                    { new Guid("55f5a1e5-4168-4170-a1a0-cff392249ca2"), null, "7", "2500-3100g", "22mm", "18g", "108g", new Guid("9b701fba-417e-4791-85ad-fd9df6cfe703"), 300000.00m, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000005"), true, null, new DateTime(2025, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("fb1b4d03-dbb5-440e-b16c-a60c8181b17a"), new Guid("66c9552e-8714-4e99-8452-152632333abc"), 600000.00m, new Guid("3103e99c-5ca0-4093-9687-afb58a02b101"), null, null, "4", 0, 15, new Guid("99999999-9999-9999-9999-999999999999"), "1,5mm", "10g", "15,3mm", null, "89g" },
                    { new Guid("5d1e1667-89f9-4f08-83cc-04d732f78f4d"), null, "8", "5000-6000g", "26mm", "18g", "122cm", new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000005"), true, null, new DateTime(2025, 12, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("33333333-3333-3333-3333-333333333333"), new Guid("66c9552e-8714-4e99-8452-152632333abc"), 900000.00m, new Guid("45109257-a076-425e-98ae-3c29b232ea13"), null, null, "5", 0, 3, new Guid("99999999-9999-9999-9999-999999999999"), "1,6mm", "10g", "20mm", null, "172g" },
                    { new Guid("606d0e6d-5f97-44c8-9223-2466d0f70b04"), null, "8", "5000-6000g", "28,9mm", "30g", "121cm", new Guid("7a964316-12ed-4b83-bd75-924e93528f5e"), 1700000.00m, new Guid("c335cf47-08b0-4faa-94c0-2d531b76c874"), null, new Guid("00000000-0000-0000-0000-000000000006"), true, null, new DateTime(2025, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("88888888-8888-8888-8888-888888888888"), new Guid("66c9552e-8714-4e99-8452-152632333abc"), 2400000.00m, new Guid("ad07e3ea-ed59-4c10-a634-dc30f4f17fb0"), null, null, "9", 14, 70, new Guid("94ce350e-4bf0-499d-9b80-8c7c0a9510d0"), "1,6mm", "15g", "27,7mm", null, "514g" },
                    { new Guid("6e3d5c21-c4c2-4a99-896d-a7cf79088041"), null, "8", "5000-6000g", "26mm", "18g", "121cm", new Guid("7a964316-12ed-4b83-bd75-924e93528f5e"), 1600000.00m, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000005"), true, null, new DateTime(2025, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("66666666-6666-6666-6666-666666666666"), new Guid("66c9552e-8714-4e99-8452-152632333abc"), 2600000.00m, new Guid("bd482f00-c2a4-4b79-9441-260891ed1744"), null, null, "7", 1, 10, new Guid("99999999-9999-9999-9999-999999999999"), "1,6mm", "10g", "20mm", null, "310g" },
                    { new Guid("72ddf35a-f7c9-420e-9dd2-650c6082779d"), null, "7", "2600-3100g", "26mm", "20g", "122cm", new Guid("9b701fba-417e-4791-85ad-fd9df6cfe703"), 700000.00m, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000005"), true, null, new DateTime(2025, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("44444444-4444-4444-4444-444444444444"), new Guid("66c9552e-8714-4e99-8452-152632333abc"), 1000000.00m, new Guid("3103e99c-5ca0-4093-9687-afb58a02b101"), null, null, "5", 25, 27, new Guid("99999999-9999-9999-9999-999999999999"), "1,4mm", "12g", "20mm", null, "172g" },
                    { new Guid("754ca79f-7c36-45f3-8c82-23e2978f1b6a"), null, "7", "6500", "23,4mm", "18g", "118cm", new Guid("79927f97-f7f2-4ecd-bed3-d89a5f9ed414"), 900000.00m, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000006"), true, null, new DateTime(2025, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("55555555-5555-5555-5555-555555555555"), new Guid("8d455725-d3a3-47d2-a8ae-1887b2595f87"), 1500000.00m, new Guid("fc6f1a89-2072-4d30-8782-d61f1c66b425"), null, null, "6", 1, 25, new Guid("94ce350e-4bf0-499d-9b80-8c7c0a9510d0"), "1,5mm", "10g", "18,4mm", null, "216g" },
                    { new Guid("81c268b2-a541-48b0-8e98-73b92512b1ad"), null, "7", "4000-5000g", "23,9", "34g", "121cm", new Guid("7a964316-12ed-4b83-bd75-924e93528f5e"), 1100000.00m, new Guid("c335cf47-08b0-4faa-94c0-2d531b76c874"), null, new Guid("00000000-0000-0000-0000-000000000005"), true, null, new DateTime(2025, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("44444444-4444-4444-4444-444444444444"), new Guid("66c9552e-8714-4e99-8452-152632333abc"), 1700000.00m, new Guid("bd482f00-c2a4-4b79-9441-260891ed1744"), null, null, "5", 10, 15, new Guid("94ce350e-4bf0-499d-9b80-8c7c0a9510d0"), "1,6mm", "15g", "17", null, "165,4g" },
                    { new Guid("8f1f78bd-aac3-4ffe-9b67-a5b0546291f9"), null, "8", "8000", "22,8mm", "24g", "118cm", new Guid("a0c2ae82-56fb-4af1-8a42-2678ad491853"), 1268000.00m, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000006"), true, null, new DateTime(2025, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("55555555-5555-5555-5555-555555555555"), new Guid("66c9552e-8714-4e99-8452-152632333abc"), 1750000.00m, new Guid("2c7e99bd-94cb-415f-806d-5ac175805818"), null, null, "6", 0, 43, new Guid("94ce350e-4bf0-499d-9b80-8c7c0a9510d0"), "1,28mm", "18g", "19,3mm", null, "286g" },
                    { new Guid("b111948c-c3e3-41ce-93c5-831b90897fd7"), null, null, "5000-6000g", "26mm", "18g", "121cm", new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000005"), true, null, new DateTime(2025, 12, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("44444444-4444-4444-4444-444444444444"), new Guid("77777777-7777-7777-7777-000000000001"), 1340000.00m, new Guid("45109257-a076-425e-98ae-3c29b232ea13"), null, null, "5", 0, 23, new Guid("94ce350e-4bf0-499d-9b80-8c7c0a9510d0"), "1,6mm", "10g", "20mm", null, "172g" },
                    { new Guid("ca1871c6-d19d-4712-b04a-92e3b9db7fe6"), null, "9", "9000-11000g", "23,8mm", "30g", null, new Guid("7a964316-12ed-4b83-bd75-924e93528f5e"), 1000000.00m, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000005"), true, null, new DateTime(2025, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("55555555-5555-5555-5555-555555555555"), new Guid("66c9552e-8714-4e99-8452-152632333abc"), 1400000.00m, new Guid("45109257-a076-425e-98ae-3c29b232ea13"), null, null, "6", 15, 6, new Guid("94ce350e-4bf0-499d-9b80-8c7c0a9510d0"), "1,7mm", "18g", "21,8mm", null, "121cm" },
                    { new Guid("d0025a02-fbd3-41b4-b444-d84c9e902e08"), null, null, "5000-6000g", "26mm", null, "121cm", new Guid("a0c2ae82-56fb-4af1-8a42-2678ad491853"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000006"), true, null, new DateTime(2025, 12, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("55555555-5555-5555-5555-555555555555"), new Guid("66c9552e-8714-4e99-8452-152632333abc"), 1800000.00m, new Guid("d4726e64-22ab-4936-8338-3b4b5a54e4dd"), null, null, "6", 0, 45, new Guid("99999999-9999-9999-9999-999999999999"), "1,7mm", "15g", "15,3mm", null, "172g" },
                    { new Guid("d7dc2fc2-cf47-453a-ba5c-4f21e33fe112"), null, "9", "5000-6000g", "26mm", "18g", "121cm", new Guid("a0c2ae82-56fb-4af1-8a42-2678ad491853"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000006"), true, null, new DateTime(2025, 12, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("66666666-6666-6666-6666-666666666666"), new Guid("66c9552e-8714-4e99-8452-152632333abc"), 2300000.00m, new Guid("2c7e99bd-94cb-415f-806d-5ac175805818"), null, null, "7", 0, 9, new Guid("99999999-9999-9999-9999-999999999999"), "1,6mm", "10g", "20mm", null, "172g" },
                    { new Guid("e44202d7-7a1b-49ce-9573-aff799783500"), null, null, "7800g", "31mm", "34g", "118cm", new Guid("66666666-6666-6666-6666-000000000001"), null, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000006"), true, null, new DateTime(2025, 12, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("88888888-8888-8888-8888-888888888888"), new Guid("66c9552e-8714-4e99-8452-152632333abc"), 11160000.00m, new Guid("62342343-5e30-41f2-8e73-2c13f72fedc7"), null, null, "9", 0, 3, new Guid("99999999-9999-9999-9999-999999999999"), "1,6mm", "18g", "26mm", null, "534g" },
                    { new Guid("e4dc208e-a1ba-4165-98f6-08d28324100a"), null, "7", "4500", "23,8mm", "26g", "119g", new Guid("79927f97-f7f2-4ecd-bed3-d89a5f9ed414"), 8000000.00m, new Guid("88888888-8888-8888-8888-000000000001"), null, new Guid("00000000-0000-0000-0000-000000000005"), true, null, new DateTime(2025, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("44444444-4444-4444-4444-444444444444"), new Guid("66c9552e-8714-4e99-8452-152632333abc"), 11560000.00m, new Guid("3d4087a9-8e60-4adc-9f3a-d3c3e037923b"), null, null, "5", 0, 25, new Guid("99999999-9999-9999-9999-999999999999"), "1,3mm", "16g", "18,7mm", null, "141g" }
                });

            migrationBuilder.InsertData(
                table: "ProductImage",
                columns: new[] { "Id", "ImageUrl", "ProductDetailId" },
                values: new object[,]
                {
                    { new Guid("0c9fdeab-fd4f-4fb3-a10e-859d695fcc69"), "/uploads/product-details/46a67c25-d91e-4cf6-8c24-3434dbd06a34/1ec7e57d-1fc3-4432-81f9-3255f77d01ef.jpg", new Guid("46a67c25-d91e-4cf6-8c24-3434dbd06a34") },
                    { new Guid("166f611f-5a3d-4578-b298-313beb7527f1"), "/uploads/product-details/487f82fc-6678-4bf1-a3d8-25d2cada8633/c93be9d3-62a4-436a-8fe0-e815dbbba3a6.jpg", new Guid("487f82fc-6678-4bf1-a3d8-25d2cada8633") },
                    { new Guid("1a82b9c8-2e0d-4237-8b4e-faf7c79c4d32"), "/uploads/product-details/d7dc2fc2-cf47-453a-ba5c-4f21e33fe112/b676c131-4f11-49c9-8826-b459b1142988.webp", new Guid("d7dc2fc2-cf47-453a-ba5c-4f21e33fe112") },
                    { new Guid("30449a6a-c59b-4237-a623-fcb1497ff2ac"), "/uploads/product-details/55f5a1e5-4168-4170-a1a0-cff392249ca2/c079ad99-4043-4663-ac78-067135f474ee.webp", new Guid("55f5a1e5-4168-4170-a1a0-cff392249ca2") },
                    { new Guid("31cc06d8-ab17-4a91-a195-7106cc0ece06"), "/uploads/product-details/e44202d7-7a1b-49ce-9573-aff799783500/553fcf43-717e-43e4-9f62-fc44d97eb251.webp", new Guid("e44202d7-7a1b-49ce-9573-aff799783500") },
                    { new Guid("3c83d4b6-89be-415b-9b9d-eb394059ad9b"), "/uploads/product-details/72ddf35a-f7c9-420e-9dd2-650c6082779d/0cba5d21-f486-4eac-bb3f-150a76d9e0e6.webp", new Guid("72ddf35a-f7c9-420e-9dd2-650c6082779d") },
                    { new Guid("3d28d9ba-8ca9-4d90-a56a-51b839722410"), "/uploads/product-details/d0025a02-fbd3-41b4-b444-d84c9e902e08/5c6068c7-21f1-47f1-a103-4c44d790b4b0.webp", new Guid("d0025a02-fbd3-41b4-b444-d84c9e902e08") },
                    { new Guid("42c1bd70-9159-40d2-bd33-543658b8c17b"), "/uploads/product-details/268d8602-8ffb-4f08-af04-309d9ee7a652/ddc35887-da66-4d19-b9f8-f4eb8dcb7685.webp", new Guid("268d8602-8ffb-4f08-af04-309d9ee7a652") },
                    { new Guid("4ce58328-7b9f-4f99-87ab-756b7e52438c"), "/uploads/product-details/81c268b2-a541-48b0-8e98-73b92512b1ad/f0edd22c-3a16-4e43-976b-dbd748d077d3.webp", new Guid("81c268b2-a541-48b0-8e98-73b92512b1ad") },
                    { new Guid("5a6da4f4-f107-46a9-845d-695256ed958d"), "/uploads/product-details/49219172-62e3-4e6d-bbb8-57b7466a59f4/c23bcf82-d3b1-4fbb-b202-76a7267e39ae.jpg", new Guid("49219172-62e3-4e6d-bbb8-57b7466a59f4") },
                    { new Guid("6a5754b8-b821-4855-a931-80ec039128d3"), "/uploads/product-details/1d5b0c18-60f3-46a6-9df8-003211c32983/b3ca37a6-b3ef-47ae-9a09-1751336ba19f.webp", new Guid("1d5b0c18-60f3-46a6-9df8-003211c32983") },
                    { new Guid("704331b4-e712-47b6-a71e-6823e41f85f7"), "/uploads/product-details/8f1f78bd-aac3-4ffe-9b67-a5b0546291f9/98d8ec04-9aad-4411-aec3-6fdf49bd1a0d.webp", new Guid("8f1f78bd-aac3-4ffe-9b67-a5b0546291f9") },
                    { new Guid("75ffaa89-9ed5-4d72-ba97-7a94b5bb1815"), "/uploads/product-details/3dbe0697-6b17-4854-a251-05db0b49a684/bdceaaf5-b097-4b94-b961-e97c044309b2.webp", new Guid("3dbe0697-6b17-4854-a251-05db0b49a684") },
                    { new Guid("7d968c7e-afda-436d-9fdf-1dca5be0bd1b"), "/uploads/product-details/606d0e6d-5f97-44c8-9223-2466d0f70b04/76014e04-8fde-46af-9e9c-0b5375c2484f.webp", new Guid("606d0e6d-5f97-44c8-9223-2466d0f70b04") },
                    { new Guid("862127da-a1d3-4ace-9460-480568c7770f"), "/uploads/product-details/014ffd65-5d70-4534-a11c-245361dafbe8/44685adc-9179-40fb-a88e-f2cd8ca10d2a.webp", new Guid("014ffd65-5d70-4534-a11c-245361dafbe8") },
                    { new Guid("86544e67-5f25-4ac7-a009-1aea255cd595"), "/uploads/product-details/ca1871c6-d19d-4712-b04a-92e3b9db7fe6/e27ac9e8-d238-4f9b-af60-d5f3abaf5e00.webp", new Guid("ca1871c6-d19d-4712-b04a-92e3b9db7fe6") },
                    { new Guid("9515e595-e10f-4bca-b70d-3d0506a70961"), "/uploads/product-details/26d96697-c8ca-4cb9-8314-be1d32386d74/69cb8ed7-32d2-4be0-b5cd-7a1395ee77ba.webp", new Guid("26d96697-c8ca-4cb9-8314-be1d32386d74") },
                    { new Guid("98a6070a-3b81-4c64-b004-371a6de1e151"), "/uploads/product-details/3dd3a835-efb9-4656-8128-1bccf2020e95/140f050f-160d-4cfb-9f0b-00c6c55c73d9.webp", new Guid("3dd3a835-efb9-4656-8128-1bccf2020e95") },
                    { new Guid("999c981c-6823-42cd-aa2e-a63a0a89f3f9"), "/uploads/product-details/b111948c-c3e3-41ce-93c5-831b90897fd7/4f08440a-18ab-46f1-b850-8f302908ebf6.webp", new Guid("b111948c-c3e3-41ce-93c5-831b90897fd7") },
                    { new Guid("a1ce0b3f-5fb4-4585-b064-838edc96a794"), "/uploads/product-details/0677c96a-7474-4936-b8d6-24bd1c1ac08b/4a39c052-8133-4641-87df-dbcbcc3f5ff5.webp", new Guid("0677c96a-7474-4936-b8d6-24bd1c1ac08b") },
                    { new Guid("a4ccfc2a-632d-4bc7-935c-1cb2a76da18b"), "/uploads/product-details/333c99b1-b645-4b45-8588-edd50e706c1e/a8f3f183-c89b-42e1-b22a-289734ab4a3a.jpg", new Guid("333c99b1-b645-4b45-8588-edd50e706c1e") },
                    { new Guid("af8334b8-ce61-4abc-bc7b-2ae6de336e54"), "/uploads/product-details/5d1e1667-89f9-4f08-83cc-04d732f78f4d/69b7f261-36a2-4a15-ba4c-cc6707701fd3.webp", new Guid("5d1e1667-89f9-4f08-83cc-04d732f78f4d") },
                    { new Guid("b813ee6d-0a91-474a-98f2-92c1d4fb0e51"), "/uploads/product-details/6e3d5c21-c4c2-4a99-896d-a7cf79088041/9d85c5ea-4219-49ef-962e-2dd2f9816d25.webp", new Guid("6e3d5c21-c4c2-4a99-896d-a7cf79088041") },
                    { new Guid("ba4f34ac-5578-4788-a2b4-9446b816bc6e"), "/uploads/product-details/754ca79f-7c36-45f3-8c82-23e2978f1b6a/04ba3bb2-7530-4a2d-9d58-20129a5a07c9.jpg", new Guid("754ca79f-7c36-45f3-8c82-23e2978f1b6a") },
                    { new Guid("c1a149bb-f65a-4749-bf29-bd7776a20a6f"), "/uploads/product-details/06efd08e-338d-41aa-b6cb-ad5d63a8f412/6b0b0329-259c-4524-83cf-6e292315019c.webp", new Guid("06efd08e-338d-41aa-b6cb-ad5d63a8f412") },
                    { new Guid("c1cfc4d0-91f5-4b26-b00a-02afe7bc35ab"), "/uploads/product-details/049e2071-4402-47e0-b223-a0b224005ce6/92b2b03f-b1cc-4072-92c0-9ef39daa4848.jpg", new Guid("049e2071-4402-47e0-b223-a0b224005ce6") },
                    { new Guid("dc4a2d34-47b2-4c0a-919d-4690465841a8"), "/uploads/product-details/2b77eed7-80c0-42e3-9bc5-94677c01dc99/b2f3c83c-9cae-41d5-ad6b-019eb38379a0.webp", new Guid("2b77eed7-80c0-42e3-9bc5-94677c01dc99") }
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
