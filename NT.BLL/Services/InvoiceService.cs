using System;
using System.Threading.Tasks;
using NT.BLL.Interfaces;
using NT.SHARED.Models;

namespace NT.BLL.Services
{
    public class InvoiceService : GenericService<Order>, IInvoiceService
    {
        private readonly IGenericRepository<Order> _orderRepo;

        public InvoiceService(IGenericRepository<Order> orderRepo) : base(orderRepo)
        {
            _orderRepo = orderRepo;
        }

        public async Task<Order> ConfirmPaymentAsync(Guid orderId)
        {
            var order = await _orderRepo.GetByIdAsync(orderId) ?? throw new InvalidOperationException("Order kh�ng t?n t?i");
            throw new NotSupportedException("Kh�ng th? c?p nh?t tr?ng th�i thanh to�n v� Order.Status c� setter private. H�y th�m ph??ng th?c domain ?? x�c nh?n thanh to�n.");
        }

        public async Task<bool> CancelInvoiceAsync(Guid orderId)
        {
            var order = await _orderRepo.GetByIdAsync(orderId);
            if (order == null) return false;
            throw new NotSupportedException("Kh�ng th? h?y h�a ??n v� Order.Status c� setter private. H�y th�m ph??ng th?c domain ?? h?y.");
        }
    }
}
