using NT.SHARED.Models;
using NT.BLL.Interfaces;
namespace NT.BLL.Interface
{
   

   
        /// <summary>
        /// Dịch vụ quản lý đơn hàng.
        /// Cung cấp các chức năng CRUD và thay đổi trạng thái cho bảng <see cref="Order"/>.
        /// </summary>
        public interface IOrderService : IGenericService<Order>
    {
           
            /// <summary>
            /// Thay đổi trạng thái của đơn hàng.
            /// </summary>
            /// <param name="orderId">Id của đơn hàng cần thay đổi trạng thái.</param>
            /// <param name="newStatus">Trạng thái mới cần cập nhật.</param>
            /// <returns>
            /// Đối tượng <see cref="Order"/> sau khi thay đổi trạng thái.
            /// </returns>
            Task<Order> ChangeStatusAsync(Guid orderId, string newStatus);
        }
    
}