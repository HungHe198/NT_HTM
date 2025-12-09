using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NT.SHARED.Models
{
    public class Cart
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Display(Name = "Tên khách hàng")]
        public Guid CustomerId { get; set; }
        [Display(Name = "Ngày tạo tài khoản")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Cart() { }
        public static Cart Create(Guid customerId)
        {
            if (customerId == Guid.Empty) throw new ArgumentException("Vui lòng chọn khách hàng phù hợp");
            return new Cart { CustomerId = customerId };
        }

        public Customer? Customer { get; set; } 
        public ICollection<CartDetail>? Items { get; set; } = new List<CartDetail>();
    }
}
