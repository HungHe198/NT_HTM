using System;
using System.Collections.Generic;

namespace NT.SHARED.Models
{
    public class Cart
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CustomerId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Cart() { }
        public static Cart Create(Guid customerId)
        {
            if (customerId == Guid.Empty) throw new ArgumentException("CustomerId required");
            return new Cart { CustomerId = customerId };
        }

        public Customer Customer { get; set; } = null!;
        public ICollection<CartDetail> Items { get; set; } = new List<CartDetail>();
    }
}
