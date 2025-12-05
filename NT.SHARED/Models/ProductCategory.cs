using System;
using System.ComponentModel.DataAnnotations;

namespace NT.SHARED.Models
{
    public class ProductCategory
    {
        [Display(Name = "Danh mục")]
        public Guid CategoryId { get; set; }
        [Display(Name = "Sản phẩm")]
        public Guid ProductId { get; set; }

        public ProductCategory() { }
        public static ProductCategory Create(Guid categoryId, Guid productId)
        {
            if (categoryId == Guid.Empty || productId == Guid.Empty) throw new ArgumentException("danh mục hoặc sản phẩm không hợp lệ");
            return new ProductCategory { CategoryId = categoryId, ProductId = productId };
        }

        public Category Category { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}
