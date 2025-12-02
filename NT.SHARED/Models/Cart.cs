using System;
using System.Collections.Generic;

namespace NT.SHARED.Models
{
    public class Cart
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid CustomerId { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        private Cart() { }
        public static Cart Create(Guid customerId)
        {
            if (customerId == Guid.Empty) throw new ArgumentException("CustomerId required");
            return new Cart { CustomerId = customerId };
        }

        public Customer Customer { get; private set; } = null!;
        public ICollection<CartDetail> Items { get; private set; } = new List<CartDetail>();
    }
}
