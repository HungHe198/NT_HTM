using NT.SHARED.Models;

namespace NT.BLL.Interfaces
{
    /// <summary>
    /// Dịch vụ quản lý đơn hàng.
    /// Cung cấp các chức năng CRUD và thay đổi trạng thái cho bảng <see cref="Order"/>.
    /// </summary>
    public interface IOrderService : IGenericService<Order>
    {
        Task<Order> ChangeStatusAsync(Guid orderId, string newStatus);
    }
}