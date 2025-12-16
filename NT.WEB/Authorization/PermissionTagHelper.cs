using Microsoft.AspNetCore.Razor.TagHelpers;
using NT.BLL.Interfaces;
using NT.SHARED.Models;
using System.Security.Claims;

namespace NT.WEB.Authorization
{
    /// <summary>
    /// TagHelper để ẩn/hiện element dựa trên quyền.
    /// Sử dụng: &lt;div permission-resource="Product" permission-action="Create"&gt;...&lt;/div&gt;
    /// Element sẽ bị ẩn nếu user không có quyền tương ứng.
    /// </summary>
    [HtmlTargetElement(Attributes = "permission-resource,permission-action")]
    public class PermissionTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IServiceProvider _serviceProvider;

        [HtmlAttributeName("permission-resource")]
        public string Resource { get; set; } = string.Empty;

        [HtmlAttributeName("permission-action")]
        public string Action { get; set; } = string.Empty;

        public PermissionTagHelper(
            IHttpContextAccessor httpContextAccessor,
            IServiceProvider serviceProvider)
        {
            _httpContextAccessor = httpContextAccessor;
            _serviceProvider = serviceProvider;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (string.IsNullOrWhiteSpace(Resource) || string.IsNullOrWhiteSpace(Action))
            {
                output.SuppressOutput();
                return;
            }

            var hasPermission = await CheckPermissionAsync();

            if (!hasPermission)
            {
                output.SuppressOutput();
            }
        }

        private async Task<bool> CheckPermissionAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null) return false;

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

            // Tạo scope để lấy scoped services
            using var scope = _serviceProvider.CreateScope();
            var roleRepo = scope.ServiceProvider.GetRequiredService<IGenericRepository<Role>>();
            var permissionRepo = scope.ServiceProvider.GetRequiredService<IGenericRepository<Permission>>();
            var rolePermissionRepo = scope.ServiceProvider.GetRequiredService<IGenericRepository<RolePermission>>();

            // Tìm role
            var roles = await roleRepo.FindAsync(r => r.Name == roleName);
            var role = roles.FirstOrDefault();
            if (role == null)
                return false;

            // Tìm permission
            var permissions = await permissionRepo.FindAsync(p =>
                p.Resource == Resource && p.Action == Action);
            var permission = permissions.FirstOrDefault();
            if (permission == null)
                return false;

            // Kiểm tra role có permission không
            var rolePermissions = await rolePermissionRepo.FindAsync(rp =>
                rp.RoleId == role.Id && rp.PermissionId == permission.Id);

            return rolePermissions.Any();
        }
    }
}
