using System;
using System.Collections.Generic;

namespace NT.SHARED.Models
{
    public class Order
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid CustomerId { get; private set; }
        public Guid? CouponId { get; private set; }
        public Guid PaymentMethodId { get; private set; }
        public Guid? CreatedByUserId { get; private set; }
        public DateTime CreatedTime { get; private set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; private set; }
        public decimal? DiscountAmount { get; private set; }
        public decimal FinalAmount { get; private set; }
        public string? Status { get; private set; }
        public string? ShippingAddress { get; private set; }
        public string? Note { get; private set; }

        private Order() { }
        public static Order Create(Guid customerId, Guid paymentMethodId, decimal totalAmount, decimal finalAmount)
        {
            if (customerId == Guid.Empty) throw new ArgumentException("CustomerId required");
            if (paymentMethodId == Guid.Empty) throw new ArgumentException("PaymentMethodId required");
            return new Order { CustomerId = customerId, PaymentMethodId = paymentMethodId, TotalAmount = totalAmount, FinalAmount = finalAmount };
        }

        public Customer Customer { get; private set; } = null!;
        public Voucher? Coupon { get; private set; }
        public PaymentMethod PaymentMethod { get; private set; } = null!;
        public ICollection<OrderDetail> Details { get; private set; } = new List<OrderDetail>();
    }
}
