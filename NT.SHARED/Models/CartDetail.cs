using System;

namespace NT.SHARED.Models
{
    public class CartDetail
    {
        public Guid CartId { get; set; }
        public Guid ProductDetailId { get; set; }
        public int Quantity { get; set; }

        public CartDetail() { }
        public static CartDetail Create(Guid cartId, Guid productDetailId, int quantity)
        {
            if (cartId == Guid.Empty || productDetailId == Guid.Empty) throw new ArgumentException("Invalid ids");
            if (quantity <= 0) throw new ArgumentException("Quantity must be > 0");
            return new CartDetail { CartId = cartId, ProductDetailId = productDetailId, Quantity = quantity };
        }

        public Cart Cart { get; set; } = null!;
        public ProductDetail ProductDetail { get; set; } = null!;
    }
}
