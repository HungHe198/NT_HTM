using NT.SHARED.Models;

namespace NT.BLL.Interface
{
    /// <summary>
    /// D?ch v? qu?n l� thu?c t�nh s?n ph?m (?? c?ng, m�u s?c, ?? d�i).
    /// </summary>
    public interface IProductAttributeService : IGenericService<RodHardness>, IGenericService<RodColor>, IGenericService<RodLength>
    {
        /// <summary>
        /// L?y danh s�ch t?t c? ?? c?ng c?a c?n c�u.
        /// </summary>
        /// <returns>
        /// M?t t?p h?p <see cref="IEnumerable{RodHardness}"/> ch?a c�c gi� tr? ?? c?ng.
        /// </returns>
        Task<IEnumerable<RodHardness>> GetAllHardnessAsync();

        /// <summary>
        /// L?y danh s�ch t?t c? m�u s?c c?a c?n c�u.
        /// </summary>
        /// <returns>
        /// M?t t?p h?p <see cref="IEnumerable{RodColor}"/> ch?a c�c gi� tr? m�u s?c.
        /// </returns>
        Task<IEnumerable<RodColor>> GetAllColorsAsync();

        /// <summary>
        /// L?y danh s�ch t?t c? ?? d�i c?a c?n c�u.
        /// </summary>
        /// <returns>
        /// M?t t?p h?p <see cref="IEnumerable{RodLength}"/> ch?a c�c gi� tr? ?? d�i.
        /// </returns>
        Task<IEnumerable<RodLength>> GetAllLengthsAsync();
    }
}
