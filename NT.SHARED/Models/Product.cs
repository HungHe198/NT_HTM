using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NT.SHARED.Models
{
    public class Product
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        [Required, MaxLength(100)]
        public string Name { get; private set; } = null!; // Tên sản phẩm (VD: NHẮM NGÁ TIẾU)
        [Required, MaxLength(100)]
        public string Brand { get; private set; } = null!; // Thương hiệu (VD: LAOMA)
        [MaxLength(500)]
        public string? Description { get; private set; } // Mô tả sản phẩm
        [Required, MaxLength(200)]
        public string ImageUrl { get; private set; } = null!; // Đường dẫn ảnh sản phẩm

        // Private constructor for EF
        private Product() { }

        // Public static factory
        public static Product Create(string name, string brand, string imageUrl, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Tên sản phẩm không được để trống", nameof(name));
            if (string.IsNullOrWhiteSpace(brand)) throw new ArgumentException("Thương hiệu không được để trống", nameof(brand));
            if (string.IsNullOrWhiteSpace(imageUrl)) throw new ArgumentException("Đường dẫn ảnh không được để trống", nameof(imageUrl));
            return new Product
            {
                Name = name,
                Brand = brand,
                ImageUrl = imageUrl,
                Description = description
            };
        }

        // Navigation
        public ICollection<CategoryProduct> CategoryProducts { get; private set; } = new List<CategoryProduct>();
        public ICollection<ProductDetail> ProductDetails { get; private set; } = new List<ProductDetail>();
    }
}