using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NT.SHARED.Models
{
    public class Hardness
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;
        [MaxLength(250)]
        public string? Description { get; set; }

        public Hardness() { }
        public static Hardness Create(string name, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name required");
            return new Hardness { Name = name.Trim(), Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim() };
        }

        public ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();
    }
}
