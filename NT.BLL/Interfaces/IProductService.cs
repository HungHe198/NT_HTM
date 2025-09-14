using NT.SHARED.Models;

namespace NT.BLL.Interface
{
    /// <summary>
    /// D?ch v? qu?n l� s?n ph?m (c?n c�u).
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// L?y danh s�ch t?t c? s?n ph?m.
        /// </summary>
        /// <returns>Danh s�ch s?n ph?m.</returns>
        Task<IEnumerable<Product>> GetAllAsync();

        /// <summary>
        /// L?y th�ng tin s?n ph?m theo Id.
        /// </summary>
        /// <param name="id">Id s?n ph?m.</param>
        /// <returns>S?n ph?m ho?c null n?u kh�ng t�m th?y.</returns>
        Task<Product?> GetByIdAsync(Guid id);

        /// <summary>
        /// Th�m m?i s?n ph?m.
        /// </summary>
        /// <param name="product">Th�ng tin s?n ph?m.</param>
        /// <returns>S?n ph?m ?� th�m.</returns>
        Task<Product> AddAsync(Product product);

        /// <summary>
        /// C?p nh?t th�ng tin s?n ph?m.
        /// </summary>
        /// <param name="product">Th�ng tin s?n ph?m c?p nh?t.</param>
        /// <returns>S?n ph?m sau khi c?p nh?t.</returns>
        Task<Product> UpdateAsync(Product product);

        /// <summary>
        /// X�a s?n ph?m theo Id.
        /// </summary>
        /// <param name="id">Id s?n ph?m.</param>
        /// <returns>True n?u x�a th�nh c�ng, ng??c l?i l� false.</returns>
        Task<bool> DeleteAsync(Guid id);
    }
}