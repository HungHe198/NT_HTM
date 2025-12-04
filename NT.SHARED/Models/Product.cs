using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NT.SHARED.Models
{
    public class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid BrandId { get; set; }
        [Required, MaxLength(50)]
        public string ProductCode { get; set; } = null!;
        [Required, MaxLength(200)]
        public string Name { get; set; } = null!;
        [MaxLength(500)]
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }
        [MaxLength(300)]
        public string? Thumbnail { get; set; }
        [MaxLength(50)]
        public string? Status { get; set; }
        [MaxLength(200)]
        public string? SeoTitle { get; set; }
        [MaxLength(300)]
        public string? SeoDescription { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public Product() { }
        public static Product Create(Guid brandId, string productCode, string name)
        {
            if (brandId == Guid.Empty) throw new ArgumentException("BrandId required");
            if (string.IsNullOrWhiteSpace(productCode)) throw new ArgumentException("ProductCode required");
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Product name required");
            return new Product { BrandId = brandId, ProductCode = productCode.Trim(), Name = name.Trim() };
        }

        public Brand Brand { get; set; } = null!;
        public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
        public ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();
    }
}
