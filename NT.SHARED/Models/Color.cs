using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NT.SHARED.Models
{
    public class Color
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        [Required, MaxLength(100)]
        public string Name { get; private set; } = null!;
        [MaxLength(7)]
        public string? HexCode { get; private set; }

        private Color() { }
        public static Color Create(string name, string? hexCode = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name required");
            return new Color { Name = name.Trim(), HexCode = string.IsNullOrWhiteSpace(hexCode) ? null : hexCode.Trim() };
        }

        public ICollection<ProductDetail> ProductDetails { get; private set; } = new List<ProductDetail>();
    }
}
