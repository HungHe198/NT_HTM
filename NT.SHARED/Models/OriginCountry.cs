using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NT.SHARED.Models
{
    public class OriginCountry
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required, MaxLength(100), Display(Name = "Qu?c gia xu?t x?")]
        public string Name { get; set; } = null!;
        [MaxLength(250), Display(Name = "Mô t?")]
        public string? Description { get; set; }

        public OriginCountry() { }
        public static OriginCountry Create(string name, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Vui lòng nh?p tên qu?c gia xu?t x?");
            return new OriginCountry { Name = name.Trim(), Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim() };
        }

        public ICollection<ProductDetail>? ProductDetails { get; set; } = new List<ProductDetail>();
    }
}
