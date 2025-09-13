using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NT.SHARED.Models
{
    public class CategoryProduct
    {
        [Required]
        public Guid CategoryId { get; private set; }
        [Required]
        public Guid ProductId { get; private set; }

        // Private constructor for EF
        private CategoryProduct() { }

        // Public static factory
        public static CategoryProduct Create(Guid categoryId, Guid productId)
        {
            if (categoryId == Guid.Empty) throw new ArgumentException("ID danh mục không hợp lệ", nameof(categoryId));
            if (productId == Guid.Empty) throw new ArgumentException("ID sản phẩm không hợp lệ", nameof(productId));
            return new CategoryProduct
            {
                CategoryId = categoryId,
                ProductId = productId
            };
        }

        // Navigation
        public Category Category { get; private set; } = null!;
        public Product Product { get; private set; } = null!;
    }
}