using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NT.SHARED.Models
{
    public class Brand
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        [Required, MaxLength(150)]
        public string Name { get; private set; } = null!;
        [MaxLength(200)]
        public string? Website { get; private set; }

        private Brand() { }

        public static Brand Create(string name, string? website = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Tên th??ng hi?u không ???c ?? tr?ng");
            return new Brand { Name = name.Trim(), Website = string.IsNullOrWhiteSpace(website) ? null : website };
        }

        public ICollection<Product> Products { get; private set; } = new List<Product>();
    }
}
