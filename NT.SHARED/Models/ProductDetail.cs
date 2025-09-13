using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NT.SHARED.Models
{
    public class ProductDetail
    {
        [Required]
        public Guid ProductId { get; private set; }
        [Required]
        public Guid RodHardnessId { get; private set; }
        [Required]
        public Guid RodLengthId { get; private set; }
        [Required]
        public Guid RodColorId { get; private set; }
        [Required, Range(0, double.MaxValue)]
        public decimal Price { get; private set; } // Giá của biến thể
        [Required, Range(0, int.MaxValue)]
        public int StockQuantity { get; private set; } // Số lượng tồn kho
        [MaxLength(50)]
        public string? Material { get; private set; } // Chất liệu (VD: Carbon)

        // Private constructor for EF
        private ProductDetail() { }

        // Public static factory
        public static ProductDetail Create(Guid productId, Guid rodHardnessId, Guid rodLengthId, Guid rodColorId, decimal price, int stockQuantity, string? material = null)
        {
            if (productId == Guid.Empty) throw new ArgumentException("ID sản phẩm không hợp lệ", nameof(productId));
            if (rodHardnessId == Guid.Empty) throw new ArgumentException("ID độ cứng không hợp lệ", nameof(rodHardnessId));
            if (rodLengthId == Guid.Empty) throw new ArgumentException("ID độ dài không hợp lệ", nameof(rodLengthId));
            if (rodColorId == Guid.Empty) throw new ArgumentException("ID màu sắc không hợp lệ", nameof(rodColorId));
            if (price < 0) throw new ArgumentException("Giá phải lớn hơn hoặc bằng 0", nameof(price));
            if (stockQuantity < 0) throw new ArgumentException("Số lượng tồn kho phải lớn hơn hoặc bằng 0", nameof(stockQuantity));
            return new ProductDetail
            {
                ProductId = productId,
                RodHardnessId = rodHardnessId,
                RodLengthId = rodLengthId,
                RodColorId = rodColorId,
                Price = price,
                StockQuantity = stockQuantity,
                Material = material
            };
        }

        // Navigation
        public Product Product { get; private set; } = null!;
        public RodHardness RodHardness { get; private set; } = null!;
        public RodLength RodLength { get; private set; } = null!;
        public RodColor RodColor { get; private set; } = null!;
        public ICollection<OrderDetail> OrderDetails { get; private set; } = new List<OrderDetail>();
    }
}