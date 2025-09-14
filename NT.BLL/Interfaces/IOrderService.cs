using NT.SHARED.Models;

namespace NT.BLL.Interface
{
    /// <summary>
    /// D?ch v? qu?n l� ??n h�ng.
    /// </summary>
    public interface IOrderService
    {
        /// <summary>
        /// L?y danh s�ch t?t c? ??n h�ng.
        /// </summary>
        /// <returns>Danh s�ch ??n h�ng.</returns>
        Task<IEnumerable<Order>> GetAllAsync();

        /// <summary>
        /// L?y th�ng tin ??n h�ng theo Id.
        /// </summary>
        /// <param name="id">Id ??n h�ng.</param>
        /// <returns>??n h�ng ho?c null n?u kh�ng t�m th?y.</returns>
        Task<Order?> GetByIdAsync(Guid id);

        /// <summary>
        /// Th�m m?i ??n h�ng.
        /// </summary>
        /// <param name="order">Th�ng tin ??n h�ng.</param>
        /// <param name="orderDetails">Chi ti?t ??n h�ng.</param>
        /// <returns>??n h�ng ?� th�m.</returns>
        Task<Order> AddAsync(Order order, IEnumerable<OrderDetail> orderDetails);

        /// <summary>
        /// C?p nh?t th�ng tin ??n h�ng.
        /// </summary>
        /// <param name="order">Th�ng tin ??n h�ng c?p nh?t.</param>
        /// <returns>??n h�ng sau khi c?p nh?t.</returns>
        Task<Order> UpdateAsync(Order order);

        /// <summary>
        /// X�a ??n h�ng theo Id.
        /// </summary>
        /// <param name="id">Id ??n h�ng.</param>
        /// <returns>True n?u x�a th�nh c�ng, ng??c l?i l� false.</returns>
        Task<bool> DeleteAsync(Guid id);

        /// <summary>
        /// Thay ??i tr?ng th�i ??n h�ng.
        /// </summary>
        /// <param name="orderId">Id ??n h�ng.</param>
        /// <param name="newStatus">Tr?ng th�i m?i.</param>
        /// <returns>??n h�ng sau khi thay ??i tr?ng th�i.</returns>
        Task<Order> ChangeStatusAsync(Guid orderId, string newStatus);
    }
}