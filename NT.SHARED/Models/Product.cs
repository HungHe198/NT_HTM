using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NT.SHARED.Models
{
    public class Product
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid BrandId { get; private set; }
        [Required, MaxLength(50)]
        public string ProductCode { get; private set; } = null!;
        [Required, MaxLength(200)]
        public string Name { get; private set; } = null!;
        [MaxLength(500)]
        public string? ShortDescription { get; private set; }
        public string? Description { get; private set; }
        [MaxLength(300)]
        public string? Thumbnail { get; private set; }
        [MaxLength(50)]
        public string? Status { get; private set; }
        [MaxLength(200)]
        public string? SeoTitle { get; private set; }
        [MaxLength(300)]
        public string? SeoDescription { get; private set; }
        public Guid? CreatedBy { get; private set; }
        public DateTime? CreatedDate { get; private set; }
        public Guid? UpdatedBy { get; private set; }
        public DateTime? UpdatedDate { get; private set; }

        private Product() { }
        public static Product Create(Guid brandId, string productCode, string name)
        {
            if (brandId == Guid.Empty) throw new ArgumentException("BrandId required");
            if (string.IsNullOrWhiteSpace(productCode)) throw new ArgumentException("ProductCode required");
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Product name required");
            return new Product { BrandId = brandId, ProductCode = productCode.Trim(), Name = name.Trim() };
        }

        public Brand Brand { get; private set; } = null!;
        public ICollection<ProductCategory> ProductCategories { get; private set; } = new List<ProductCategory>();
        public ICollection<ProductDetail> ProductDetails { get; private set; } = new List<ProductDetail>();
    }
}
