using NT.BLL.Interfaces;
using NT.SHARED.Models;

namespace NT.BLL.Interfaces
{
    /// <summary>
    /// Dịch vụ quản lý danh mục sản phẩm.
    /// Cung cấp các chức năng CRUD cho bảng <see cref="Category"/>.
    /// </summary>
    public interface ICategoryService : IGenericService<Category>
    {
        // tạm thời không có gì đặc biệt
    }
}