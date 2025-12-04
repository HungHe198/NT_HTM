using System;

namespace NT.SHARED.Models
{
    public class ProductCategory
    {
        public Guid CategoryId { get; set; }
        public Guid ProductId { get; set; }

        public ProductCategory() { }
        public static ProductCategory Create(Guid categoryId, Guid productId)
        {
            if (categoryId == Guid.Empty || productId == Guid.Empty) throw new ArgumentException("Invalid ids");
            return new ProductCategory { CategoryId = categoryId, ProductId = productId };
        }

        public Category Category { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}
