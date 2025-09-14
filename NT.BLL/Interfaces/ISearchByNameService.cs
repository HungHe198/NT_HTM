using System.Threading.Tasks;
using System.Collections.Generic;

namespace NT.BLL.Interface
{
    /// <summary>
    /// Interface t?ng qu�t cho nghi?p v? t�m ki?m theo t�n cho c�c entity c� thu?c t�nh Name.
    /// </summary>
    /// <typeparam name="TEntity">Ki?u entity c?n t�m ki?m.</typeparam>
    public interface ISearchByNameService<TEntity>
    {
        /// <summary>
        /// T�m ki?m c�c entity theo t�n (c� th? ch?a ho?c gi?ng ho�n to�n).
        /// </summary>
        /// <param name="name">T�n c?n t�m ki?m.</param>
        /// <returns>Danh s�ch entity ph� h?p.</returns>
        Task<IEnumerable<TEntity>> SearchByNameAsync(string name);
    }
}