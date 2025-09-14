using NT.SHARED.Models;

namespace NT.BLL.Interface
{
    /// <summary>
    /// D?ch v? qu?n lý ??n hàng.
    /// </summary>
    public interface IOrderService
    {
        /// <summary>
        /// L?y danh sách t?t c? ??n hàng.
        /// </summary>
        /// <returns>Danh sách ??n hàng.</returns>
        Task<IEnumerable<Order>> GetAllAsync();

        /// <summary>
        /// L?y thông tin ??n hàng theo Id.
        /// </summary>
        /// <param name="id">Id ??n hàng.</param>
        /// <returns>??n hàng ho?c null n?u không tìm th?y.</returns>
        Task<Order?> GetByIdAsync(Guid id);

        /// <summary>
        /// Thêm m?i ??n hàng.
        /// </summary>
        /// <param name="order">Thông tin ??n hàng.</param>
        /// <param name="orderDetails">Chi ti?t ??n hàng.</param>
        /// <returns>??n hàng ?ã thêm.</returns>
        Task<Order> AddAsync(Order order, IEnumerable<OrderDetail> orderDetails);

        /// <summary>
        /// C?p nh?t thông tin ??n hàng.
        /// </summary>
        /// <param name="order">Thông tin ??n hàng c?p nh?t.</param>
        /// <returns>??n hàng sau khi c?p nh?t.</returns>
        Task<Order> UpdateAsync(Order order);

        /// <summary>
        /// Xóa ??n hàng theo Id.
        /// </summary>
        /// <param name="id">Id ??n hàng.</param>
        /// <returns>True n?u xóa thành công, ng??c l?i là false.</returns>
        Task<bool> DeleteAsync(Guid id);

        /// <summary>
        /// Thay ??i tr?ng thái ??n hàng.
        /// </summary>
        /// <param name="orderId">Id ??n hàng.</param>
        /// <param name="newStatus">Tr?ng thái m?i.</param>
        /// <returns>??n hàng sau khi thay ??i tr?ng thái.</returns>
        Task<Order> ChangeStatusAsync(Guid orderId, string newStatus);
    }
}