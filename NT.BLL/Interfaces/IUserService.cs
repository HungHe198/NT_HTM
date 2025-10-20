using System;
using System.Collections.Generic;
using NT.SHARED.Models;

namespace NT.BLL.Interfaces
{
    /// <summary>
    /// Dịch vụ quản lý người dùng (User).
    /// Cung cấp các chức năng CRUD và lấy vai trò của người dùng.
    /// </summary>
    public interface IUserService : IGenericService<User>
    {
        

        /// <summary>
        /// Lấy danh sách vai trò (roles) của một người dùng.
        /// </summary>
        /// <param name="userId">Id người dùng.</param>
        /// <returns>
        /// Tập hợp <see cref="IEnumerable{Role}"/> chứa các vai trò của người dùng.
        /// </returns>
        Task<IEnumerable<Role>> GetRolesAsync(Guid userId);
        /// <summary>
        /// Thay đổi trạng thái (Status) của người dùng.
        /// </summary>
        /// <param name="userId">Id người dùng cần thay đổi.</param>
        /// <param name="newStatus">Trạng thái mới (true = hoạt động, false = không hoạt động).</param>
        /// <returns>
        /// Trả về <see cref="User"/> sau khi cập nhật trạng thái.
        /// </returns>
        Task<User> ChangeStatusAsync(Guid userId, bool newStatus);
    }
}
