using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NT.SHARED.Models
{
    public class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Display(Name = "Thương hiệu")]
        public Guid BrandId { get; set; }
        [Required, MaxLength(50), Display(Name = "Mã sản phẩm")]
        public string ProductCode { get; set; } = null!;
        [Required, MaxLength(200), Display(Name = "Tên sản phẩm")]
        public string Name { get; set; } = null!;
        [MaxLength(500), Display(Name = "Mô t? ng?n")]
        public string? ShortDescription { get; set; }
        [Display(Name = "Mô t?")]
        public string? Description { get; set; }
        [MaxLength(300), Display(Name = "?nh ð?i di?n")]
        public string? Thumbnail { get; set; }
        [MaxLength(50), Display(Name = "Tr?ng thái")]
        public string? Status { get; set; }
        [MaxLength(200), Display(Name = "Tiêu ð? SEO")]
        public string? SeoTitle { get; set; }
        [MaxLength(300), Display(Name = "Mô t? SEO")]
        public string? SeoDescription { get; set; }
        [Display(Name = "Ngý?i t?o")]
        public Guid? CreatedBy { get; set; }
        [Display(Name = "Ngày t?o")]
        public DateTime? CreatedDate { get; set; }
        [Display(Name = "Ngý?i c?p nh?t")]
        public Guid? UpdatedBy { get; set; }
        [Display(Name = "Ngày c?p nh?t")]
        public DateTime? UpdatedDate { get; set; }

        public Product() { }
        public static Product Create(Guid brandId, string productCode, string name)
        {
            if (brandId == Guid.Empty) throw new ArgumentException("Vui l?ng ch?n thýõng hi?u");
            if (string.IsNullOrWhiteSpace(productCode)) throw new ArgumentException("Vui l?ng nh?p m? s?n ph?m");
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Vui l?ng nh?p tên s?n ph?m");
            return new Product { BrandId = brandId, ProductCode = productCode.Trim(), Name = name.Trim() };
        }

        public Brand? Brand { get; set; }
        public ICollection<ProductCategory>? ProductCategories { get; set; } = new List<ProductCategory>();
        public ICollection<ProductDetail>? ProductDetails { get; set; } = new List<ProductDetail>();
    }
}
