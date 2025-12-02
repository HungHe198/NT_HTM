using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NT.SHARED.Models
{
    public class PaymentMethod
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        [Required, MaxLength(100)]
        public string Name { get; private set; } = null!;
        [MaxLength(250)]
        public string? Description { get; private set; }

        private PaymentMethod() { }
        public static PaymentMethod Create(string name, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name required");
            return new PaymentMethod { Name = name.Trim(), Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim() };
        }

        public ICollection<Order> Orders { get; private set; } = new List<Order>();
    }
}
