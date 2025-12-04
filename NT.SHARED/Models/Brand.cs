using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NT.SHARED.Models
{
    public class Brand
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required, MaxLength(150)]
        public string Name { get; set; } = null!;
        [MaxLength(200)]
        public string? Website { get; set; }

        public Brand() { }

        public static Brand Create(string name, string? website = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("T�n th??ng hi?u kh�ng ???c ?? tr?ng");
            return new Brand { Name = name.Trim(), Website = string.IsNullOrWhiteSpace(website) ? null : website };
        }

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
