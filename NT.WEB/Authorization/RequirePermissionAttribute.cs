using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace NT.WEB.Authorization
{
    /// <summary>
    /// Attribute để đánh dấu Controller/Action yêu cầu quyền truy cập.
    /// Sử dụng: [RequirePermission("Product", "View")] hoặc [RequirePermission("Order", "Create")]
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class RequirePermissionAttribute : Attribute, IAsyncAuthorizationFilter
    {
        /// <summary>
        /// Tên tài nguyên (Resource) - thường là tên Controller không có hậu tố "Controller"
        /// VD: Product, Order, Customer, Employee
        /// </summary>
        public string Resource { get; }

        /// <summary>
        /// Hành động (Action) trên tài nguyên
        /// VD: View, Create, Edit, Delete, Export
        /// </summary>
        public string Action { get; }

        public RequirePermissionAttribute(string resource, string action)
        {
            Resource = resource ?? throw new ArgumentNullException(nameof(resource));
            Action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            // Nếu chưa đăng nhập, chuyển đến trang login
            if (user?.Identity?.IsAuthenticated != true)
            {
                context.Result = new RedirectToActionResult("Login", "Account", new { returnUrl = context.HttpContext.Request.Path });
                return;
            }

            // Lấy role từ claims
            var roleClaim = user.FindFirst(ClaimTypes.Role);
            var roleName = roleClaim?.Value ?? string.Empty;

            // Admin luôn có full quyền
            if (string.Equals(roleName, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                return; // Cho phép truy cập
            }

            // Lấy IAuthorizationService để kiểm tra quyền
            var authService = context.HttpContext.RequestServices.GetRequiredService<IAuthorizationService>();
            var requirement = new PermissionRequirement(Resource, Action);

            var result = await authService.AuthorizeAsync(user, null, requirement);

            if (!result.Succeeded)
            {
                // Không có quyền - trả về AccessDenied
                context.Result = new RedirectToActionResult("AccessDenied", "Home", new { resource = Resource, action = Action });
            }
        }
    }
}
