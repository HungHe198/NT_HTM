using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NT.SHARED.Models
{
    public class OrderDetail
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        // Foreign key tới Order
        public Guid OrderId { get; private set; }
        public Order Order { get; private set; } = null!;

        // Foreign key tới ProductDetail
        public Guid ProductDetailId { get; private set; }
        public ProductDetail ProductDetail { get; private set; } = null!;

        [Required]
        public int Quantity { get; private set; }

        // Giá đơn vị tại thời điểm vào hóa đơn
        [Required]
        public decimal UnitPrice { get; private set; }

        // Snapshot thông tin thuộc tính tại thời điểm vào hóa đơn
        [Required, MaxLength(200)]
        public string NameAtOrder { get; private set; } = null!;
        [Required, MaxLength(50)]
        public string ColorAtOrder { get; private set; } = null!;
        [Required, MaxLength(50)]
        public string HardnessAtOrder { get; private set; } = null!;
        [Required]
        public decimal LengthAtOrder { get; private set; }

        private OrderDetail() { }

        public static OrderDetail Create(Guid id, Guid orderId, Guid productDetailId, int qty, decimal price,
                                         string nameAtOrder, string colorAtOrder, string hardnessAtOrder, decimal lengthAtOrder)
        {
            if (id == Guid.Empty) throw new ArgumentException("Id không hợp lệ", nameof(id));
            if (orderId == Guid.Empty) throw new ArgumentException("OrderId không hợp lệ", nameof(orderId));
            if (productDetailId == Guid.Empty) throw new ArgumentException("ProductDetailId không hợp lệ", nameof(productDetailId));
            if (qty <= 0) throw new ArgumentException("Số lượng phải > 0", nameof(qty));
            if (price < 0) throw new ArgumentException("Đơn giá không hợp lệ", nameof(price));
            if (string.IsNullOrWhiteSpace(nameAtOrder)) throw new ArgumentException("Tên tại thời điểm đặt hàng không được để trống", nameof(nameAtOrder));
            if (string.IsNullOrWhiteSpace(colorAtOrder)) throw new ArgumentException("Màu tại thời điểm đặt hàng không được để trống", nameof(colorAtOrder));
            if (string.IsNullOrWhiteSpace(hardnessAtOrder)) throw new ArgumentException("Độ cứng tại thời điểm đặt hàng không được để trống", nameof(hardnessAtOrder));
            if (lengthAtOrder <= 0) throw new ArgumentException("Chiều dài tại thời điểm đặt hàng phải > 0", nameof(lengthAtOrder));

            return new OrderDetail
            {
                Id = id,
                OrderId = orderId,
                ProductDetailId = productDetailId,
                Quantity = qty,
                UnitPrice = price,
                NameAtOrder = nameAtOrder.Trim(),
                ColorAtOrder = colorAtOrder.Trim(),
                HardnessAtOrder = hardnessAtOrder.Trim(),
                LengthAtOrder = lengthAtOrder
            };
        }
    }
}
