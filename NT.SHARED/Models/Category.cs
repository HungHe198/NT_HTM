using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NT.SHARED.Models
{
    public class Category
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required, MaxLength(150), Display(Name = "Tên danh mục")]
        public string Name { get; set; } = null!;
        [MaxLength(250), Display(Name = "Mô tả cho danh mục")]
        public string? Description { get; set; }

        public Category() { }
        public static Category Create(string name, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Tên danh mục không hợp lệ(đang trống hoặc đã tồn tại)");
            return new Category { Name = name.Trim(), Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim() };
        }

        public ICollection<ProductCategory>? ProductCategories { get; set; } = new List<ProductCategory>();
    }
}
