using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NT.SHARED.Models
{
    public class Cart
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        [Required]
        public Guid CustomerId { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        private Cart() { }

        public static Cart Create(Guid customerId)
        {
            if (customerId == Guid.Empty) throw new ArgumentException("CustomerId không h?p l?");
            return new Cart { CustomerId = customerId, CreatedAt = DateTime.UtcNow };
        }

        public Customer Customer { get; private set; } = null!;
        public ICollection<CartDetail> CartDetails { get; private set; } = new List<CartDetail>();
    }
}
