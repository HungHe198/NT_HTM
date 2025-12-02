using System;

namespace NT.SHARED.Models
{
    public class ProductImage
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid ProductDetailId { get; private set; }
        public string ImageUrl { get; private set; } = null!;

        private ProductImage() { }
        public static ProductImage Create(Guid productDetailId, string imageUrl)
        {
            if (productDetailId == Guid.Empty) throw new ArgumentException("ProductDetailId required");
            if (string.IsNullOrWhiteSpace(imageUrl)) throw new ArgumentException("ImageUrl required");
            return new ProductImage { ProductDetailId = productDetailId, ImageUrl = imageUrl.Trim() };
        }

        public ProductDetail ProductDetail { get; private set; } = null!;
    }
}
