using NT.BLL.Interface;
using NT.SHARED.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NT.BLL.Services
{
    /// <summary>
    /// Service logic cho nghiệp vụ quản lý đơn hàng.
    /// </summary>
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
            => await _orderRepository.GetAllAsync();

        public async Task<Order?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id đơn hàng không hợp lệ", nameof(id));
            return await _orderRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Tạo mới đơn hàng cùng các chi tiết đơn hàng.
        /// </summary>
        public async Task<Order> AddAsync(Order order, IEnumerable<OrderDetail> orderDetails)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));
            if (order.UserId == Guid.Empty)
                throw new ArgumentException("Id khách hàng không hợp lệ");
            if (orderDetails == null)
                throw new ArgumentNullException(nameof(orderDetails));
            if (!orderDetails.Any())
                throw new ArgumentException("Đơn hàng phải có ít nhất một sản phẩm");

            // Validate từng OrderDetail
            foreach (var detail in orderDetails)
            {
                if (detail.ProductId == Guid.Empty)
                    throw new ArgumentException("Sản phẩm trong đơn hàng không hợp lệ");
                if (detail.Quantity < 1)
                    throw new ArgumentException("Số lượng sản phẩm phải lớn hơn 0");
                if (detail.UnitPrice < 0)
                    throw new ArgumentException("Giá sản phẩm không hợp lệ");
            }

            // Đẩy nghiệp vụ xuống repository (có thể là transaction)
            return await _orderRepository.AddWithDetailsAsync(order, orderDetails);
        }

        public async Task<Order> UpdateAsync(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));
            if (order.Id == Guid.Empty)
                throw new ArgumentException("Id đơn hàng không hợp lệ");
            return await _orderRepository.UpdateAsync(order);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id đơn hàng không hợp lệ", nameof(id));
            return await _orderRepository.DeleteAsync(id);
        }

        public async Task<Order> ChangeStatusAsync(Guid orderId, string newStatus)
        {
            if (orderId == Guid.Empty)
                throw new ArgumentException("Id đơn hàng không hợp lệ", nameof(orderId));
            if (string.IsNullOrWhiteSpace(newStatus))
                throw new ArgumentException("Trạng thái mới không hợp lệ", nameof(newStatus));
            // Validate logic chuyển trạng thái nếu cần
            return await _orderRepository.ChangeStatusAsync(orderId, newStatus);
        }
    }
}