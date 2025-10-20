using NT.SHARED.Models;

namespace NT.BLL.Interfaces
{
    /// <summary>
    /// Dịch vụ quản lý vai trò người dùng (Role).
    /// Cung cấp các chức năng CRUD và lấy quyền của vai trò.
    /// </summary>
    public interface IRoleService : IGenericService<Role>
    {
        Task<IEnumerable<Permission>> GetPermissionsAsync(Guid roleId);
    }
}
