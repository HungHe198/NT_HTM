using System.Threading.Tasks;
using System.Collections.Generic;

namespace NT.BLL.Interface
{
    /// <summary>
    /// Interface tổng quát cho nghiệp vụ tìm kiếm theo tên
    /// cho các entity có thuộc tính <c>Name</c>.
    /// </summary>
    /// <typeparam name="TEntity">Kiểu entity cần tìm kiếm.</typeparam>
    public interface ISearchByNameService<TEntity>
    {
        /// <summary>
        /// Tìm kiếm các entity theo tên (có thể chứa hoặc giống hoàn toàn).
        /// </summary>
        /// <param name="name">Tên cần tìm kiếm.</param>
        /// <returns>
        /// Danh sách <typeparamref name="TEntity"/> phù hợp với từ khóa.
        /// </returns>
        Task<IEnumerable<TEntity>> SearchByNameAsync(string name);
    }
}
