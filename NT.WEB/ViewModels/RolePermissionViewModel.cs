using NT.SHARED.Models;

namespace NT.WEB.ViewModels
{
    /// <summary>
    /// ViewModel cho trang phân quyền Role
    /// </summary>
    public class RolePermissionViewModel
    {
        /// <summary>
        /// Role đang được phân quyền
        /// </summary>
        public Role Role { get; set; } = null!;

        /// <summary>
        /// Tất cả Permission được nhóm theo Resource
        /// </summary>
        public Dictionary<string, List<PermissionItemViewModel>> PermissionsByResource { get; set; } = new();

        /// <summary>
        /// Các Permission ID đã được gán cho Role này
        /// </summary>
        public HashSet<Guid> AssignedPermissionIds { get; set; } = new();
    }

    /// <summary>
    /// ViewModel cho một Permission item
    /// </summary>
    public class PermissionItemViewModel
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Resource { get; set; }
        public string? Action { get; set; }
        public string? Method { get; set; }
        public bool IsAssigned { get; set; }
    }

    /// <summary>
    /// ViewModel cho danh sách Role với thông tin quyền
    /// </summary>
    public class RoleListViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int PermissionCount { get; set; }
    }

    /// <summary>
    /// Request model để cập nhật quyền cho Role
    /// </summary>
    public class UpdateRolePermissionsRequest
    {
        public Guid RoleId { get; set; }
        public List<Guid> PermissionIds { get; set; } = new();
    }
}
