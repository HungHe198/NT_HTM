using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NT.SHARED.Models
{
    public class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Vui lòng chọn thương hiệu")]
        [Display(Name = "Thương hiệu")]
        public Guid BrandId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mã sản phẩm")]
        [MaxLength(50, ErrorMessage = "Mã sản phẩm tối đa 50 ký tự")]
        [RegularExpression(@"^[A-Za-z0-9\-_]+$", ErrorMessage = "Mã sản phẩm chỉ được chứa chữ cái, số, dấu gạch ngang và gạch dưới")]
        [Display(Name = "Mã sản phẩm")]
        public string ProductCode { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm")]
        [MaxLength(200, ErrorMessage = "Tên sản phẩm tối đa 200 ký tự")]
        [MinLength(3, ErrorMessage = "Tên sản phẩm tối thiểu 3 ký tự")]
        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; } = null!;

        [MaxLength(500, ErrorMessage = "Mô tả ngắn tối đa 500 ký tự")]
        [Display(Name = "Mô tả ngắn")]
        public string? ShortDescription { get; set; }

        [Display(Name = "Mô tả chi tiết")]
        public string? Description { get; set; }

        [MaxLength(300, ErrorMessage = "Đường dẫn ảnh đại diện tối đa 300 ký tự")]
        [Display(Name = "Ảnh đại diện")]
        public string? Thumbnail { get; set; }

        [MaxLength(50, ErrorMessage = "Trạng thái tối đa 50 ký tự")]
        [Display(Name = "Trạng thái")]
        public string? Status { get; set; }

        [MaxLength(200, ErrorMessage = "Tiêu đề SEO tối đa 200 ký tự")]
        [Display(Name = "Tiêu đề SEO")]
        public string? SeoTitle { get; set; }

        [MaxLength(300, ErrorMessage = "Mô tả SEO tối đa 300 ký tự")]
        [Display(Name = "Mô tả SEO")]
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
