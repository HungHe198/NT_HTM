# 🛍️ NT_HTM - Hệ Thống Quản Lý Bán Hàng

<p align="center">
  <img src="https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt=".NET 8"/>
  <img src="https://img.shields.io/badge/Entity%20Framework%20Core-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt="EF Core"/>
  <img src="https://img.shields.io/badge/SQL%20Server-Database-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white" alt="SQL Server"/>
  <img src="https://img.shields.io/badge/ASP.NET%20Core-MVC-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt="ASP.NET Core MVC"/>
</p>

## 📋 Mục Lục

- [Giới Thiệu](#-giới-thiệu)
- [Tính Năng](#-tính-năng)
- [Kiến Trúc Hệ Thống](#-kiến-trúc-hệ-thống)
- [Cấu Trúc Dự Án](#-cấu-trúc-dự-án)
- [Công Nghệ Sử Dụng](#-công-nghệ-sử-dụng)
- [Yêu Cầu Hệ Thống](#-yêu-cầu-hệ-thống)
- [Hướng Dẫn Cài Đặt](#-hướng-dẫn-cài-đặt)
- [Cấu Hình](#-cấu-hình)
- [Chạy Ứng Dụng](#-chạy-ứng-dụng)
- [Cơ Sở Dữ Liệu](#-cơ-sở-dữ-liệu)
- [API Documentation](#-api-documentation)
- [Đóng Góp](#-đóng-góp)
- [Liên Hệ](#-liên-hệ)

---

## 🎯 Giới Thiệu

**NT_HTM** là một hệ thống quản lý bán hàng trực tuyến được xây dựng trên nền tảng **.NET 8**, áp dụng kiến trúc **N-Tier Architecture** (kiến trúc đa tầng) giúp phân tách rõ ràng các tầng xử lý trong hệ thống.

Hệ thống được thiết kế để hỗ trợ các doanh nghiệp quản lý sản phẩm, đơn hàng, khách hàng, nhân viên và các nghiệp vụ bán hàng một cách hiệu quả.

---

## ✨ Tính Năng

### 👤 Quản Lý Người Dùng
- Hệ thống phân quyền với 3 vai trò: **Admin**, **Employee**, **Customer**
- Đăng ký, đăng nhập, đăng xuất với Cookie Authentication
- Quản lý thông tin tài khoản, đổi mật khẩu
- Phân quyền chi tiết theo Role và Permission

### 🛒 Quản Lý Sản Phẩm
- CRUD sản phẩm với thông tin chi tiết
- Quản lý thương hiệu (Brand)
- Quản lý danh mục sản phẩm (Category)
- Hỗ trợ nhiều thuộc tính sản phẩm:
  - Màu sắc (Color)
  - Độ dài (Length)
  - Độ cứng (Hardness)
  - Độ đàn hồi (Elasticity)
  - Hoàn thiện bề mặt (Surface Finish)
  - Xuất xứ (Origin Country)
- Quản lý hình ảnh sản phẩm (hỗ trợ upload file lên đến 2GB)
- Chi tiết sản phẩm (Product Detail) với các biến thể

### 🛍️ Quản Lý Đơn Hàng & Bán Hàng
- Giỏ hàng (Cart) với chi tiết giỏ hàng (Cart Detail)
- Quy trình checkout đơn hàng
- Quản lý đơn hàng với các trạng thái
- Hỗ trợ mã giảm giá (Voucher)
- Nhiều phương thức thanh toán (Payment Method)
- In hóa đơn bán hàng
- Lịch sử mua hàng

### 👥 Quản Lý Khách Hàng & Nhân Viên
- Quản lý thông tin khách hàng
- Quản lý nhân viên
- Quản lý admin

### 🔐 Bảo Mật
- Cookie Authentication với thời gian hết hạn 30 phút
- Sliding Expiration hỗ trợ gia hạn session
- Mã hóa mật khẩu với BCrypt
- Xác thực số điện thoại Việt Nam

### 🌐 Đa Ngôn Ngữ & Địa Phương Hóa
- Hỗ trợ tiếng Việt (vi-VN) và tiếng Anh (en-US)
- UTF-8 encoding toàn diện cho tiếng Việt

---

## 🏗 Kiến Trúc Hệ Thống

Dự án sử dụng **N-Tier Architecture** (Kiến trúc đa tầng) với các layer rõ ràng:

```
┌─────────────────────────────────────────────────────────────┐
│                    Presentation Layer                        │
│                  ┌─────────┐  ┌─────────┐                   │
│                  │ NT.WEB  │  │ NT.API  │                   │
│                  │  (MVC)  │  │  (API)  │                   │
│                  └────┬────┘  └────┬────┘                   │
├───────────────────────┼────────────┼────────────────────────┤
│                    Business Logic Layer                      │
│                       ┌─────────┐                           │
│                       │ NT.BLL  │                           │
│                       │Services │                           │
│                       └────┬────┘                           │
├────────────────────────────┼────────────────────────────────┤
│                    Data Access Layer                         │
│              ┌─────────────┴─────────────┐                  │
│              │         NT.DAL            │                  │
│              │  Repository + DbContext   │                  │
│              └─────────────┬─────────────┘                  │
├────────────────────────────┼────────────────────────────────┤
│                    Shared/Common Layer                       │
│       ┌────────────────────┴────────────────────┐           │
│       │              NT.SHARED                  │           │
│       │    Models, DTOs, Configurations         │           │
│       └────────────────────┬────────────────────┘           │
│       ┌────────────────────┴────────────────────┐           │
│       │          NT.Infrastructure              │           │
│       │      Logging, Error Handling            │           │
│       └─────────────────────────────────────────┘           │
└─────────────────────────────────────────────────────────────┘
```

---

## 📁 Cấu Trúc Dự Án

```
NT_HTM/
├── 📂 NT.SHARED/                    # Shared Layer - Models & Configurations
│   ├── 📂 Models/                   # Domain Models (Entities)
│   │   ├── User.cs
│   │   ├── Admin.cs
│   │   ├── Employee.cs
│   │   ├── Customer.cs
│   │   ├── Product.cs
│   │   ├── ProductDetail.cs
│   │   ├── ProductImage.cs
│   │   ├── Category.cs
│   │   ├── Brand.cs
│   │   ├── Cart.cs
│   │   ├── CartDetail.cs
│   │   ├── Order.cs
│   │   ├── OrderDetail.cs
│   │   ├── Voucher.cs
│   │   ├── PaymentMethod.cs
│   │   ├── Role.cs
│   │   ├── Permission.cs
│   │   ├── RolePermission.cs
│   │   ├── Color.cs
│   │   ├── Length.cs
│   │   ├── Hardness.cs
│   │   ├── Elasticity.cs
│   │   ├── SurfaceFinish.cs
│   │   └── OriginCountry.cs
│   ├── 📂 Configurations/           # Entity Framework Configurations
│   │   └── *Configuration.cs
│   ├── 📂 DTOs/                     # Data Transfer Objects
│   │   ├── LoginDTO.cs
│   │   ├── RegisterDto.cs
│   │   └── ForgotPasswordDto.cs
│   ├── 📂 ErrorHandling/            # Error Handling Interfaces
│   └── 📂 Logging/                  # Logging Interfaces
│
├── 📂 NT.Infrastructure/            # Infrastructure Layer
│   ├── 📂 ErrorHandling/
│   │   └── BugReportService.cs
│   └── 📂 Logging/
│       └── LoggerService.cs
│
├── 📂 NT.DAL/                       # Data Access Layer
│   ├── 📂 ContextFile/
│   │   └── NTAppDbContext.cs        # Entity Framework DbContext
│   ├── 📂 Repositories/
│   │   └── GenericRepository.cs     # Generic Repository Pattern
│   └── 📂 Services/
│       └── DataSeeder.cs            # Database Seeding
│
├── 📂 NT.BLL/                       # Business Logic Layer
│   ├── 📂 Interfaces/               # Service Interfaces
│   │   ├── IGenericService.cs
│   │   ├── IGenericRepository.cs
│   │   ├── IProductService.cs
│   │   ├── IOrderService.cs
│   │   ├── IUserService.cs
│   │   └── ...
│   └── 📂 Services/                 # Service Implementations
│       ├── GenericService.cs
│       ├── ProductService.cs
│       ├── OrderService.cs
│       ├── UserService.cs
│       ├── CartService.cs
│       ├── CustomerService.cs
│       └── ...
│
├── 📂 NT.API/                       # API Layer (REST API)
│   ├── 📂 Controllers/
│   │   └── WeatherForecastController.cs
│   ├── 📂 Middleware/
│   │   └── ExceptionHandlingMiddleware.cs
│   └── Program.cs
│
├── 📂 NT.WEB/                       # Web Application (MVC)
│   ├── 📂 Controllers/              # MVC Controllers
│   │   ├── HomeController.cs
│   │   ├── ProductController.cs
│   │   ├── CartController.cs
│   │   ├── OrdersController.cs
│   │   ├── CheckoutController.cs
│   │   ├── AccountController.cs
│   │   ├── LoginController.cs
│   │   ├── RegisterController.cs
│   │   ├── AdminController.cs
│   │   ├── CustomerController.cs
│   │   ├── EmployeeController.cs
│   │   ├── SalesController.cs
│   │   └── ...
│   ├── 📂 Views/                    # Razor Views
│   │   ├── 📂 Shared/
│   │   │   ├── _AdminLayout.cshtml
│   │   │   ├── _ClientLayout.cshtml
│   │   │   └── Error.cshtml
│   │   ├── 📂 Home/
│   │   ├── 📂 Product/
│   │   ├── 📂 Cart/
│   │   ├── 📂 Orders/
│   │   └── ...
│   ├── 📂 ViewComponents/           # View Components
│   │   ├── CartSummaryViewComponent.cs
│   │   ├── FeaturedProductsViewComponent.cs
│   │   └── OrdersPendingCountViewComponent.cs
│   ├── 📂 Services/                 # Web Services
│   │   ├── ProductWebService.cs
│   │   ├── CartWebService.cs
│   │   ├── OrdersWebService.cs
│   │   ├── SmtpEmailService.cs
│   │   └── ...
│   ├── 📂 DTO/
│   │   └── CartItemDto.cs
│   ├── 📂 Models/
│   │   └── ErrorViewModel.cs
│   ├── 📂 Pages/                    # Razor Pages
│   └── Program.cs
│
└── 📄 NT_HTM.sln                    # Solution File
```

---

## 🛠 Công Nghệ Sử Dụng

### Backend
| Công nghệ | Phiên bản | Mô tả |
|-----------|-----------|-------|
| .NET | 8.0 | Framework chính |
| ASP.NET Core MVC | 8.0 | Web framework |
| Entity Framework Core | 8.0.20 | ORM |
| SQL Server | - | Database |

### Authentication & Security
| Công nghệ | Phiên bản | Mô tả |
|-----------|-----------|-------|
| Cookie Authentication | Built-in | Xác thực người dùng |
| BCrypt.Net-Next | 4.0.2 | Mã hóa mật khẩu |
| JWT Bearer | 8.0.20 | API Authentication |

### Libraries & Tools
| Công nghệ | Phiên bản | Mô tả |
|-----------|-----------|-------|
| AutoMapper | 12.0.1 | Object mapping |
| FluentValidation | 11.9.1 | Validation |
| Serilog | 4.3.0 | Logging framework |
| Swashbuckle (Swagger) | 10.0.1 | API Documentation |

---

## 💻 Yêu Cầu Hệ Thống

### Phần Mềm
- **.NET SDK 8.0** hoặc cao hơn
- **SQL Server 2019** hoặc cao hơn (hoặc SQL Server Express)
- **Visual Studio 2022** (khuyến nghị) hoặc VS Code
- **Git**

### Phần Cứng (Khuyến nghị)
- RAM: 8GB trở lên
- Dung lượng đĩa: 10GB trống
- CPU: 4 cores trở lên

---

## 🚀 Hướng Dẫn Cài Đặt

### 1. Clone Repository

```bash
git clone https://github.com/HungHe198/NT_HTM.git
cd NT_HTM
```

### 2. Restore Packages

```bash
dotnet restore
```

### 3. Cấu Hình Database

Cập nhật connection string trong file `NT.DAL\ContextFile\NTAppDbContext.cs`:

```csharp
optionsBuilder.UseSqlServer("Data Source=YOUR_SERVER;Initial Catalog=NT_HTM;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");
```

Hoặc cấu hình trong `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=YOUR_SERVER;Initial Catalog=NT_HTM;Integrated Security=True;Encrypt=True;Trust Server Certificate=True"
  }
}
```

### 4. Tạo Database

```bash
# Di chuyển đến thư mục project NT.DAL
cd NT.DAL

# Tạo migration (nếu chưa có)
dotnet ef migrations add InitialCreate --startup-project ../NT.WEB

# Cập nhật database
dotnet ef database update --startup-project ../NT.WEB
```

### 5. Build Solution

```bash
dotnet build
```

---

## ⚙ Cấu Hình

### appsettings.json (NT.WEB)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Your_Connection_String_Here"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "SmtpSettings": {
    "Server": "smtp.gmail.com",
    "Port": 587,
    "Username": "your-email@gmail.com",
    "Password": "your-app-password"
  }
}
```

### Cấu Hình Session & Authentication

Ứng dụng đã được cấu hình sẵn với:
- **Session timeout**: 30 phút
- **Cookie expiration**: 30 phút
- **Sliding expiration**: Có (tự động gia hạn khi có hoạt động)

---

## ▶ Chạy Ứng Dụng

### Sử dụng .NET CLI

```bash
# Chạy Web Application
cd NT.WEB
dotnet run

# Hoặc chạy API
cd NT.API
dotnet run
```

### Sử dụng Visual Studio

1. Mở file `NT_HTM.sln` bằng Visual Studio 2022
2. Chọn startup project (NT.WEB hoặc NT.API)
3. Nhấn `F5` hoặc click **Start**

### Truy Cập Ứng Dụng

- **Web Application**: `https://localhost:5001` hoặc `http://localhost:5000`
- **API (Swagger)**: `https://localhost:5001/swagger`

---

## 🗄 Cơ Sở Dữ Liệu

### Entity Relationship Diagram (ERD)

```
┌─────────────────┐       ┌─────────────────┐       ┌─────────────────┐
│      Role       │       │      User       │       │     Admin       │
├─────────────────┤       ├─────────────────┤       ├─────────────────┤
│ Id (PK)         │◄──────│ RoleId (FK)     │───────►│ UserId (PK,FK)  │
│ Name            │       │ Id (PK)         │       │ ...             │
└─────────────────┘       │ Username        │       └─────────────────┘
        │                 │ PasswordHash    │
        │                 │ Fullname        │       ┌─────────────────┐
        ▼                 │ ...             │       │    Employee     │
┌─────────────────┐       └─────────────────┘       ├─────────────────┤
│ RolePermission  │               │                 │ UserId (PK,FK)  │
├─────────────────┤               │                 │ ...             │
│ RoleId (FK)     │               │                 └─────────────────┘
│ PermissionId(FK)│               │
└─────────────────┘               ▼                 ┌─────────────────┐
        │                 ┌─────────────────┐       │    Customer     │
        ▼                 │    Customer     │───────│ UserId (PK,FK)  │
┌─────────────────┐       └─────────────────┘       │ ...             │
│   Permission    │               │                 └─────────────────┘
├─────────────────┤               │
│ Id (PK)         │               ▼
│ Name            │       ┌─────────────────┐       ┌─────────────────┐
└─────────────────┘       │      Order      │◄──────│   OrderDetail   │
                          ├─────────────────┤       ├─────────────────┤
┌─────────────────┐       │ Id (PK)         │       │ Id (PK)         │
│     Product     │       │ CustomerId (FK) │       │ OrderId (FK)    │
├─────────────────┤       │ VoucherId (FK)  │       │ ProductDetailId │
│ Id (PK)         │       │ PaymentMethodId │       │ Quantity        │
│ BrandId (FK)    │       │ TotalAmount     │       │ UnitPrice       │
│ ProductCode     │       │ FinalAmount     │       └─────────────────┘
│ Name            │       │ Status          │
│ ...             │       └─────────────────┘
└─────────────────┘
        │
        ▼
┌─────────────────┐       ┌─────────────────┐
│ ProductCategory │       │  ProductDetail  │
├─────────────────┤       ├─────────────────┤
│ ProductId (FK)  │       │ Id (PK)         │
│ CategoryId (FK) │       │ ProductId (FK)  │
└─────────────────┘       │ ColorId (FK)    │
                          │ LengthId (FK)   │
                          │ Price           │
                          │ Stock           │
                          └─────────────────┘
```

### Các Bảng Chính

| Bảng | Mô Tả |
|------|-------|
| `Users` | Thông tin người dùng cơ bản |
| `Admins` | Thông tin admin |
| `Employees` | Thông tin nhân viên |
| `Customers` | Thông tin khách hàng |
| `Roles` | Vai trò người dùng |
| `Permissions` | Quyền hạn |
| `RolePermissions` | Phân quyền theo vai trò |
| `Products` | Sản phẩm |
| `ProductDetails` | Chi tiết/biến thể sản phẩm |
| `ProductImages` | Hình ảnh sản phẩm |
| `Categories` | Danh mục sản phẩm |
| `ProductCategories` | Liên kết sản phẩm-danh mục |
| `Brands` | Thương hiệu |
| `Colors` | Màu sắc |
| `Lengths` | Độ dài |
| `Hardnesses` | Độ cứng |
| `Elasticities` | Độ đàn hồi |
| `SurfaceFinishes` | Hoàn thiện bề mặt |
| `OriginCountries` | Xuất xứ |
| `Carts` | Giỏ hàng |
| `CartDetails` | Chi tiết giỏ hàng |
| `Orders` | Đơn hàng |
| `OrderDetails` | Chi tiết đơn hàng |
| `Vouchers` | Mã giảm giá |
| `PaymentMethods` | Phương thức thanh toán |

---

## 📚 API Documentation

API được document bằng **Swagger/OpenAPI**. Sau khi chạy `NT.API`, truy cập:

```
https://localhost:PORT/swagger
```

### Các Endpoint Chính

```
GET     /api/products           # Lấy danh sách sản phẩm
GET     /api/products/{id}      # Lấy chi tiết sản phẩm
POST    /api/products           # Tạo sản phẩm mới
PUT     /api/products/{id}      # Cập nhật sản phẩm
DELETE  /api/products/{id}      # Xóa sản phẩm

GET     /api/orders             # Lấy danh sách đơn hàng
POST    /api/orders             # Tạo đơn hàng mới
...
```

---

## 🤝 Đóng Góp

Chúng tôi hoan nghênh mọi đóng góp! Để đóng góp:

1. **Fork** repository
2. Tạo **feature branch** (`git checkout -b feature/AmazingFeature`)
3. **Commit** changes (`git commit -m 'Add some AmazingFeature'`)
4. **Push** to branch (`git push origin feature/AmazingFeature`)
5. Tạo **Pull Request**

### Quy Tắc Code

- Tuân thủ coding conventions của C#/.NET
- Viết unit tests cho các tính năng mới
- Comment code khi cần thiết (tiếng Việt hoặc tiếng Anh)
- Đảm bảo code build thành công trước khi tạo PR

---

## 👨‍💻 Tác Giả

- **HungHe198** - *Developer* - [GitHub](https://github.com/HungHe198)

---

## 📄 License

Dự án này được phân phối dưới giấy phép MIT. Xem file `LICENSE` để biết thêm chi tiết.

---

## 📞 Liên Hệ

- **Repository**: [https://github.com/HungHe198/NT_HTM](https://github.com/HungHe198/NT_HTM)
- **Issues**: [https://github.com/HungHe198/NT_HTM/issues](https://github.com/HungHe198/NT_HTM/issues)

---

<p align="center">
  ⭐ Nếu dự án hữu ích, hãy cho chúng tôi một star! ⭐
</p>
