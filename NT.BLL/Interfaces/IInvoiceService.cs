using NT.SHARED.Models;

namespace NT.BLL.Interface
{
    /// <summary>
    /// Dịch vụ quản lý hóa đơn.
    /// </summary>
    public interface IInvoiceService
    {
        /// <summary>
        /// Lấy danh sách hóa đơn.
        /// </summary>
        /// <returns>Danh sách hóa đơn.</returns>
        Task<IEnumerable<Order>> GetAllInvoicesAsync();

        /// <summary>
        /// Lấy hóa đơn theo Id.
        /// </summary>
        /// <param name="id">Id hóa đơn.</param>
        /// <returns>Hóa đơn hoặc null nếu không tìm thấy.</returns>
        Task<Order?> GetInvoiceByIdAsync(Guid id);

        /// <summary>
        /// Xác nhận thanh toán hóa đơn.
        /// </summary>
        /// <param name="orderId">Id hóa đơn.</param>
        /// <returns>Hóa đơn sau khi xác nhận thanh toán.</returns>
        Task<Order> ConfirmPaymentAsync(Guid orderId);

        /// <summary>
        /// Hủy hóa đơn.
        /// </summary>
        /// <param name="orderId">Id hóa đơn.</param>
        /// <returns>True nếu hủy thành công, ngược lại là false.</returns>
        Task<bool> CancelInvoiceAsync(Guid orderId);
    }
}