using Microsoft.AspNetCore.Authorization;
using NT.BLL.Interfaces;
using NT.SHARED.Models;
using System.Security.Claims;

namespace NT.WEB.Authorization
{
    /// <summary>
    /// Handler xử lý authorization dựa trên Permission được gán cho Role của User.
    /// Kiểm tra User có quyền truy cập Resource + Action hay không.
    /// </summary>
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IServiceProvider _serviceProvider;

        public PermissionAuthorizationHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            // Nếu user chưa đăng nhập
            if (context.User?.Identity?.IsAuthenticated != true)
            {
                return;
            }

            // Lấy RoleName từ Claims
            var roleClaim = context.User.FindFirst(ClaimTypes.Role);
            if (roleClaim == null || string.IsNullOrWhiteSpace(roleClaim.Value))
            {
                return;
            }

            var roleName = roleClaim.Value;

            // Admin luôn có full quyền
            if (string.Equals(roleName, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                context.Succeed(requirement);
                return;
            }

            // Tạo scope để lấy services
            using var scope = _serviceProvider.CreateScope();
            var roleRepo = scope.ServiceProvider.GetRequiredService<IGenericRepository<Role>>();
            var permissionRepo = scope.ServiceProvider.GetRequiredService<IGenericRepository<Permission>>();
            var rolePermissionRepo = scope.ServiceProvider.GetRequiredService<IGenericRepository<RolePermission>>();

            // Tìm Role theo tên
            var roles = await roleRepo.FindAsync(r => r.Name == roleName);
            var role = roles.FirstOrDefault();
            if (role == null)
            {
                return;
            }

            // Lấy tất cả RolePermission của Role này
            var rolePermissions = await rolePermissionRepo.FindAsync(
                rp => rp.RoleId == role.Id,
                rp => rp.Permission!);

            // Kiểm tra có permission phù hợp không
            var hasPermission = rolePermissions.Any(rp =>
                rp.Permission != null &&
                string.Equals(rp.Permission.Resource, requirement.Resource, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(rp.Permission.Action, requirement.Action, StringComparison.OrdinalIgnoreCase));

            if (hasPermission)
            {
                context.Succeed(requirement);
            }
        }
    }
}
