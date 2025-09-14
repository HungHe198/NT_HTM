using System.Threading.Tasks;
using System.Collections.Generic;

namespace NT.BLL.Interface
{
    /// <summary>
    /// Interface t?ng quát cho nghi?p v? tìm ki?m theo tên cho các entity có thu?c tính Name.
    /// </summary>
    /// <typeparam name="TEntity">Ki?u entity c?n tìm ki?m.</typeparam>
    public interface ISearchByNameService<TEntity>
    {
        /// <summary>
        /// Tìm ki?m các entity theo tên (có th? ch?a ho?c gi?ng hoàn toàn).
        /// </summary>
        /// <param name="name">Tên c?n tìm ki?m.</param>
        /// <returns>Danh sách entity phù h?p.</returns>
        Task<IEnumerable<TEntity>> SearchByNameAsync(string name);
    }
}