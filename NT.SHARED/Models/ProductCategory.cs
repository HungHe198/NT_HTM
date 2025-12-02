using System;

namespace NT.SHARED.Models
{
    public class ProductCategory
    {
        public Guid CategoryId { get; private set; }
        public Guid ProductId { get; private set; }

        private ProductCategory() { }
        public static ProductCategory Create(Guid categoryId, Guid productId)
        {
            if (categoryId == Guid.Empty || productId == Guid.Empty) throw new ArgumentException("Invalid ids");
            return new ProductCategory { CategoryId = categoryId, ProductId = productId };
        }

        public Category Category { get; private set; } = null!;
        public Product Product { get; private set; } = null!;
    }
}
