using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NT.BLL.Interfaces;
using NT.SHARED.Models;

namespace NT.BLL.Services
{
    public class OrderService : GenericService<Order>, IOrderService
    {
        private readonly IGenericRepository<Order> _orderRepo;

        public OrderService(IGenericRepository<Order> orderRepo) : base(orderRepo)
        {
            _orderRepo = orderRepo;
        }

        public async Task<Order> ChangeStatusAsync(Guid orderId, string newStatus)
        {
            var order = await _orderRepo.GetByIdAsync(orderId) ?? throw new InvalidOperationException("Order không t?n t?i");
            // Domain hi?n t?i ??t setter c?a Status là private, không th? c?p nh?t t? BLL.
            // C?n b? sung ph??ng th?c domain (ví d?: order.ChangeStatus(newStatus)) ?? h?p l?.
            throw new NotSupportedException("Không th? thay ??i tr?ng thái Order vì thu?c tính Status có setter private. Hãy thêm ph??ng th?c domain ?? thay ??i tr?ng thái.");
        }
    }
}
