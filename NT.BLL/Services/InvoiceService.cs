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
            var order = await _orderRepo.GetByIdAsync(orderId) ?? throw new InvalidOperationException("Order không t?n t?i");
            throw new NotSupportedException("Không th? c?p nh?t tr?ng thái thanh toán vì Order.Status có setter private. Hãy thêm ph??ng th?c domain ?? xác nh?n thanh toán.");
        }

        public async Task<bool> CancelInvoiceAsync(Guid orderId)
        {
            var order = await _orderRepo.GetByIdAsync(orderId);
            if (order == null) return false;
            throw new NotSupportedException("Không th? h?y hóa ??n vì Order.Status có setter private. Hãy thêm ph??ng th?c domain ?? h?y.");
        }
    }
}
