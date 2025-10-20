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
            var order = await _orderRepo.GetByIdAsync(orderId) ?? throw new InvalidOperationException("Order kh�ng t?n t?i");
            // Domain hi?n t?i ??t setter c?a Status l� private, kh�ng th? c?p nh?t t? BLL.
            // C?n b? sung ph??ng th?c domain (v� d?: order.ChangeStatus(newStatus)) ?? h?p l?.
            throw new NotSupportedException("Kh�ng th? thay ??i tr?ng th�i Order v� thu?c t�nh Status c� setter private. H�y th�m ph??ng th?c domain ?? thay ??i tr?ng th�i.");
        }
    }
}
