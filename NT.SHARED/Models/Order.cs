using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NT.SHARED.Models
{
    public class Order
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        [Required]
        public Guid UserId { get; private set; } // Khách hàng đặt hàng
        [Required]
        public DateTime OrderDate { get; private set; } // Ngày đặt hàng
        [Required, Range(0, double.MaxValue)]
        public decimal TotalAmount { get; private set; } // Tổng tiền
        [Required, MaxLength(50)]
        public string Status { get; private set; } = null!; // Trạng thái (VD: Pending)
        [MaxLength(200)]
        public string? ShippingAddress { get; private set; } // Địa chỉ giao hàng

        // Private constructor for EF
        private Order() { }

        // Public static factory
        public static Order Create(Guid userId, decimal totalAmount, string status, string? shippingAddress = null)
        {
            if (userId == Guid.Empty) throw new ArgumentException("ID khách hàng không hợp lệ", nameof(userId));
            if (totalAmount < 0) throw new ArgumentException("Tổng tiền phải lớn hơn hoặc bằng 0", nameof(totalAmount));
            if (string.IsNullOrWhiteSpace(status)) throw new ArgumentException("Trạng thái không được để trống", nameof(status));
            return new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = totalAmount,
                Status = status,
                ShippingAddress = shippingAddress
            };
        }

        // Navigation
        public User User { get; private set; } = null!;
        public ICollection<OrderDetail> OrderDetails { get; private set; } = new List<OrderDetail>();
    }
}