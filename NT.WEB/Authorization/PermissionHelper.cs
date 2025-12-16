using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using NT.BLL.Interfaces;
using NT.SHARED.Models;
using System.Security.Claims;

namespace NT.WEB.Authorization
{
    /// <summary>
    /// Helper kiểm tra quyền trong View.
    /// Sử dụng để ẩn/hiện các menu item, button dựa trên quyền của user.
    /// </summary>
    public static class PermissionHelper
    {
        /// <summary>
        /// Kiểm tra user hiện tại có quyền truy cập Resource + Action không
        /// </summary>
        public static async Task<bool> HasPermissionAsync(
            this IHtmlHelper htmlHelper,
            string resource,
            string action)
        {
            var httpContext = htmlHelper.ViewContext.HttpContext;
            var user = httpContext.User;

            // Chưa đăng nhập
            if (user?.Identity?.IsAuthenticated != true)
                return false;

            // Lấy role từ claims
            var roleClaim = user.FindFirst(ClaimTypes.Role);
            var roleName = roleClaim?.Value ?? string.Empty;

            // Admin luôn có full quyền
            if (string.Equals(roleName, "Admin", StringComparison.OrdinalIgnoreCase))
                return true;

            // Lấy service từ DI
            var serviceProvider = httpContext.RequestServices;
            var roleRepo = serviceProvider.GetRequiredService<IGenericRepository<Role>>();
            var permissionRepo = serviceProvider.GetRequiredService<IGenericRepository<Permission>>();
            var rolePermissionRepo = serviceProvider.GetRequiredService<IGenericRepository<RolePermission>>();

            // Tìm role
            var roles = await roleRepo.FindAsync(r => r.Name == roleName);
            var role = roles.FirstOrDefault();
            if (role == null)
                return false;

            // Tìm permission
            var permissions = await permissionRepo.FindAsync(p =>
                p.Resource == resource && p.Action == action);
            var permission = permissions.FirstOrDefault();
            if (permission == null)
                return false;

            // Kiểm tra role có permission không
            var rolePermissions = await rolePermissionRepo.FindAsync(rp =>
                rp.RoleId == role.Id && rp.PermissionId == permission.Id);

            return rolePermissions.Any();
        }

        /// <summary>
        /// Kiểm tra đồng bộ (blocking) - chỉ dùng khi cần thiết
        /// </summary>
        public static bool HasPermission(
            this IHtmlHelper htmlHelper,
            string resource,
            string action)
        {
            return HasPermissionAsync(htmlHelper, resource, action).GetAwaiter().GetResult();
        }
    }

    /// <summary>
    /// Extension methods cho HttpContext để kiểm tra quyền
    /// </summary>
    public static class HttpContextPermissionExtensions
    {
        /// <summary>
        /// Kiểm tra user hiện tại có quyền không
        /// </summary>
        public static async Task<bool> UserHasPermissionAsync(
            this HttpContext httpContext,
            string resource,
            string action)
        {
            var user = httpContext.User;

            if (user?.Identity?.IsAuthenticated != true)
                return false;

            var roleClaim = user.FindFirst(ClaimTypes.Role);
            var roleName = roleClaim?.Value ?? string.Empty;

            if (string.Equals(roleName, "Admin", StringComparison.OrdinalIgnoreCase))
                return true;

            var roleRepo = httpContext.RequestServices.GetRequiredService<IGenericRepository<Role>>();
            var permissionRepo = httpContext.RequestServices.GetRequiredService<IGenericRepository<Permission>>();
            var rolePermissionRepo = httpContext.RequestServices.GetRequiredService<IGenericRepository<RolePermission>>();

            var roles = await roleRepo.FindAsync(r => r.Name == roleName);
            var role = roles.FirstOrDefault();
            if (role == null) return false;

            var permissions = await permissionRepo.FindAsync(p =>
                p.Resource == resource && p.Action == action);
            var permission = permissions.FirstOrDefault();
            if (permission == null) return false;

            var rolePermissions = await rolePermissionRepo.FindAsync(rp =>
                rp.RoleId == role.Id && rp.PermissionId == permission.Id);

            return rolePermissions.Any();
        }
    }
}
