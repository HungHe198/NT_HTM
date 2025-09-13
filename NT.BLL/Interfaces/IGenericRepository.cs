using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NT.BLL.Interfaces
{
    /// <summary>
    /// Interface đại diện cho một kho lưu trữ chung (generic repository) hỗ trợ các hoạt động CRUD cơ bản
    /// cho bất kỳ entity nào trong hệ thống. Interface này được thiết kế để áp dụng cho tất cả các bảng
    /// trong cơ sở dữ liệu, thúc đẩy tính tái sử dụng và tuân thủ nguyên tắc Dependency Inversion (DIP).
    /// 
    /// <para>
    /// Sử dụng: Implement interface này trong tầng DAL (ví dụ: GenericRepository&lt;T&gt;) và inject qua DI
    /// trong các service của tầng BLL. Ví dụ: <c>IGenericRepository&lt;User&gt; userRepo</c>.
    /// </para>
    /// 
    /// <example>
    /// Ví dụ sử dụng trong service:
    /// <code>
    /// public class UserService
    /// {
    ///     private readonly IGenericRepository&lt;User&gt; _repository;
    ///     
    ///     public async Task&lt;User&gt; GetUserAsync(Guid id)
    ///     {
    ///         return await _repository.GetByIdAsync(id);
    ///     }
    /// }
    /// </code>
    /// </example>
    /// 
    /// <remarks>
    /// - Interface này không xử lý các truy vấn phức tạp; sử dụng Specification pattern nếu cần.
    /// - Tất cả các phương thức đều bất đồng bộ để hỗ trợ hiệu suất cao trong ứng dụng web.
    /// - Entity T phải là một lớp (class) và thường có khóa chính là Guid.
    /// - Xem thêm: <seealso cref="IUserRepository"/> cho các phương thức cụ thể hóa.
    /// </remarks>
    /// </summary>
    /// <typeparam name="H">Loại entity đại diện cho bảng trong cơ sở dữ liệu.
    /// Entity phải có khóa chính là Guid và hỗ trợ các thuộc tính navigation nếu cần.</typeparam>
    public interface IGenericRepository<H> where H : class
    {
        /// <summary>
        /// Lấy tất cả các bản ghi từ bảng tương ứng với entity H.
        /// </summary>
        /// <returns>
        /// Một tác vụ trả về tập hợp (IEnumerable) các entity H.
        /// Nếu không có dữ liệu, trả về tập hợp rỗng.
        /// </returns>
        /// <exception cref="Exception">
        /// Ném ngoại lệ nếu có lỗi truy vấn cơ sở dữ liệu (ví dụ: kết nối thất bại).
        /// </exception>
        /// <remarks>
        /// Phương thức này không hỗ trợ phân trang; sử dụng FindAsync với predicate nếu cần lọc.
        /// Hiệu suất có thể kém với bảng lớn; khuyến nghị sử dụng Take/Skip trong implementation.
        /// </remarks>
        /// <example>
        /// <code>
        /// var allUsers = await _repository.GetAllAsync();
        /// foreach (var user in allUsers)
        /// {
        ///     // Xử lý user
        /// }
        /// </code>
        /// </example>
        Task<IEnumerable<H>> GetAllAsync();

        /// <summary>
        /// Lấy một bản ghi duy nhất dựa trên ID (khóa chính).
        /// </summary>
        /// <param name="id">ID (Guid) của entity cần lấy.</param>
        /// <returns>
        /// Một tác vụ trả về entity H nếu tìm thấy, hoặc null nếu không tồn tại.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Ném nếu id là Guid.Empty (không hợp lệ).
        /// </exception>
        /// <exception cref="NotFoundException">
        /// Ném nếu không tìm thấy entity với ID đã cho (tùy thuộc vào implementation).
        /// </exception>
        /// <remarks>
        /// Phương thức này thường eager load navigation properties nếu cần (config trong implementation).
        /// Sử dụng cho các truy vấn đơn lẻ; tránh dùng cho danh sách.
        /// </remarks>
        /// <example>
        /// <code>
        /// var user = await _repository.GetByIdAsync(Guid.Parse("123e4567-e89b-12d3-a456-426614174000"));
        /// if (user != null)
        /// {
        ///     // Xử lý user
        /// }
        /// </code>
        /// </example>
        Task<H?> GetByIdAsync(Guid id);

        /// <summary>
        /// Thêm một entity mới vào bảng tương ứng.
        /// </summary>
        /// <param name="entity">Entity H cần thêm. Không được null và phải hợp lệ (validation ở BLL).</param>
        /// <returns>
        /// Một tác vụ hoàn thành khi thêm thành công (không trả về entity đã thêm, tùy implementation có reload nếu cần).
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Ném nếu entity là null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Ném nếu entity vi phạm ràng buộc (ví dụ: khóa chính trùng lặp).
        /// </exception>
        /// <remarks>
        /// Entity sẽ được persist ngay lập tức (SaveChangesAsync gọi ngầm hoặc explicit tùy implementation).
        /// Khuyến nghị validate entity ở tầng BLL trước khi gọi.
        /// </remarks>
        /// <example>
        /// <code>
        /// var newUser = User.Create("username", "hash", Guid.Empty);
        /// await _repository.AddAsync(newUser);
        /// </code>
        /// </example>
        Task AddAsync(H entity);

        /// <summary>
        /// Cập nhật một entity hiện có trong bảng.
        /// </summary>
        /// <param name="entity">Entity H cần cập nhật. Phải có ID hợp lệ và không null.</param>
        /// <returns>
        /// Một tác vụ hoàn thành khi cập nhật thành công.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Ném nếu entity là null.
        /// </exception>
        /// <exception cref="NotFoundException">
        /// Ném nếu không tìm thấy entity với ID đã cho.
        /// </exception>
        /// <remarks>
        /// Chỉ cập nhật các thuộc tính thay đổi; implementation nên dùng Attach/Update để tối ưu.
        /// Gọi SaveChangesAsync nếu cần persist ngay.
        /// </remarks>
        /// <example>
        /// <code>
        /// var user = await _repository.GetByIdAsync(id);
        /// user.Username = "newUsername"; // Thay đổi thuộc tính
        /// await _repository.UpdateAsync(user);
        /// </code>
        /// </example>
        Task UpdateAsync(H entity);

        /// <summary>
        /// Xóa một entity dựa trên ID.
        /// </summary>
        /// <param name="id">ID (Guid) của entity cần xóa.</param>
        /// <returns>
        /// Một tác vụ hoàn thành khi xóa thành công.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Ném nếu id là Guid.Empty.
        /// </exception>
        /// <exception cref="NotFoundException">
        /// Ném nếu không tìm thấy entity để xóa.
        /// </exception>
        /// <remarks>
        /// Xóa soft delete nếu config (thêm IsDeleted flag); cascade delete tùy relationships.
        /// Kiểm tra quyền trước khi gọi ở BLL.
        /// </remarks>
        /// <example>
        /// <code>
        /// await _repository.DeleteAsync(Guid.Parse("123e4567-e89b-12d3-a456-426614174000"));
        /// </code>
        /// </example>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Lưu tất cả các thay đổi đang pending vào cơ sở dữ liệu.
        /// </summary>
        /// <returns>
        /// Một tác vụ trả về số lượng entity bị ảnh hưởng (rows affected).
        /// </returns>
        /// <exception cref="DbUpdateException">
        /// Ném nếu có lỗi lưu (ví dụ: constraint violation).
        /// </exception>
        /// <remarks>
        /// Sử dụng sau khi Add/Update/Delete nếu implementation không auto-save.
        /// Hỗ trợ transaction nếu cần (bắt đầu transaction ở service).
        /// </remarks>
        /// <example>
        /// <code>
        /// await _repository.AddAsync(entity);
        /// await _repository.SaveChangesAsync(); // Lưu thay đổi
        /// </code>
        /// </example>
        Task SaveChangesAsync();
    }
}