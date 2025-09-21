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
        public Guid CustomerId { get; private set; }
        [Required]
        public Guid CreatedByUserId { get; private set; }
        public DateTime CreatedTime { get; private set; }
        public Guid? CouponId { get; private set; }
        [Required]
        public decimal TotalAmount { get; private set; }
        public decimal DiscountAmount { get; private set; }
        [Required]
        public decimal FinalAmount { get; private set; }
        [Required, MaxLength(50)]
        public string Status { get; private set; } = null!;

        private Order() { }

        public static Order Create(Guid customerId, Guid createdByUserId, DateTime createdTime, decimal total, decimal discount, decimal final, string status, Guid? couponId = null)
        {
            return new Order
            {
                CustomerId = customerId,
                CreatedByUserId = createdByUserId,
                CreatedTime = createdTime,
                TotalAmount = total,
                DiscountAmount = discount,
                FinalAmount = final,
                Status = status,
                CouponId = couponId
            };
        }

        public Customer Customer { get; private set; } = null!;
        public User CreatedByUser { get; private set; } = null!;
        public Coupon? Coupon { get; private set; }
        public ICollection<OrderDetail> OrderDetails { get; private set; } = new List<OrderDetail>();
    }
}