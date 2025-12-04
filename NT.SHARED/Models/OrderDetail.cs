using System;

namespace NT.SHARED.Models
{
    public class OrderDetail
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid OrderId { get; set; }
        public Guid ProductDetailId { get; set; }
        public string? NameAtOrder { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string? LengthAtOrder { get; set; }
        public string? ColorAtOrder { get; set; }
        public string? HardnessAtOrder { get; set; }
        public decimal? DiscountPercent { get; set; }
        public decimal TotalPrice { get; set; }

        public OrderDetail() { }
        public static OrderDetail Create(Guid orderId, Guid productDetailId, int quantity, decimal unitPrice)
        {
            if (orderId == Guid.Empty || productDetailId == Guid.Empty) throw new ArgumentException("Invalid ids");
            if (quantity <= 0) throw new ArgumentException("Quantity must be > 0");
            return new OrderDetail { OrderId = orderId, ProductDetailId = productDetailId, Quantity = quantity, UnitPrice = unitPrice, TotalPrice = quantity * unitPrice };
        }

        public Order Order { get; set; } = null!;
        public ProductDetail ProductDetail { get; set; } = null!;
    }
}
