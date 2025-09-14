using NT.SHARED.Models;

namespace NT.BLL.Interface
{
    /// <summary>
    /// D?ch v? qu?n lý s?n ph?m (c?n câu).
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// L?y danh sách t?t c? s?n ph?m.
        /// </summary>
        /// <returns>Danh sách s?n ph?m.</returns>
        Task<IEnumerable<Product>> GetAllAsync();

        /// <summary>
        /// L?y thông tin s?n ph?m theo Id.
        /// </summary>
        /// <param name="id">Id s?n ph?m.</param>
        /// <returns>S?n ph?m ho?c null n?u không tìm th?y.</returns>
        Task<Product?> GetByIdAsync(Guid id);

        /// <summary>
        /// Thêm m?i s?n ph?m.
        /// </summary>
        /// <param name="product">Thông tin s?n ph?m.</param>
        /// <returns>S?n ph?m ?ã thêm.</returns>
        Task<Product> AddAsync(Product product);

        /// <summary>
        /// C?p nh?t thông tin s?n ph?m.
        /// </summary>
        /// <param name="product">Thông tin s?n ph?m c?p nh?t.</param>
        /// <returns>S?n ph?m sau khi c?p nh?t.</returns>
        Task<Product> UpdateAsync(Product product);

        /// <summary>
        /// Xóa s?n ph?m theo Id.
        /// </summary>
        /// <param name="id">Id s?n ph?m.</param>
        /// <returns>True n?u xóa thành công, ng??c l?i là false.</returns>
        Task<bool> DeleteAsync(Guid id);
    }
}