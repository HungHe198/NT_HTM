using System;

namespace NT.SHARED.Models
{
    public class OrderDetail
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid OrderId { get; private set; }
        public Guid ProductDetailId { get; private set; }
        public string? NameAtOrder { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public string? LengthAtOrder { get; private set; }
        public string? ColorAtOrder { get; private set; }
        public string? HardnessAtOrder { get; private set; }
        public decimal? DiscountPercent { get; private set; }
        public decimal TotalPrice { get; private set; }

        private OrderDetail() { }
        public static OrderDetail Create(Guid orderId, Guid productDetailId, int quantity, decimal unitPrice)
        {
            if (orderId == Guid.Empty || productDetailId == Guid.Empty) throw new ArgumentException("Invalid ids");
            if (quantity <= 0) throw new ArgumentException("Quantity must be > 0");
            return new OrderDetail { OrderId = orderId, ProductDetailId = productDetailId, Quantity = quantity, UnitPrice = unitPrice, TotalPrice = quantity * unitPrice };
        }

        public Order Order { get; private set; } = null!;
        public ProductDetail ProductDetail { get; private set; } = null!;
    }
}
