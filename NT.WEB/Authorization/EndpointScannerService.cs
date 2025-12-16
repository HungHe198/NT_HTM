using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Reflection;

namespace NT.WEB.Authorization
{
    /// <summary>
    /// Service quét tất cả Controller và Action trong ứng dụng để tạo danh sách Endpoint.
    /// Dùng để tự động đăng ký Permission vào database.
    /// </summary>
    public class EndpointScannerService
    {
        private readonly IActionDescriptorCollectionProvider _actionDescriptorProvider;

        public EndpointScannerService(IActionDescriptorCollectionProvider actionDescriptorProvider)
        {
            _actionDescriptorProvider = actionDescriptorProvider;
        }

        /// <summary>
        /// Quét tất cả các endpoint trong ứng dụng
        /// </summary>
        public IEnumerable<EndpointInfo> ScanAllEndpoints()
        {
            var endpoints = new List<EndpointInfo>();

            var actionDescriptors = _actionDescriptorProvider.ActionDescriptors.Items
                .OfType<ControllerActionDescriptor>();

            foreach (var descriptor in actionDescriptors)
            {
                var controllerName = descriptor.ControllerName;
                var actionName = descriptor.ActionName;

                // Bỏ qua các controller hệ thống
                if (IsSystemController(controllerName))
                    continue;

                // Xác định HTTP method
                var httpMethod = GetHttpMethod(descriptor);

                // Xác định có phải admin area không (dựa vào naming convention hoặc attribute)
                var isAdminArea = IsAdminController(controllerName);

                // Tạo mô tả action
                var description = GenerateDescription(controllerName, actionName, httpMethod);

                var endpoint = new EndpointInfo
                {
                    Controller = controllerName,
                    Action = actionName,
                    HttpMethod = httpMethod,
                    Route = $"/{controllerName}/{actionName}",
                    Description = description,
                    IsAdminArea = isAdminArea
                };

                // Tránh trùng lặp
                if (!endpoints.Any(e => e.PermissionCode == endpoint.PermissionCode))
                {
                    endpoints.Add(endpoint);
                }
            }

            return endpoints.OrderBy(e => e.Controller).ThenBy(e => e.Action);
        }

        /// <summary>
        /// Quét các endpoint thuộc admin area
        /// </summary>
        public IEnumerable<EndpointInfo> ScanAdminEndpoints()
        {
            return ScanAllEndpoints().Where(e => e.IsAdminArea);
        }

        /// <summary>
        /// Lấy danh sách các Resource (Controller) duy nhất
        /// </summary>
        public IEnumerable<string> GetUniqueResources()
        {
            return ScanAllEndpoints()
                .Select(e => e.Controller)
                .Distinct()
                .OrderBy(r => r);
        }

        /// <summary>
        /// Lấy danh sách các Action duy nhất của một Resource
        /// </summary>
        public IEnumerable<string> GetActionsForResource(string resource)
        {
            return ScanAllEndpoints()
                .Where(e => string.Equals(e.Controller, resource, StringComparison.OrdinalIgnoreCase))
                .Select(e => e.Action)
                .Distinct()
                .OrderBy(a => a);
        }

        private bool IsSystemController(string controllerName)
        {
            var systemControllers = new[]
            {
                "Home", // Có thể giữ nếu muốn phân quyền trang chủ
            };

            // Không filter bất kỳ controller nào để có thể phân quyền đầy đủ
            return false;
        }

        private bool IsAdminController(string controllerName)
        {
            // Các controller thuộc phần admin site
            var adminControllers = new[]
            {
                "Admin", "Employee", "Product", "ProductCategory", "ProductImage",
                "Brand", "Category", "Color", "Length", "Hardness", "Elasticity",
                "SurfaceFinish", "OriginCountry", "Voucher", "PaymentMethod",
                "PaymentMethods", "Orders", "OrderDetail", "Role", "Permission",
                "Account", "Customer", "Sales", "Cart", "CartDetail"
            };

            return adminControllers.Contains(controllerName, StringComparer.OrdinalIgnoreCase);
        }

        private string GetHttpMethod(ControllerActionDescriptor descriptor)
        {
            var methodInfo = descriptor.MethodInfo;

            if (methodInfo.GetCustomAttribute<HttpPostAttribute>() != null)
                return "POST";
            if (methodInfo.GetCustomAttribute<HttpPutAttribute>() != null)
                return "PUT";
            if (methodInfo.GetCustomAttribute<HttpDeleteAttribute>() != null)
                return "DELETE";
            if (methodInfo.GetCustomAttribute<HttpPatchAttribute>() != null)
                return "PATCH";

            return "GET";
        }

        private string GenerateDescription(string controller, string action, string httpMethod)
        {
            var actionDescriptions = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "Index", "Xem danh sách" },
                { "Details", "Xem chi tiết" },
                { "Create", "Thêm mới" },
                { "Edit", "Chỉnh sửa" },
                { "Delete", "Xóa" },
                { "DeleteConfirmed", "Xác nhận xóa" },
                { "Export", "Xuất dữ liệu" },
                { "Import", "Nhập dữ liệu" },
                { "Search", "Tìm kiếm" },
                { "Filter", "Lọc dữ liệu" },
            };

            var controllerVietnamese = GetControllerVietnameseName(controller);

            if (actionDescriptions.TryGetValue(action, out var actionDesc))
            {
                return $"{actionDesc} {controllerVietnamese}";
            }

            return $"{action} - {controllerVietnamese}";
        }

        private string GetControllerVietnameseName(string controller)
        {
            var names = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "Product", "Sản phẩm" },
                { "ProductCategory", "Danh mục sản phẩm" },
                { "ProductImage", "Hình ảnh sản phẩm" },
                { "Brand", "Thương hiệu" },
                { "Category", "Danh mục" },
                { "Color", "Màu sắc" },
                { "Length", "Chiều dài" },
                { "Hardness", "Độ cứng" },
                { "Elasticity", "Độ đàn hồi" },
                { "SurfaceFinish", "Hoàn thiện bề mặt" },
                { "OriginCountry", "Xuất xứ" },
                { "Voucher", "Mã giảm giá" },
                { "PaymentMethod", "Phương thức thanh toán" },
                { "PaymentMethods", "Phương thức thanh toán" },
                { "Orders", "Đơn hàng" },
                { "OrderDetail", "Chi tiết đơn hàng" },
                { "Customer", "Khách hàng" },
                { "Employee", "Nhân viên" },
                { "Admin", "Quản trị viên" },
                { "Role", "Vai trò" },
                { "Permission", "Quyền hạn" },
                { "Account", "Tài khoản" },
                { "Sales", "Bán hàng" },
                { "Cart", "Giỏ hàng" },
                { "CartDetail", "Chi tiết giỏ hàng" },
                { "Home", "Trang chủ" },
                { "Checkout", "Thanh toán" },
                { "Login", "Đăng nhập" },
                { "Register", "Đăng ký" },
                { "Logout", "Đăng xuất" },
            };

            return names.TryGetValue(controller, out var name) ? name : controller;
        }
    }
}
