using NT.SHARED.Models;

namespace NT.BLL.Interfaces
{
    /// <summary>
    /// Dịch vụ quản lý hóa đơn.
    /// </summary>
    public interface IInvoiceService : IGenericService<Order>
    {
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