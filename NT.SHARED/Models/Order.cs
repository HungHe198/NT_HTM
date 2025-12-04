using System;
using System.Collections.Generic;

namespace NT.SHARED.Models
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CustomerId { get; set; }
        public Guid? CouponId { get; set; }
        public Guid PaymentMethodId { get; set; }
        public Guid? CreatedByUserId { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public string? Status { get; set; }
        public string? ShippingAddress { get; set; }
        public string? Note { get; set; }

        public Order() { }
        public static Order Create(Guid customerId, Guid paymentMethodId, decimal totalAmount, decimal finalAmount)
        {
            if (customerId == Guid.Empty) throw new ArgumentException("CustomerId required");
            if (paymentMethodId == Guid.Empty) throw new ArgumentException("PaymentMethodId required");
            return new Order { CustomerId = customerId, PaymentMethodId = paymentMethodId, TotalAmount = totalAmount, FinalAmount = finalAmount };
        }

        public Customer Customer { get; set; } = null!;
        public Voucher? Coupon { get; set; }
        public PaymentMethod PaymentMethod { get; set; } = null!;
        public ICollection<OrderDetail> Details { get; set; } = new List<OrderDetail>();
    }
}
