using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NT.SHARED.Models
{
    public class Length
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required, MaxLength(100), Display(Name = "Chiều dài")]
        public string Name { get; set; } = null!;
        [MaxLength(250), Display(Name = "Mô tả")]
        public string? Description { get; set; }

        public Length() { }
        public static Length Create(string name, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Vui lòng nhập giá trị chiều dài");
            return new Length { Name = name.Trim(), Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim() };
        }

        public ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();
    }
}
