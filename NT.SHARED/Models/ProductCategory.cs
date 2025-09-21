using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NT.SHARED.Models
{
    public class ProductCategory
    {
        public Guid ProductId { get; private set; }
        public Guid CategoryId { get; private set; }

        private ProductCategory() { }

        public static ProductCategory Create(Guid productId, Guid categoryId)
        {
            return new ProductCategory { ProductId = productId, CategoryId = categoryId };
        }

        public Product Product { get; private set; } = null!;
        public Category Category { get; private set; } = null!;
    }
}
