using NT.SHARED.Models;

namespace NT.BLL.Interface
{
    /// <summary>
    /// Dịch vụ quản lý vai trò người dùng (Role).
    /// Cung cấp các chức năng CRUD và lấy quyền của vai trò.
    /// </summary>
    public interface IRoleService : IGenericService<Role>
    {
        

        /// <summary>
        /// Lấy danh sách quyền (permissions) gắn với một vai trò.
        /// </summary>
        /// <param name="roleId">Id vai trò.</param>
        /// <returns>
        /// Tập hợp <see cref="IEnumerable{Permission}"/> chứa các quyền của vai trò.
        /// </returns>
        Task<IEnumerable<Permission>> GetPermissionsAsync(Guid roleId);
    }
}
