using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NT.SHARED.Models
{
    public class Coupon
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        [Required, MaxLength(50)]
        public string Code { get; private set; } = null!;
        [Required]
        public decimal DiscountAmount { get; private set; }
        [Required]
        public DateTime ExpiryDate { get; private set; }

        private Coupon() { }

        public static Coupon Create(string code, decimal discount, DateTime expiry)
        {
            return new Coupon { Code = code, DiscountAmount = discount, ExpiryDate = expiry };
        }

        public ICollection<Order> Orders { get; private set; } = new List<Order>();
    }
}
