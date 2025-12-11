using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NT.SHARED.Models
{
    public class SurfaceFinish
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required, MaxLength(100), Display(Name = "Hoàn thiện bề mặt")]
        public string Name { get; set; } = null!;
        [MaxLength(250), Display(Name = "Mô tả")]
        public string? Description { get; set; }

        public SurfaceFinish() { }
        public static SurfaceFinish Create(string name, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Vui lòng nhập tên hoàn thiện bề mặt");
            return new SurfaceFinish { Name = name.Trim(), Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim() };
        }

        public ICollection<ProductDetail>? ProductDetails { get; set; } = new List<ProductDetail>();
    }
}
