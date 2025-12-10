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
        [MaxLength(500), Display(Name = "Mô tả ngắn")]
        public string? ShortDescription { get; set; }
        [Display(Name = "Mô tả")]
        public string? Description { get; set; }
        [MaxLength(300), Display(Name = "Ảnh đại diện")]
        public string? Thumbnail { get; set; }
        [MaxLength(50), Display(Name = "Trạng thái")]
        public string? Status { get; set; }
        [MaxLength(200), Display(Name = "Tiêu đề SEO")]
        public string? SeoTitle { get; set; }
        [MaxLength(300), Display(Name = "Mô tả SEO")]
        public string? SeoDescription { get; set; }
        [Display(Name = "Người tạo")]
        public Guid? CreatedBy { get; set; }
        [Display(Name = "Ngày tạo")]
        public DateTime? CreatedDate { get; set; }
        [Display(Name = "Người cập nhật")]
        public Guid? UpdatedBy { get; set; }
        [Display(Name = "Ngày cập nhật")]
        public DateTime? UpdatedDate { get; set; }

        public Product() { }
        public static Product Create(Guid brandId, string productCode, string name)
        {
            if (brandId == Guid.Empty) throw new ArgumentException("Vui lòng chọn thương hiệu");
            if (string.IsNullOrWhiteSpace(productCode)) throw new ArgumentException("Vui lòng nhập mã sản phẩm");
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Vui lòng nhập tên sản phẩm");
            return new Product { BrandId = brandId, ProductCode = productCode.Trim(), Name = name.Trim() };
        }

        public Brand? Brand { get; set; }
        public ICollection<ProductCategory>? ProductCategories { get; set; } = new List<ProductCategory>();
        public ICollection<ProductDetail>? ProductDetails { get; set; } = new List<ProductDetail>();
    }
}
