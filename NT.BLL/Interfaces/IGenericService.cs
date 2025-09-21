using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NT.BLL.Interface
{
    /// <summary>
    /// Interface dịch vụ tổng quát cho các entity trong tầng Business Logic (BLL).
    /// Cung cấp các thao tác CRUD chuẩn hóa để các dịch vụ cụ thể (UserService,
    /// ProductService, InvoiceService, ...) có thể kế thừa và sử dụng.
    /// 
    /// Mục đích:
    /// - Đảm bảo tính thống nhất giữa các dịch vụ.
    /// - Giảm trùng lặp code ở tầng Service.
    /// - Cho phép mở rộng, bổ sung logic nghiệp vụ đặc thù cho từng entity.
    /// </summary>
    /// <typeparam name="TEntity">Loại entity cần quản lý.</typeparam>
    public interface IGenericService<TEntity> where TEntity : class
    {
        /// <summary>
        /// Lấy tất cả entity trong hệ thống.
        /// </summary>
        /// <returns>Danh sách tất cả entity.</returns>
        Task<IEnumerable<TEntity>> GetAllAsync();

        /// <summary>
        /// Lấy thông tin một entity theo Id.
        /// </summary>
        /// <param name="id">Id của entity cần lấy.</param>
        /// <returns>Entity tìm thấy hoặc null nếu không tồn tại.</returns>
        Task<TEntity?> GetByIdAsync(Guid id);

        /// <summary>
        /// Thêm một entity mới vào hệ thống.
        /// </summary>
        /// <param name="entity">Entity cần thêm.</param>
        /// <returns>Entity sau khi đã được thêm thành công.</returns>
        Task<TEntity> AddAsync(TEntity entity);

        /// <summary>
        /// Cập nhật thông tin cho một entity.
        /// </summary>
        /// <param name="entity">Entity với thông tin mới cần cập nhật.</param>
        /// <returns>Entity sau khi đã cập nhật thành công.</returns>
        Task<TEntity> UpdateAsync(TEntity entity);

        /// <summary>
        /// Xóa một entity theo Id.
        /// </summary>
        /// <param name="id">Id của entity cần xóa.</param>
        /// <returns>True nếu xóa thành công, ngược lại là false.</returns>
        Task<bool> DeleteAsync(Guid id);
    }
}
