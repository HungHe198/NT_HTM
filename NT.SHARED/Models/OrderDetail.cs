using System;
using System.ComponentModel.DataAnnotations;

namespace NT.SHARED.Models
{
    public class OrderDetail
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Display(Name = "Đơn hàng")]
        public Guid OrderId { get; set; }
        [Display(Name = "Chi tiết sản phẩm")]
        public Guid ProductDetailId { get; set; }
        [Display(Name = "Tên sản phẩm tại thời điểm đặt")]
        public string? NameAtOrder { get; set; }
        [Display(Name = "Số lượng")]
        public int Quantity { get; set; }
        [Display(Name = "Đơn giá(tại đặt)")]
        public decimal UnitPrice { get; set; }
        [Display(Name = "Chiều dài (tại đặt)")]
        public string? LengthAtOrder { get; set; }
        [Display(Name = "Màu (tại đặt)")]
        public string? ColorAtOrder { get; set; }
        [Display(Name = "Độ cứng (tại đặt)")]
        public string? HardnessAtOrder { get; set; }
        
        [Display(Name = "Tổng tiền")]
        public decimal TotalPrice { get; set; }

        public OrderDetail() { }
        public static OrderDetail Create(Guid orderId, Guid productDetailId, int quantity, decimal unitPrice)
        {
            if (orderId == Guid.Empty || productDetailId == Guid.Empty) throw new ArgumentException("Id đơn hàng hoặc id chi tiết sản phẩm không hợp lệ");
            if (quantity <= 0) throw new ArgumentException("Số lượng phải lớn hơn 0");
            return new OrderDetail { OrderId = orderId, ProductDetailId = productDetailId, Quantity = quantity, UnitPrice = unitPrice, TotalPrice = quantity * unitPrice };
        }

        public Order? Order { get; set; }   
        public ProductDetail? ProductDetail { get; set; }   
    }
}
