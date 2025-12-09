using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NT.SHARED.Models
{
    public class Brand
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required, MaxLength(150), Display(Name = "Tên thương hiệu")]
        public string Name { get; set; } = null!;
        [MaxLength(200), Display(Name = "Website của thương hiệu")]
        public string? Website { get; set; }

        public Brand() { }

        public static Brand Create(string name, string? website = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Vui lòng nhập tên thương hiệu");
            return new Brand { Name = name.Trim(), Website = string.IsNullOrWhiteSpace(website) ? null : website };
        }

        public ICollection<Product>? Products { get; set; } = new List<Product>();
    }
}
