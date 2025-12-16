using Microsoft.AspNetCore.Authorization;

namespace NT.WEB.Authorization
{
    /// <summary>
    /// Yêu cầu quyền truy cập dựa trên Resource và Action.
    /// Dùng cho endpoint-based authorization.
    /// </summary>
    public class PermissionRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// Tài nguyên cần truy cập (VD: Product, Order, Customer)
        /// </summary>
        public string Resource { get; }

        /// <summary>
        /// Hành động trên tài nguyên (VD: View, Create, Edit, Delete)
        /// </summary>
        public string Action { get; }

        public PermissionRequirement(string resource, string action)
        {
            Resource = resource ?? throw new ArgumentNullException(nameof(resource));
            Action = action ?? throw new ArgumentNullException(nameof(action));
        }
    }
}
