using NT.SHARED.Models;

namespace NT.BLL.Interface
{
    /// <summary>
    /// D?ch v? qu?n lý thu?c tính s?n ph?m (?? c?ng, màu s?c, ?? dài).
    /// </summary>
    public interface IProductAttributeService : IGenericService<RodHardness>, IGenericService<RodColor>, IGenericService<RodLength>
    {
        /// <summary>
        /// L?y danh sách t?t c? ?? c?ng c?a c?n câu.
        /// </summary>
        /// <returns>
        /// M?t t?p h?p <see cref="IEnumerable{RodHardness}"/> ch?a các giá tr? ?? c?ng.
        /// </returns>
        Task<IEnumerable<RodHardness>> GetAllHardnessAsync();

        /// <summary>
        /// L?y danh sách t?t c? màu s?c c?a c?n câu.
        /// </summary>
        /// <returns>
        /// M?t t?p h?p <see cref="IEnumerable{RodColor}"/> ch?a các giá tr? màu s?c.
        /// </returns>
        Task<IEnumerable<RodColor>> GetAllColorsAsync();

        /// <summary>
        /// L?y danh sách t?t c? ?? dài c?a c?n câu.
        /// </summary>
        /// <returns>
        /// M?t t?p h?p <see cref="IEnumerable{RodLength}"/> ch?a các giá tr? ?? dài.
        /// </returns>
        Task<IEnumerable<RodLength>> GetAllLengthsAsync();
    }
}
