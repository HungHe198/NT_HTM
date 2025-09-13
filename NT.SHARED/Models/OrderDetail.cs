using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NT.SHARED.Models
{
    public class OrderDetail
    {
        [Required]
        public Guid OrderId { get; private set; }
        [Required]
        public Guid ProductId { get; private set; } // Thêm để khớp với composite key
        [Required]
        public Guid RodHardnessId { get; private set; } // Thêm để khớp với composite key
        [Required]
        public Guid RodLengthId { get; private set; } // Thêm để khớp với composite key
        [Required]
        public Guid RodColorId { get; private set; } // Thêm để khớp với composite key
        [Required, Range(1, int.MaxValue)]
        public int Quantity { get; private set; } // Số lượng
        [Required, Range(0, double.MaxValue)]
        public decimal UnitPrice { get; private set; } // Giá tại thời điểm đặt

        // Private constructor for EF
        private OrderDetail() { }

        // Public static factory
        public static OrderDetail Create(Guid orderId, Guid productId, Guid rodHardnessId, Guid rodLengthId, Guid rodColorId, int quantity, decimal unitPrice)
        {
            if (orderId == Guid.Empty) throw new ArgumentException("ID đơn hàng không hợp lệ", nameof(orderId));
            if (productId == Guid.Empty) throw new ArgumentException("ID sản phẩm không hợp lệ", nameof(productId));
            if (rodHardnessId == Guid.Empty) throw new ArgumentException("ID độ cứng không hợp lệ", nameof(rodHardnessId));
            if (rodLengthId == Guid.Empty) throw new ArgumentException("ID độ dài không hợp lệ", nameof(rodLengthId));
            if (rodColorId == Guid.Empty) throw new ArgumentException("ID màu sắc không hợp lệ", nameof(rodColorId));
            if (quantity < 1) throw new ArgumentException("Số lượng phải lớn hơn 0", nameof(quantity));
            if (unitPrice < 0) throw new ArgumentException("Giá phải lớn hơn hoặc bằng 0", nameof(unitPrice));
            return new OrderDetail
            {
                OrderId = orderId,
                ProductId = productId,
                RodHardnessId = rodHardnessId,
                RodLengthId = rodLengthId,
                RodColorId = rodColorId,
                Quantity = quantity,
                UnitPrice = unitPrice
            };
        }

        // Navigation
        public Order Order { get; private set; } = null!;
        public ProductDetail ProductDetail { get; private set; } = null!;
    }
}